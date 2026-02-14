using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISubscriptionService
    {

        Task<bool> CreateSubscriptionAsync(int userId, int planId, decimal amount, string paymentMethod);

        Task<bool> RenewSubscriptionAsync(int userId, int planId, decimal amount, string paymentMethod);

 
        Task<Subscription?> GetActiveSubscriptionAsync(int userId);

 
        Task<List<Subscription>> GetUserSubscriptionsAsync(int userId);

  
        Task<bool> CancelSubscriptionAsync(int subscriptionId);

        // Metodo para el worker de expiracion
        Task ProcessExpirationAsync();
    }
}