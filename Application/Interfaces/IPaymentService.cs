using Contracts.Payment.Request;
using Contracts.Payment.Response;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        List<PaymentResponse> GetAll();

        PaymentResponse? GetById(int id);

        bool CreatePayment(CreatePaymentRequest request);


    }
}



