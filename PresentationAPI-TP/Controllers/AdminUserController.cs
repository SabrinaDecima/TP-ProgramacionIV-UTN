using Application.Interfaces;
using Application.Services.Implementations;
using Contracts.User.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationAPI_TP.Controllers
{
    [Route("api/admin/user")]
    [ApiController]
    [Authorize(Roles = "Administrador,SuperAdministrador")]
    public class AdminUserController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminUserController(IUserService userService)
        {
            _userService = userService;
            
        }


        [Authorize(Roles = "Administrador, SuperAdministrador")]

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();

            return Ok(users);
        }


        [Authorize(Roles = "Administrador, SuperAdministrador")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserByAdminRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = _userService.CreateUserByAdmin(request);

            if (!created)
                return BadRequest(new { message = "No se pudo crear el usuario. Verifica datos y plan/rol válidos." });

            return Ok(new { message = "Usuario creado correctamente." });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateByAdmin(int id, [FromBody] UpdateUserByAdminRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = _userService.UpdateUserByAdmin(id, request);
            if (!updated)
                return BadRequest(new { message = "No se pudo actualizar el usuario. Verifica datos y plan/rol válidos." });

            return Ok(new { message = "Usuario actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _userService.DeleteUser(id);
            if (!deleted)
                return NotFound("Usuario no encontrado.");

            return Ok("Usuario eliminado correctamente.");
        }


    }
}
