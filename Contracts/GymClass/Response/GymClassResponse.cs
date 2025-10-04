using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.GymClass.Response
{
    public class GymClassResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DuracionMinutos { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Fecha { get; set; }

        public TimeSpan Hora {  get; set; }
    }
}
    