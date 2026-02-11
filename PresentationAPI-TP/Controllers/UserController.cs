using Application.Interfaces;
using Contracts.User.Request;
using Contracts.User.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PresentationAPI_TP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, IPlanService planService)
        {
            _userService = userService;

        }

       

        [Authorize(Roles = "Administrador, SuperAdministrador")]

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute]int id)
        {
            
            var user = _userService.GetById(id);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateUserRequest request)
        {
           
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var updated = _userService.UpdateUser(id, request);

            if (!updated)
                return NotFound("Usuario no encontrado o datos inválidos (planId incorrecto).");

            return Ok("Usuario actualizado correctamente.");
        }


        [Authorize]
        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var email = User.Identity?.Name;
            return Ok(new { email, role });
        }


        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var profile = _userService.GetProfile(userId);
            if (profile == null)
                return NotFound();

            return Ok(profile); 
        }

    }
}
