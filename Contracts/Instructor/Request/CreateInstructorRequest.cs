using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Instructor.Request
{
    public class CreateInstructorRequest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
