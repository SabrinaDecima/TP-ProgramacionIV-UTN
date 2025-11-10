using Contracts.Payment.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.ExternalService
{
    public interface IPaymentGateway
    {
        Task<(string InitPoint, string PreferenceId)> CreatePreferenceAsync(CreateMercadoPagoRequest request);
    }
}
