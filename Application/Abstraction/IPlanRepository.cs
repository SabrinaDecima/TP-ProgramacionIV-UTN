using Domain.Entities;


namespace Application.Abstraction
{
    public interface IPlanRepository
    {
        List<Plan> GetAllPlans();

        Plan? GetPlanById(int id);

        Plan CreatePlan(Plan plan);

        bool UpdatePlan(int id, Plan plan);

        bool DeletePlan(int id);
    }
}
