using Application.Abstraction.ExternalServices;
using Contracts.Login.Request;
using Contracts.User.Request;
using Infrastructure.Persistence;
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

        public AuthController(IAuthenticationService authenticationService)
        {
            
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]

        public ActionResult Register([FromBody] CreateUserRequest request)
        {
            var result = _authenticationService.Register(request);

            if (result == "Usuario ya existe" || result == "Plan no encontrado" ||
                result == "Rol no encontrado" || result == "Error al crear el usuario")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginRequest request)
        {

            var token = _authenticationService.Login(request);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Credenciales inválidas");
            }

            return Ok(token);
        }
    }
}
