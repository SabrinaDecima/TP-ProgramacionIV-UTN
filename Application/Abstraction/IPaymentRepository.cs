using Domain.Entities;


namespace Application.Abstraction
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Payment payment); 
        Task<bool> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(int id);

        Task<Payment?> GetByPreferenceIdAsync(string preferenceId);
        Task<List<Payment>> GetByUserIdAsync(int userId);
        Task<List<Payment>> GetBySubscriptionIdAsync(int subscriptionId);
    }
}
