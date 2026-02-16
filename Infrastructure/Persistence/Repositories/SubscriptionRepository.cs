using Application.Abstraction;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly GymDbContext _context;

        public SubscriptionRepository(GymDbContext context)
        {
            _context = context;
        }

        public async Task<Subscription?> GetActiveSubscriptionAsync(int userId)
        {
            var now = DateTime.Now;
            return await _context.Subscriptions
                .Include(s => s.Plan) 
                .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive && s.EndDate > now);
        }

        public async Task<Subscription?> GetByIdAsync(int id)
        {
            return await _context.Subscriptions
         .Include(s => s.Plan)
         .Include(s => s.User)
         .FirstOrDefaultAsync(s => s.Id == id);

        }

        public async Task<List<Subscription>> GetByUserIdAsync(int userId)
        {
            return await _context.Subscriptions
            .Include(s => s.Plan)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.EndDate) 
            .ToListAsync();
        }

        public async Task<List<Subscription>> GetExpiredSubscriptionsAsync()
        {
            return await _context.Subscriptions
                .Where(s => s.IsActive && s.EndDate <= DateTime.Now)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
                return false;

            _context.Subscriptions.Remove(subscription);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}