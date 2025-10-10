
namespace Contracts.GymClass.Request
{
    public class UpdateGymClassRequest 
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        public int DuracionMinutos { get; set; }
        public string ImageUrl { get; set; }
        public string Fecha { get; set; }

        public string Hora {  get; set; }
    }
}
