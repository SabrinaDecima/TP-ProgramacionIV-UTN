using Application.Abstraction;
using Contracts.Login.Request;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        private readonly GymDbContext _context;

        public UserRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }
        
        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() > 0;

        }

        public bool UpdateUser(int id, User user)
        {
            _context.Users.Update(user);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteUser (int id)
        {
            var user = _context.Users.FirstOrDefault(u =>u.Id == id);
            if (user == null) 
                return false;
            _context.Users.Remove(user);
            return _context.SaveChanges() > 0;

        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User? GetByEmailAndPassword (LoginRequest request)
        {
            return _context.Users
                .Include( x => x.Rol)
                .FirstOrDefault( x => x.Email == request.Email && x.Contraseña == request.Password);
        }

 
        public User? GetUserWithPlan(int id)
        {
            return _context.Users
                .Where(u => u.Id == id)
                .Select(u => new User
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    Telefono = u.Telefono,
                    Contraseña = u.Contraseña,
                    RoleId = u.RoleId,
                    Rol = u.Rol,
                    PlanId = u.PlanId,
                    Plan = u.Plan,
                    UserClasses = u.UserClasses,
                    Pagos = u.Pagos
                })
                .FirstOrDefault();
        }

        public User? GetUserWithClasses(int id)
        {
            return _context.Users
                .Where(u => u.Id == id)
                .Select(u => new User
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    Telefono = u.Telefono,
                    Contraseña = u.Contraseña,
                    RoleId = u.RoleId,
                    Rol = u.Rol,
                    PlanId = u.PlanId,
                    Plan = u.Plan,
                    UserClasses = u.UserClasses,
                    Pagos = u.Pagos
                })
                .FirstOrDefault();
        }

        public User? GetUserWithPayment(int id)
        {
            return _context.Users
                .Where(u => u.Id == id)
                .Select(u => new User
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    Telefono = u.Telefono,
                    Contraseña = u.Contraseña,
                    RoleId = u.RoleId,
                    Rol = u.Rol,
                    PlanId = u.PlanId,
                    Plan = u.Plan,
                    UserClasses = u.UserClasses,
                    Pagos = u.Pagos
                })
                .FirstOrDefault();
        }

        public bool ChangeUserRole(int id, string newRole)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return false; // Usuario no encontrado
            }
            var role = _context.Roles.FirstOrDefault(r => r.Nombre == newRole);
            if (role == null)
            {
                return false; // Rol no encontrado
            }
            user.RoleId = role.Id;
            user.Rol = role;
            _context.Users.Update(user);
            return _context.SaveChanges() > 0;
        }

    }
}