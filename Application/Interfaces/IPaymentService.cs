using Contracts.GymClass.Request;
using Contracts.Payment.Request;
using Contracts.Payment.Response;

namespace Application.Abstraction
{
    public interface IPaymentService
    {
        Task<(string InitPoint, string PreferenceId)> CreatePaymentPreferenceAsync(CreateMercadoPagoRequest request);
        Task HandlePaymentNotificationAsync(string mercadoPagoPaymentId);
        Task<List<PaymentResponse>> GetAllPaymentsAsync();
        Task<PaymentResponse?> GetPaymentByIdAsync(int id);
        Task<List<PaymentResponse>> GetPaymentsByUserIdAsync(int userId);
        Task<bool> ProcessManualPaymentAsync(int userId, int planId, decimal amount);
       
    }
}