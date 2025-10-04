using Domain.Entities;

namespace Application.Abstraction
{
    public interface IPaymentService
    {
        List<Payment> GetAllPayments();
        Payment? GetPaymentById(int id);
        Payment CreatePayment(Payment payment);
        Payment UpdatePayment(Payment payment);
        bool DeletePayment(int id);
        List<Payment> GetPaymentsByUserId(int userId);
        List<Payment> GetPendingPaymentsByUserId(int userId);
    }
}
