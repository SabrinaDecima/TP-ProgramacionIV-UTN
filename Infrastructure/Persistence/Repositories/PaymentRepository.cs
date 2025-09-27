using Application.Abstraction;

using Domain.Entities;
using Infrastructure.Persistence;


namespace Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly GymDbContext _context;

        public PaymentRepository(GymDbContext context)
        {
            _context = context;
        }

        public List<Payment> GetAll() {
            return _context.Payments.ToList();
        }

        public Payment? GetById(int id)
        {
            return _context.Payments.Find(id);
        }

        public Payment CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
            return payment;
        }

        public bool UpdatePayment(int id, Payment payment)
        {
            _context.Payments.Update(payment);
            _context.SaveChanges();
            return true;
        }

        public bool DeletePayment(int id)
        {
            var entity = _context.Payments.Find(id);
            if (entity != null)
            {
                _context.Payments.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Payment> GetPaymentsByUserId(int userId) {
            return _context.Payments.Where(p => p.UserId == userId).ToList();
        }

        public List<Payment> GetPendingsPaymentsByUserId(int userId)
        {
            return _context.Payments
                .Where(p => p.UserId == userId && !p.Pagado)
                .ToList();
        }
            
    }
}

