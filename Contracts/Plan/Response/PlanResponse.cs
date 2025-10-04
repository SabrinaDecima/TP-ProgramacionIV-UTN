
using Domain.Entities;

namespace Contracts.Plan.Response
{
    public class PlanResponse
    {
        public int Id { get; set; }
        public TypePlan Tipo { get; set; } 

        public decimal Precio { get; set; }
    }
}
