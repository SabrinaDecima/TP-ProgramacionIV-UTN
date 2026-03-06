using Application.Abstraction;
using Application.Interfaces;
using Contracts.Enrollment.Request;
using Contracts.Enrollment.Response;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGymClassRepository _gymClassRepository;
        private readonly IHistoricalRepository _historicalRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public EnrollmentService(
            IUserRepository userRepository,
            IGymClassRepository gymClassRepository,
            IHistoricalRepository historicalRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _userRepository = userRepository;
            _gymClassRepository = gymClassRepository;
            _historicalRepository = historicalRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<EnrollmentResponse> EnrollUserAsync(EnrollUserRequest request)
        {   
            var user = _userRepository.GetUserWithClasses(request.UserId);
            if (user == null)
                return new EnrollmentResponse { Success = false, Message = "Usuario no encontrado." };

            var gymClass = _gymClassRepository.GetById(request.GymClassId);
            if (gymClass == null)
                return new EnrollmentResponse { Success = false, Message = "Clase no encontrada." };

            var enrollmentCheck = await CanEnrollAsync(user);
            if (!enrollmentCheck.CanEnroll)
                return new EnrollmentResponse { Success = false, Message = enrollmentCheck.Message };

            if (gymClass.Users.Count >= gymClass.MaxCapacityUser)
                return new EnrollmentResponse { Success = false, Message = "La clase está completa." };

            if (user.GymClasses.Any(c => c.Id == gymClass.Id))
                return new EnrollmentResponse { Success = false, Message = "Ya está inscrito en esta clase." };

            user.GymClasses.Add(gymClass);
            gymClass.Users.Add(user);

            var updated = _userRepository.UpdateUser(user.Id, user);

            if (updated)
            {
                var classTime = TimeSpan.Parse(gymClass.Hora);
                var classDate = new DateTime(2000, 1, (int)gymClass.Dia + 1, classTime.Hours, classTime.Minutes, 0);

                _historicalRepository.Add(new Historical
                {
                    UserId = user.Id,
                    GymClassId = gymClass.Id,
                    ClassDate = classDate,
                    ActionDate = DateTime.Now,
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

        public async Task<EnrollmentResponse> UnenrollUserAsync(EnrollUserRequest request)
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
                    historical.ActionDate = DateTime.Now;

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

        private async Task<(bool CanEnroll, string Message)> CanEnrollAsync(User user)
        {
            var activeSubscription = await _subscriptionRepository.GetActiveSubscriptionAsync(user.Id);

            if (activeSubscription == null || !activeSubscription.IsActive ||
                activeSubscription.EndDate < DateTime.Now)
                return (false, "No tenés una suscripción activa. Por favor, abona tu plan para reservar clases.");

            var limit = activeSubscription.PlanId switch
            {
                1 => 5,
                2 => 10,
                3 => 15,
                _ => 0
            };

            var currentEnrollments = user.GymClasses?.Count ?? 0;

            if (currentEnrollments >= limit)
            {
                var planName = activeSubscription.Plan?.Tipo.ToString() ?? "tu plan";
                return (false, $"Límite de clases alcanzado. Tu plan {planName} incluye {limit} clases mensuales y ya reservaste {currentEnrollments}.");
            }

            return (true, string.Empty);
        }
    }
}


