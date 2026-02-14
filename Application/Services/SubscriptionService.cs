using Application.Abstraction;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;
        public SubscriptionService(
            ISubscriptionRepository subscriptionRepository,
            IPlanRepository planRepository,
            IUserRepository userRepository,
            IPaymentRepository paymentRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _planRepository = planRepository;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<bool> CreateSubscriptionAsync(int userId, int planId, decimal amount, string paymentMethod)
        {
            
            var user =  _userRepository.GetById(userId);

            var plan = _planRepository.GetPlanById(planId);

            if (user == null || plan == null)
                return false;


            var existingSub = await _subscriptionRepository.GetActiveSubscriptionAsync(userId);

            if(existingSub != null)
            {
                throw new InvalidOperationException("El usuario ya tiene una suscripción activa.");
            }

            var subscription = new Subscription
            {
                UserId = userId,
                PlanId = planId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                IsActive = true
            };

            var created = await _subscriptionRepository.CreateAsync(subscription);
            if (!created) return false;

            // registro de pago
            await RegisterPaymentAsync(subscription, amount, paymentMethod);

            return true;
        }

        public async Task<bool> RenewSubscriptionAsync(int userId, int planId, decimal amount, string paymentMethod)
        {
            
            var user =  _userRepository.GetById(userId);
            var plan = _planRepository.GetPlanById(planId);
            if (user == null || plan == null)
                return false;

           

            // Obtener suscripción activa
            var activeSubscription = await _subscriptionRepository.GetActiveSubscriptionAsync(userId);

            // Si está en deuda (vencida), la nueva empieza hoy.
            DateTime startDate = (activeSubscription != null && activeSubscription.EndDate > DateTime.UtcNow)
                                 ? activeSubscription.EndDate
                                 : DateTime.UtcNow;

            // Crear nueva suscripción
            var newSubscription = new Subscription
            {
                UserId = userId,
                PlanId = planId,
                StartDate = startDate,
                EndDate = startDate.AddDays(30),
                IsActive = true
            };

            var created = await _subscriptionRepository.CreateAsync(newSubscription);
            if (!created) return false;

            await RegisterPaymentAsync(newSubscription, amount, paymentMethod);

            return true;
        }

        private async Task RegisterPaymentAsync(Subscription sub, decimal amount, string method)
        {
            var payment = new Payment
            {
                UserId = sub.UserId,
                SubscriptionId = sub.Id,
                Monto = amount,
                Fecha = DateTime.UtcNow,
                Pagado = true,
                MetodoPago = method
            };
             await _paymentRepository.CreateAsync(payment);
        }

        public async Task<Subscription?> GetActiveSubscriptionAsync(int userId)
        {
            return await _subscriptionRepository.GetActiveSubscriptionAsync(userId);
        }

        public async Task<List<Subscription>> GetUserSubscriptionsAsync(int userId)
        {
            return await _subscriptionRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> CancelSubscriptionAsync(int subscriptionId)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
            if (subscription == null)
                return false;

            subscription.IsActive = false;
            
            return await _subscriptionRepository.UpdateAsync(subscription); 
        }
        public async Task ProcessExpirationAsync()
        {
            var expiredSubs = await _subscriptionRepository.GetExpiredSubscriptionsAsync();
            foreach (var sub in expiredSubs)
            {
                sub.IsActive = false;
                await _subscriptionRepository.UpdateAsync(sub);
            }
        }
    }
}