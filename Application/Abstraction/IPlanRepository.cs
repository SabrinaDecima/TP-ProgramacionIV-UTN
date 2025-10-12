using Domain.Entities;


namespace Application.Abstraction
{
    public interface IPlanRepository
    {
        List<Plan> GetAll();

        Plan? GetPlanById(int? id);


        bool UpdatePlan(int id, Plan plan);


    }
}
