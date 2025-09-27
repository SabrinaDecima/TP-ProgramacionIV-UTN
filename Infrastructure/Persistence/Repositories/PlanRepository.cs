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

        public List<Plan> GetAllPlans()
        {
            return _context.Plans.ToList();
        }

        public Plan? GetPlanById(int id)
        {
            return _context.Plans.Find(id);
        }

        public Plan CreatePlan(Plan plan)
        {
            _context.Plans.Add(plan);
            _context.SaveChanges();
            return plan;
        }

        public bool UpdatePlan(int plan, Plan plan
        {
            _context.Plans.Update(plan);
            return _context.SaveChanges() > 0;
        }

        public bool DeletePlan(int id)
        {
            var entity = _context.Plans.Find(id);
            if (entity != null)
            {
                _context.Plans.Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
