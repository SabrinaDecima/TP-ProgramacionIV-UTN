using Contracts.Plan.Request;
using Contracts.Plan.Response;
using Domain.Entities;


namespace Application.Interfaces
{
    public interface IPlanService
    {
        List<PlanResponse> GetAll();

        PlanResponse? GetById(int id);

        bool CreatePlan(CreatePlanRequest request);

        bool UpdatePlan(UpdatePlanRequest request);

        bool DeletePlan(int id);
    }
}

