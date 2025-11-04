
namespace Domain.Entities
{
    public class Payment
    {
        public Payment() { }
        public Payment (int id, int userId, decimal amount, string date)
        {
            Id = id;
            UserId = userId;
            Monto = amount;
            Fecha = date;
            Pagado = false;
        }

        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; } //relacion con usuario (1 a 1).

        public decimal Monto { get; set; }
        public string Fecha { get; set; } 
        public bool Pagado { get; set; }
        public string? PreferenceId { get; set; }  // ID de Mercado Pago
        public string? InitPoint { get; set; }     // Link de pago
    }
}

