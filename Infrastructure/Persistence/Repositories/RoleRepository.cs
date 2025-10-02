using Application.Abstraction;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly GymDbContext _context;

        public RoleRepository(GymDbContext context)
        {
            _context = context;
        }

        public Role? GetById(int id)
        {
            return _context.Roles.Find(id);
        }

        public List<Role> GetAll()
        {
            return _context.Roles.ToList();
        }
    }
}