
using Application.Interfaces;
using Contracts.Instructor.Request;
using Contracts.Instructor.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var instructors = _instructorService.GetAll();
            return Ok(instructors);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var instructor = _instructorService.GetById(id);
            if (instructor == null)
                return NotFound();

            return Ok(instructor);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateInstructorRequest request)
        {
            var created = _instructorService.CreateInstructor(request);
            return Ok(created); 
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateInstructorRequest request)
        {
            var updated = _instructorService.UpdateInstructor(request);
            if (!updated)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _instructorService.DeleteInstructor(id);
            if (!deleted)
                return NotFound();

            return Ok();
        }
    }
}