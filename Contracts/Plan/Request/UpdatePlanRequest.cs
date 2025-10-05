using Domain.Entities;


namespace Contracts.Plan.Request
{
    public class UpdatePlanRequest
    {
        public int Id { get; set; }
        public TypePlan Tipo { get; set; } 

        public decimal Precio { get; set; }
    }
}
