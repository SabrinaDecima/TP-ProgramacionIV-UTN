using Contracts.GymClass.Request;

using Contracts.Payment.Request;
using Contracts.Payment.Response;

namespace Application.Abstraction
{
    public interface IPaymentService
    {
        List<PaymentResponse> GetAllPayments();
        PaymentResponse? GetPaymentById(int id);
        PaymentResponse? CreatePayment(CreatePaymentRequest request);
        PaymentResponse? UpdatePayment(int id, UpdatePaymentRequest request);
        bool DeletePayment(int id);
        List<PaymentResponse> GetPaymentsByUserId(int userId);
        List<PaymentResponse> GetPendingPaymentsByUserId(int userId);
        Task<string> CreatePaymentPreferenceAsync(CreateMercadoPagoRequest request);
        Task HandlePaymentNotificationAsync(string mercadoPagoPaymentId);
    }
}