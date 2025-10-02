using Application.Abstraction;
using Application.Interfaces;
using Contracts.Payment.Request;
using Contracts.Payment.Response;


namespace Application.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        public bool CreatePayment(CreatePaymentRequest request)
        {
            throw new NotImplementedException();
        }

        public List<PaymentResponse> GetAll()
        {
            throw new NotImplementedException();
        }

        public PaymentResponse? GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}

