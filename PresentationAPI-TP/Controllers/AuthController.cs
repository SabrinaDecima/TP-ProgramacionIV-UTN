using Application.Abstraction.ExternalServices;
using Application.Interfaces;
using Contracts.Login.Request;
using Contracts.User.Request;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public AuthController(IAuthenticationService authenticationService, IUserService userService)
        {
            
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpPost("register")]

        public ActionResult Register([FromBody] CreateUserRequest request)
        {
            var result = _userService.CreateUser(request);

            if (!result)
                return BadRequest(new { message = "No se pudo crear el usuario. Verifique los datos." });

            return Ok(new { message = "Se ha registrado el usuario con exito" });
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] Contracts.Login.Request.LoginRequest request)
        {
            var token = _authenticationService.Login(request);
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Credenciales inválidas");

            var user = await _userService.GetByEmailAsync(request.Email);

            if (user == null || user.Rol == null)
                return Unauthorized("Usuario no encontrado o sin rol asignado");

            return Ok(new
            {
                token,
                email = user.Email,
                role = user.Rol.Nombre,
                userId = user.Id,
                name = $"{user.Nombre} {user.Apellido}".Trim()
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] Contracts.User.Request.ForgotPasswordRequest request)
        {
            var result = await _userService.RequestPasswordResetAsync(request.Email);

            if (!result)
                return BadRequest(new { message = "No se encontró usuario con ese email." });

            return Ok(new { message = "Se ha enviado un enlace de reset a tu email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Contracts.User.Request.ResetPasswordRequest request)
        {
            var result = await _userService.ResetPasswordAsync(request.Token, request.NewPassword);

            if (!result)
                return BadRequest(new { message = "Token inválido o expirado." });

            return Ok(new { message = "Contraseña actualizada correctamente." });
        }



    }
}
