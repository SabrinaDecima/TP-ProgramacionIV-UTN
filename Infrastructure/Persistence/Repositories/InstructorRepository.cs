using Application.Abstraction;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly GymDbContext _context;

        public InstructorRepository(GymDbContext context)
        {
            _context = context;
        }

        public List<Instructor> GetAll() { 

            return _context.Instructors.ToList();
        }

        public Instructor? GetById(int id) {
            return _context.Instructors.Find(id);
        }

        public Instructor CreateInstructor(Instructor instructor)
        {
            _context.Instructors.Add(instructor);
            _context.SaveChanges();
            return instructor;
        }

        //public bool UpdateInstructor(int id, Instructor instructor)
       // {
       //     _context.Instructors.Update(instructor);
         //   return _context.SaveChanges() > 0;
       // }

        public bool UpdateInstructor(int id, Instructor instructor)
        {
            var existing = _context.Instructors.Find(id);
            if (existing == null) return false;

            // Actualiza solo las propiedades (mejor que reemplazar toda la entidad)
            existing.Nombre = instructor.Nombre;
            existing.Apellido = instructor.Apellido;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteInstructor(int id)
        {
            var entity = _context.Instructors.Find(id);
            if (entity != null)
            {
                _context.Instructors.Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
