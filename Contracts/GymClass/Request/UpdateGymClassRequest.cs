
using Domain.Entities;

namespace Contracts.GymClass.Request
{
    public class UpdateGymClassRequest 
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        public int DuracionMinutos { get; set; }
        public string ImageUrl { get; set; }
        public DayOfTheWeek Dia { get; set; }

        public string Hora {  get; set; }
    }
}
