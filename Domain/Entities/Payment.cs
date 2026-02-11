
namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Payment() { }
        public Payment (int id, int userId, decimal amount, DateTime date)
        {
            UserId = userId;
            Monto = amount;
            Fecha = date;
            Pagado = false;
        }

        public int UserId { get; set; }
        public User? User { get; set; } 

        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; } 
        public bool Pagado { get; set; }

        public string? MetodoPago { get; set; }
        public string? PreferenceId { get; set; }  // ID de Mercado Pago
        public string? InitPoint { get; set; }     // Link de pago

        public int? SubscriptionId { get; set; }

        public Subscription? Subscription { get; set; }
    }
}

