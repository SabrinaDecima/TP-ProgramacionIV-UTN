using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.GymClass.Request
{
    public class UpdateGymClassRequest
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int InstructorId { get; set; }

        public int DuracionMinutos { get; set; }
        public string ImageUrl { get; set; }
    }
}
