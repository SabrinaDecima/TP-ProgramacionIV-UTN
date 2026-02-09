using Application.Interfaces;
using Application.Services;
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
        private readonly IGymClassService _gymClassService;

        public AdminUserController(IUserService userService, IGymClassService gymClassService)
        {
            _userService = userService;
            _gymClassService = gymClassService;
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

            return Ok(new { message = "Usuario eliminado correctamente." });
        }

        [Authorize(Roles = "SuperAdministrador")]
        [HttpGet("activity-summary")]
        public IActionResult GetActivitySummary()
        {
            var allUsers = _userService.GetAll();

            var stats = new
            {
                TotalUsers = allUsers.Count,
                TotalAdmins = allUsers.Count(u => u.RoleId == 2 || u.RoleId == 3),
                TotalSocios = allUsers.Count(u => u.RoleId == 1),
                TotalClasses = _gymClassService.GetAll(0).Count 
            };

            var recentUsers = allUsers
                .OrderByDescending(u => u.Id)
                .Take(10)
                .Select(u => new
                {
                    u.Email,
                    Role = u.RoleId switch
                    {
                        1 => "Socio",
                        2 => "Administrador",
                        3 => "SuperAdministrador",
                        _ => "Desconocido"
                    },
                    Plan = u.RoleId == 1
                        ? (u.PlanId == 1 ? "Basic" :
                           u.PlanId == 2 ? "Premium" :
                           u.PlanId == 3 ? "Elite" : "Sin plan")
                        : "N/A"
                })
                .ToList();

            return Ok(new { stats, recentUsers });
        }


    }
}
