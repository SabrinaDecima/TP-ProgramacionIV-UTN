using Application.Interfaces;
using Contracts.GymClass.Request;
using Contracts.GymClass.Response;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymClassController : ControllerBase
    {
        private readonly IGymClassService _gymClassService;

        public GymClassController(IGymClassService gymClassService)
        {
            _gymClassService = gymClassService;
        }

    
        [HttpGet]
        public IActionResult GetAll()
        {
            var classes = _gymClassService.GetAll();
            return Ok(classes); 
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var gymClass = _gymClassService.GetById(id);
            if (gymClass == null)
                return NotFound(); 

            return Ok(gymClass); 
        }

 
        [HttpPost]
        public IActionResult Create([FromBody] CreateGymClassRequest request)
        {

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre es obligatorio.");


            var created = _gymClassService.CreateGymClass(request);
            if (!created)
                return BadRequest("No se pudo crear la clase de gimnasio.");

            return Ok(); 
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateGymClassRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre de la clase es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Instructor))
                return BadRequest("El nombre del instructor es obligatorio.");

            var updated = _gymClassService.UpdateGymClass(request);
            if (!updated)
                return NotFound("Clase de gimnasio no encontrada.");

            return Ok(); 
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _gymClassService.DeleteGymClass(id);
            if (!deleted)
                return NotFound("Clase de gimnasio no encontrada.");

            return Ok(); 
        }
    }
}