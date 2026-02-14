using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Payment.Request
{
    public class CreateMercadoPagoRequest
    {
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public decimal Monto { get; set; }
        public string Title { get; set; } = "Suscripción Gimnasio";

        public string? ExternalReference { get; set; }
    }
}