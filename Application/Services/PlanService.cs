using Application.Abstraction;
using Application.Interfaces;
using Contracts.Plan.Request;
using Contracts.Plan.Response;


namespace Application.Services.Implementations
{
    public class PlanService : IPlanService
    {
        public bool CreatePlan(CreatePlanRequest request)
        {
            throw new NotImplementedException();
        }

        public bool DeletePlan(int id)
        {
            throw new NotImplementedException();
        }

        public List<PlanResponse> GetAll()
        {
            throw new NotImplementedException();
        }

        public PlanResponse? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePlan(UpdatePlanRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
