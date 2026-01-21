using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.GymClass.Request
{
    public class CreateGymClassRequest
    {
        public string Nombre {  get; set; }

        public string Descripcion { get; set; }
        public int DuracionMinutos { get; set; }
        public string ImageUrl { get; set; }
        public DayOfTheWeek Dia { get; set; }

        public string Hora {  get; set; }
        public int MaxCapacity { get; set; } = 10;
    }
}
