using Application.Abstraction;
using Application.Interfaces;
using Contracts.Enrollment.Request;
using Contracts.Enrollment.Response;
using Domain.Entities;

namespace Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGymClassRepository _gymClassRepository;
        private readonly IHistoricalRepository _historicalRepository;

        public EnrollmentService(
            IUserRepository userRepository,
            IGymClassRepository gymClassRepository,
            IHistoricalRepository historicalRepository)
        {
            _userRepository = userRepository;
            _gymClassRepository = gymClassRepository;
            _historicalRepository = historicalRepository;
        }

        public EnrollmentResponse EnrollUser(EnrollUserRequest request)
        {
            var user = _userRepository.GetUserWithClasses(request.UserId);
            if (user == null)
                return new EnrollmentResponse { Success = false, Message = "Usuario no encontrado." };

            var gymClass = _gymClassRepository.GetById(request.GymClassId);
            if (gymClass == null)
                return new EnrollmentResponse { Success = false, Message = "Clase no encontrada." };

            if (!CanEnroll(user))
                return new EnrollmentResponse { Success = false, Message = "No puede inscribirse según su plan." };

            if (gymClass.Users.Count >= gymClass.MaxCapacityUser)
                return new EnrollmentResponse { Success = false, Message = "La clase está completa." };

            if (user.GymClasses.Any(c => c.Id == gymClass.Id))
                return new EnrollmentResponse { Success = false, Message = "Ya está inscrito en esta clase." };

           
            user.GymClasses.Add(gymClass);
            gymClass.Users.Add(user);

            var updated = _userRepository.UpdateUser(user.Id, user);

            if (updated)
            {
                // Convertir Dia (enum) + Hora (string) a DateTime simple
                var classTime = TimeSpan.Parse(gymClass.Hora); // "HH:mm"
                var classDate = new DateTime(2000, 1, (int)gymClass.Dia + 1, classTime.Hours, classTime.Minutes, 0);

                _historicalRepository.Add(new Historical
                {
                    UserId = user.Id,
                    GymClassId = gymClass.Id,
                    ClassDate = classDate,
                    ActionDate = DateTime.UtcNow,
                    Status = HistoricalStatus.Active
                });
            }

            var updatedClass = _gymClassRepository.GetByIdWithUsers(request.GymClassId);

            return new EnrollmentResponse
            {
                Success = updated,
                Message = updated ? "Inscripción exitosa." : "Error al inscribir.",
                GymClassId = request.GymClassId,
                IsReserved = updated ? true : (bool?)null,
                CurrentEnrollments = updatedClass?.Users.Count,
                MaxCapacity = updatedClass?.MaxCapacityUser
            };
        }

        public EnrollmentResponse UnenrollUser(EnrollUserRequest request)
        {
            var user = _userRepository.GetUserWithClasses(request.UserId);
            var gymClass = _gymClassRepository.GetById(request.GymClassId);

            if (user == null || gymClass == null)
                return new EnrollmentResponse { Success = false, Message = "Usuario o clase no encontrada." };

            user?.GymClasses?.RemoveAll(c => c.Id == gymClass.Id);
            gymClass.Users.RemoveAll(u => u.Id == user.Id);

            var updated = _userRepository.UpdateUser(user.Id, user);

            if (updated)
            {
                var historical = _historicalRepository.GetActive(user.Id, gymClass.Id);

                if (historical != null)
                {
                    historical.Status = HistoricalStatus.Cancelled;
                    historical.ActionDate = DateTime.UtcNow;

                    _historicalRepository.Update(historical);
                }
            }



            var updatedClass = _gymClassRepository.GetByIdWithUsers(request.GymClassId);

            return new EnrollmentResponse
            {
                Success = updated,
                Message = updated ? "Baja exitosa." : "Error al dar de baja.",
                GymClassId = request.GymClassId,
                IsReserved = updated ? false : (bool?)null,
                CurrentEnrollments = updatedClass?.Users.Count,
                MaxCapacity = updatedClass?.MaxCapacityUser
            };
        }

        private bool CanEnroll(User user)
        {
            var limit = user.PlanId switch
            {
                1 => 5,
                2 => 10,
                3 => 15,
                _ => 0
            };

            return (user.GymClasses?.Count ?? 0) < limit;
        }
    }
}
