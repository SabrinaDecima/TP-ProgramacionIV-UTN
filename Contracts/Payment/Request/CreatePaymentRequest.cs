using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Payment.Request
{
    public class CreatePaymentRequest
    {
        public int UserId { get; set; }

        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public bool Pagado { get; set; }
    }
}
