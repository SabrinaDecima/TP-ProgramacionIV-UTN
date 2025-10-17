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
            
            if (!response)
            {
                return BadRequest("No se ha podido realizar la inscripcion a clase.");
            }

            return Ok("Inscripción exitosa");
        }

        [HttpDelete("unenroll")]
        public IActionResult Unenroll([FromBody] EnrollUserRequest request)
        {
            var response = _enrollmentService.UnenrollUser(request);


            if (!response)
            {
                return BadRequest("No se ha podido realizar la desinscripcion a clase.");
            }

            return Ok("Desinscripción exitosa");
        }
    }
}