using Application.Interfaces;
using Contracts.Enrollment.Request;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost("enroll")]
        public IActionResult Enroll([FromBody] EnrollUserRequest request)
        {
            var response = _enrollmentService.EnrollUser(request);
            if (response.Message.Contains("exitosa") || response.Message.Contains("cancelada"))
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("unenroll")]
        public IActionResult Unenroll([FromBody] EnrollUserRequest request)
        {
            var response = _enrollmentService.UnenrollUser(request);
            if (response.Message.Contains("cancelada") || response.Message.Contains("exitosa"))
                return Ok(response);
            return BadRequest(response);
        }
    }
}