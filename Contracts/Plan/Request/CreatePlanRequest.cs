
using Domain.Entities;

namespace Contracts.Plan.Request
{
    public class CreatePlanRequest
    {
        public TypePlan Tipo { get; set; }

        public decimal Precio { get; set; }
    }
}
