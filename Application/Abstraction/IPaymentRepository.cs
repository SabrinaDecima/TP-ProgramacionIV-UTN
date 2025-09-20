using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IPaymentRepository
    {
        List<Payment> GetAll();
        Payment? GetById(int id);

        Payment CreatePayment(Payment payment);

        List<Payment> GetPaymentsByUserId(int userId);

        List<Payment> GetPendingsPaymentsByUserId(int userId);
    }
}
