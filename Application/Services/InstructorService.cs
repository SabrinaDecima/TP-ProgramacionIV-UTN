// Application/Services/InstructorService.cs
using Application.Abstraction;
using Application.Interfaces;
using Contracts.Instructor.Request;
using Contracts.Instructor.Response;
using Domain.Entities;

namespace Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        

        public List<InstructorResponse> GetAll()
        {
            var instructors = _instructorRepository.GetAll();
            return instructors.Select(i => new InstructorResponse
            {
                Id = i.Id,
                Nombre = i.Nombre,
                Apellido = i.Apellido
            }).ToList();
        }

        public InstructorResponse? GetById(int id)
        {
            var instructor = _instructorRepository.GetById(id);
            if (instructor == null) return null;

            return new InstructorResponse
            {
                Id = instructor.Id,
                Nombre = instructor.Nombre,
                Apellido = instructor.Apellido
            };
        }

        public InstructorResponse CreateInstructor(CreateInstructorRequest request)
        {
            var instructor = new Instructor
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido
            };

            var created = _instructorRepository.CreateInstructor(instructor);
            return new InstructorResponse
            {
                Id = created.Id,
                Nombre = created.Nombre,
                Apellido = created.Apellido
            };
        }

        public bool UpdateInstructor(UpdateInstructorRequest request)
        {
            var existing = _instructorRepository.GetById(request.Id);
            if (existing == null) return false;

            existing.Nombre = request.Nombre;
            existing.Apellido = request.Apellido;

            return _instructorRepository.UpdateInstructor(request.Id, existing);
        }

        public bool DeleteInstructor(int id)
        {
            return _instructorRepository.DeleteInstructor(id);
        }
    }
}