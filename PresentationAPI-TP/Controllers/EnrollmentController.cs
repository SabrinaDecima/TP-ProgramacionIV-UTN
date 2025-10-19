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
            var result = _enrollmentService.EnrollUser(request);

            if(result == null)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
            
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpDelete("unenroll")]
        public IActionResult Unenroll([FromBody] EnrollUserRequest request)
        {
            var result = _enrollmentService.UnenrollUser(request);


            if (result == null)
            {
                return StatusCode(500, "Error interno del servidor.");
            }



            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}