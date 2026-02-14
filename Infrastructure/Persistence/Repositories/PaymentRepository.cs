using Application.Abstraction;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly GymDbContext _context;

        public PaymentRepository(GymDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .Include(p => p.Subscription)
                .Include(p => p.User)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
            ;
        }
        
        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
               .Include(p => p.Subscription)
               .Include(p => p.User)
               .FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<bool> CreateAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            
            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<List<Payment>> GetByUserIdAsync(int userId)
        {
            // Como el pago apunta a la suscripción y la suscripción al usuario
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
        
        public async Task<List<Payment>> GetBySubscriptionIdAsync(int subscriptionId)
        {
            return await _context.Payments
                .Where(p => p.SubscriptionId == subscriptionId)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }
        
        public async Task<bool> UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            
            return await _context.SaveChangesAsync() > 0;
        }
       
        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return false;

            _context.Payments.Remove(payment);
            
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Payment?> GetByPreferenceIdAsync(string preferenceId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PreferenceId == preferenceId);
        }


    }
}
