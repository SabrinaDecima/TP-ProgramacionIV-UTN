
namespace Domain.Entities
{
    public class Payment
    {
        protected Payment() { }
        public Payment (int id, int userId, decimal amount, DateTime date)
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
        public DateTime Fecha { get; set; } //ver si es data time
        public bool Pagado { get; set; }
    }
}

