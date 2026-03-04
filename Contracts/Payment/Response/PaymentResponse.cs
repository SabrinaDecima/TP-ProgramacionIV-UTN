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
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public bool Pagado { get; set; }
        public string? MetodoPago { get; set; }
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }

        // 🔹 NUEVOS CAMPOS PARA MOSTRAR NOMBRE DEL USUARIO
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
    }
}
