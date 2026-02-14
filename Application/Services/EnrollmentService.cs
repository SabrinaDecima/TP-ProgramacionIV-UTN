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

            if (!await CanEnrollAsync(user))
                return new EnrollmentResponse { Success = false, Message = "No puede inscribirse según su plan. Verifique su suscripción activa." };

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

        private async Task<bool> CanEnrollAsync(User user)
        {
            // Obtener suscripción activa del usuario
            var activeSubscription = await _subscriptionRepository.GetActiveSubscriptionAsync(user.Id);

            // Si no tiene suscripción activa, no puede inscribirse
            if (activeSubscription == null || !activeSubscription.IsActive ||
                activeSubscription.EndDate < DateTime.Now)
                return false;

            // Validar límite según el plan
            var limit = activeSubscription.PlanId switch
            {
                1 => 5,    // Plan Basic: máx 5 clases
                2 => 10,   // Plan Premium: máx 10 clases
                3 => 15,   // Plan Elite: máx 15 clases
                _ => 0
            };

            // Retorna true si puede inscribirse (clases actuales < límite)
            return (user.GymClasses?.Count ?? 0) < limit;
        }
    }
}


