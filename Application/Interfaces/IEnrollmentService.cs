using Contracts.Enrollment.Request;
using Contracts.Enrollment.Response;

namespace Application.Interfaces
{
    public interface IEnrollmentService
    {
        EnrollmentResponse EnrollUser(EnrollUserRequest request);
        EnrollmentResponse UnenrollUser(EnrollUserRequest request);
    }
}