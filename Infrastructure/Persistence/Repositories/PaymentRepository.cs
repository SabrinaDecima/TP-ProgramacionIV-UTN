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

        public List<Payment> GetAll()
        {
            return _context.Payments.Include(p => p.User).ToList();
        }

        public Payment? GetById(int id)
        {
            return _context.Payments.Include(p => p.User).FirstOrDefault(p => p.Id == id);
        }

        public Payment CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
            return payment;
        }

        public Payment UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            _context.SaveChanges();
            return payment;
        }

        public bool DeletePayment(int id)
        {
            var payment = _context.Payments.Find(id);
            if (payment == null) return false;

            _context.Payments.Remove(payment);
            _context.SaveChanges();
            return true;
        }

        public List<Payment> GetPaymentsByUserId(int userId)
        {
            return _context.Payments
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .ToList();
        }

        public List<Payment> GetPendingPaymentsByUserId(int userId)
        {
            return _context.Payments
                .Include(p => p.User)
                .Where(p => p.UserId == userId && !p.Pagado)
                .ToList();
        }

        public Payment? GetByPreferenceId(string preferenceId)
        {
            return _context.Payments.Include(p => p.User).FirstOrDefault(p => p.PreferenceId == preferenceId);
        }
    }
}
