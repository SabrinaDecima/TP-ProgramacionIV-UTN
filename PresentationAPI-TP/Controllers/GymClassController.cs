using Application.Interfaces;
using Contracts.GymClass.Request;
using Contracts.GymClass.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        private int GetUserId()
        {
            var claim =
                User.FindFirst("userId") ??
                User.FindFirst("id") ??
                User.FindFirst("sub");

            if (claim == null)
                throw new UnauthorizedAccessException("No se pudo obtener el userId del token");

            return int.Parse(claim.Value);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = GetUserId();
            var classes = _gymClassService.GetAll(userId);
            return Ok(classes);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var userId = GetUserId();
            var gymClass = _gymClassService.GetById(id, userId);

            if (gymClass == null)
                return NotFound();

            return Ok(gymClass);
        }

        [Authorize(Roles = "Administrador, SuperAdministrador")]
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

        [Authorize(Roles = "Administrador, SuperAdministrador")]
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id , [FromBody] UpdateGymClassRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre de la clase es obligatorio.");


            var updated = _gymClassService.UpdateGymClass(id,request);
            if (!updated)
                return NotFound("Clase de gimnasio no encontrada.");

            return Ok(); 
        }

        [Authorize(Roles = "Administrador, SuperAdministrador")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _gymClassService.DeleteGymClass(id);
            if (!deleted)
                return NotFound("Clase de gimnasio no encontrada.");

            return Ok(); 
        }


        [Authorize]
        [HttpPost("{id}/reserve")]
        public IActionResult Reserve(int id)
        {
            var userId = GetUserId();

            var reserved = _gymClassService.ReserveClass(id, userId);
            if (!reserved)
                return BadRequest("No se pudo reservar la clase.");

            return Ok();
        }


        [Authorize]
        [HttpDelete("{id}/reserve")]
        public IActionResult CancelReservation(int id)
        {
            var userId = GetUserId();

            var canceled = _gymClassService.CancelReservation(id, userId);
            if (!canceled)
                return BadRequest("No se pudo cancelar la reserva.");

            return Ok();
        }
    }
}