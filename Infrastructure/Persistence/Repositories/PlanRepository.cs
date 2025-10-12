using Application.Abstraction;

using Domain.Entities;
using Infrastructure.Persistence;


namespace Infrastructure.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _context;

        public PlanRepository(GymDbContext context)
        {
            _context = context;
        }

        public List<Plan> GetAll()
        {
            return _context.Plans.ToList();
        }

        public Plan? GetPlanById(int? id)
        {
            if (id == null) return null;
            return _context.Plans.Find(id.Value);
        }


        public bool UpdatePlan(int planId, Plan plan)
        {
            _context.Plans.Update(plan);
            return _context.SaveChanges() > 0;
        }

    }
}
