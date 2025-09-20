using Domain.Entities;


namespace Application.Abstraction
{
    public interface IPlanRepository
    {
        List<Plan> GetAllPlans();

        Plan? GetPlanById(int id);

        bool CreatePlan(Plan plan);

        bool UpdatePlan(int id, Plan plan);

        bool DeletePlan(int id);
    }
}
