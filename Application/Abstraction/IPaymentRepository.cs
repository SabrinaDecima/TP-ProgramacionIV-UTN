using Domain.Entities;


namespace Application.Abstraction
{
    public interface IPaymentRepository
    {
        List<Payment> GetAll();
        Payment? GetById(int id);

        Payment CreatePayment(Payment payment);
        Payment UpdatePayment(Payment payment);
        bool DeletePayment(int id);

        List<Payment> GetPaymentsByUserId(int userId);

        List<Payment> GetPendingPaymentsByUserId(int userId);
        Payment? GetByPreferenceId(string preferenceId);
    }
}
