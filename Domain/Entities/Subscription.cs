using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        // Permite acceder a los pagos realizados para esta suscripción
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        // Propiedad calculada 
        public bool IsExpired => DateTime.UtcNow > EndDate;

    }
}
