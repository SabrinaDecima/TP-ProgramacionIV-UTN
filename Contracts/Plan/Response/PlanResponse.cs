using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Plan.Response
{
    public class PlanResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } 

        public decimal Price { get; set; }
    }
}
