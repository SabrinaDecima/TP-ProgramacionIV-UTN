using Application.Abstraction;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GymClassRepository : IGymClassRepository
    {
        private readonly GymDbContext _context;

        public GymClassRepository(GymDbContext context)
        {
            _context = context;
        }
        public List<GymClass> GetAllWithUsers()
        {
            return _context.GymClasses
                .Include(gc => gc.Users)
                .ToList();
        }

        public GymClass? GetByIdWithUsers(int id)
        {
            return _context.GymClasses
                .Include(gc => gc.Users)
                .FirstOrDefault(gc => gc.Id == id);
        }

        public List<GymClass> GetAll()
        {
            return _context.GymClasses.ToList();
        }

        public GymClass? GetById(int id)
        {
            return _context.GymClasses.Find(id);
        }

        public bool AddUserToClass(int classId, int userId)
        {
            var gymClass = _context.GymClasses
                .Include(gc => gc.Users)
                .FirstOrDefault(gc => gc.Id == classId);

            var user = _context.Users.Find(userId);

            if (gymClass == null || user == null)
                return false;

            if (!gymClass.Users.Any(u => u.Id == userId))
                gymClass.Users.Add(user);

            _context.SaveChanges();
            return true;
        }

        public bool RemoveUserFromClass(int classId, int userId)
        {
            var gymClass = _context.GymClasses
                .Include(gc => gc.Users)
                .FirstOrDefault(gc => gc.Id == classId);

            if (gymClass == null)
                return false;

            var user = gymClass.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return false;

            gymClass.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public GymClass CreateGymClass(GymClass gymClass)
        {
            _context.GymClasses.Add(gymClass);
            _context.SaveChanges();
            return gymClass;
        }

        public bool UpdateGymClass(int id, GymClass gymClass)
        {
            var existing = _context.GymClasses.Find(id);
            if (existing == null) return false;

            existing.Nombre = gymClass.Nombre;
            existing.Descripcion = gymClass.Descripcion;
            existing.DuracionMinutos = gymClass.DuracionMinutos;
            existing.ImageUrl = gymClass.ImageUrl;
            existing.Dia = gymClass.Dia;
            existing.Hora = gymClass.Hora;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteGymClass(int id)
        {
            var gymClass = _context.GymClasses
                .Include(gc => gc.Users)
                .FirstOrDefault(g => g.Id == id);

            if (gymClass == null) return false;

            gymClass.Users.Clear(); 
            _context.GymClasses.Remove(gymClass);
            _context.SaveChanges();
            return true;
        }
    }
}
