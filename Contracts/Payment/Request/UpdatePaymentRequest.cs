
namespace Contracts.GymClass.Request
{
    public class UpdatePaymentRequest
    {
        public int UserId { get; set; }

        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public bool Pagado { get; set; }
    }
}
