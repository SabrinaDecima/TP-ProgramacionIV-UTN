using Application.Interfaces;
using Contracts.User.Request;
using Contracts.User.Response;
using Microsoft.AspNetCore.Mvc;

namespace PresentationAPI_TP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPlanService _planService;

        public UserController(IUserService userService, IPlanService planService)
        {
            _userService = userService;
            _planService = planService;
        }

        [HttpGet] 
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            
            return Ok(users);
        }

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

        [HttpPost]

        public IActionResult Create([FromBody] CreateUserRequest request)
        {
            var result = _userService.CreateUser(request);

            if (!result)
            {
                return BadRequest("No se pudo crear el usuario. Verifica completar los campos");

            }

            return Ok("Usuario creado correctamente");
        }

        [HttpPut("{id}")]

        public IActionResult Update([FromRoute]int id, [FromBody] UpdateUserRequest request)
        {
            var plan = _planService.GetById(request.PlanId);

            if(plan == null)
            {
                return BadRequest("El planId proporcionado no es valido");
            }

            var user = _userService.GetById(id);


            if(user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            var updatedUser = _userService.UpdateUser(id, request);

            if (!updatedUser)
            {
                return NotFound("No se pudo actualizar el usuario");
            }

            return Ok("Usuario actualizado");
        }
    }
}
