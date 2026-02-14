using Contracts.Enrollment.Request;
using Contracts.Enrollment.Response;

namespace Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponse> EnrollUserAsync(EnrollUserRequest request);
        Task<EnrollmentResponse> UnenrollUserAsync(EnrollUserRequest request);
    }
}