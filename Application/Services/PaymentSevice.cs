using Application.Abstraction;
using Domain.Entities;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public List<Payment> GetAllPayments() => _paymentRepository.GetAll();

        public Payment? GetPaymentById(int id) => _paymentRepository.GetById(id);

        public Payment CreatePayment(Payment payment) => _paymentRepository.CreatePayment(payment);

        public Payment UpdatePayment(Payment payment) => _paymentRepository.UpdatePayment(payment);

        public bool DeletePayment(int id) => _paymentRepository.DeletePayment(id);

        public List<Payment> GetPaymentsByUserId(int userId) => _paymentRepository.GetPaymentsByUserId(userId);

        public List<Payment> GetPendingPaymentsByUserId(int userId) => _paymentRepository.GetPendingsPaymentsByUserId(userId);
    }
}
