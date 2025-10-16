using Contracts.Enrollment.Request;
using Contracts.Enrollment.Response;

namespace Application.Interfaces
{
    public interface IEnrollmentService
    {
        bool EnrollUser(EnrollUserRequest request);
        bool UnenrollUser(EnrollUserRequest request);
    }
}