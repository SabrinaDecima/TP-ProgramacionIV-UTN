using Domain.Entities;


namespace Application.Abstraction
{
    public interface IInstructorRepository
    {
        List<Instructor> GetAll();
        Instructor? GetById(int id);

        Instructor CreateInstructor (Instructor instructor);

        bool UpdateInstructor (int id, Instructor instructor);

        bool DeleteInstructor (int id);
    }
}

