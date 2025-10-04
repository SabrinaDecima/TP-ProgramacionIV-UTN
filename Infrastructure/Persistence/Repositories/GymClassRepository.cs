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

        public List<GymClass> GetAll()
        {
            return _context.GymClasses
                .Include(static gc => gc.UserClasses)     // ← Opcional: si necesitas socios
                .ThenInclude(uc => uc.User)        // ← Si necesitas datos del User
                .ToList();
        }

        public GymClass? GetById(int id)
        {
            return _context.GymClasses
                .Include(gc => gc.UserClasses)
                .ThenInclude(uc => uc.User)
                .FirstOrDefault(gc => gc.Id == id);
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
            existing.Fecha = gymClass.Fecha;
            existing.Hora = gymClass.Hora;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteGymClass(int id)
        {
            var gymClass = _context.GymClasses.Find(id);
            if (gymClass == null) return false;

            _context.GymClasses.Remove(gymClass);
            _context.SaveChanges();
            return true;
        }
    }
}