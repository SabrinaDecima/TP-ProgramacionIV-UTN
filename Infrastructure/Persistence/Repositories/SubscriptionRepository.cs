using Application.Abstraction;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly GymDbContext _context;

        public SubscriptionRepository(GymDbContext context)
        {
            _context = context;
        }

        public Subscription? GetActiveSubscription(int userId)
        {
            return _context.Subscriptions
                .FirstOrDefault(s => s.UserId == userId && s.IsActive && !s.IsExpired);
        }

        public Subscription? GetById(int id)
        {
            return _context.Subscriptions.FirstOrDefault(s => s.Id == id);
        }

        public List<Subscription> GetByUserId(int userId)
        {
            return _context.Subscriptions
                .Where(s => s.UserId == userId)
                .ToList();
        }

        public bool Create(Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);
            return _context.SaveChanges() > 0;
        }

        public bool Update(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var subscription = GetById(id);
            if (subscription == null)
                return false;

            _context.Subscriptions.Remove(subscription);
            return _context.SaveChanges() > 0;
        }
    }
}