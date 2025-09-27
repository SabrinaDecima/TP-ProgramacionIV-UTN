using Contracts.Instructor.Request;
using Contracts.Instructor.Response;



namespace Application.Interfaces
{
    public interface IInstructorService
    {
        List<InstructorResponse> GetAll();

        InstructorResponse? GetById(int id);

        bool CreateInstructor(CreateInstructorRequest request);

        bool UpdateInstructor(UpdateInstructorRequest request);

        bool DeleteInstructor(int id);
    }
}

