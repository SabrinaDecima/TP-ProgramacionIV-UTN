using Domain.Entities;

namespace Application.Abstraction
{
    public interface ISubscriptionRepository
    {
        Subscription? GetActiveSubscription(int userId);
        Subscription? GetById(int id);
        List<Subscription> GetByUserId(int userId);
        bool Create(Subscription subscription);
        bool Update(Subscription subscription);
        bool Delete(int id);
    }
}