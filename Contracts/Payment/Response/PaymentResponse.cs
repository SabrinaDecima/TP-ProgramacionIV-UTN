using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Payment.Response
{
    public class PaymentResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }
      
        public decimal Monto { get; set; }
        public string Fecha { get; set; }
        public bool Pagado { get; set; }
    }
}
