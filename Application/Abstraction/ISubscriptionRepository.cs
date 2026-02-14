using Domain.Entities;

namespace Application.Abstraction
{
    public interface ISubscriptionRepository
    {
        Task<Subscription?> GetActiveSubscriptionAsync(int userId);
        Task<Subscription?> GetByIdAsync(int id);
        Task<List<Subscription>> GetByUserIdAsync(int userId);
        Task<List<Subscription>> GetExpiredSubscriptionsAsync();
        Task<bool> CreateAsync(Subscription subscription);
        Task<bool> UpdateAsync(Subscription subscription);
        Task<bool> DeleteAsync(int id);

    }
}