using Application.Interfaces;
using Contracts.User.Request;
using Contracts.User.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
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

        [Authorize(Roles="Administrador")]

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

        [HttpPut("{userId}/plan")]
        public IActionResult ChangePlan([FromRoute] int userId, [FromBody] ChangePlanRequest request)
        {
            if (request?.PlanId <= 0)
                return BadRequest("PlanId inválido.");

            var success = _userService.ChangeUserPlan(userId, request.PlanId);
            if (!success)
                return BadRequest("No se pudo cambiar el plan. Verifica que el PlanId sea válido.");

            return Ok("Plan actualizado correctamente.");
        }

        //[HttpPost]

        //public IActionResult Create([FromBody] CreateUserRequest request)
        //{
        //    var result = _userService.CreateUser(request);

        //    if (!result)
        //   {
        //       return BadRequest("No se pudo crear el usuario. Verifica completar los campos");

        //   }

        //    return Ok("Usuario creado correctamente");
        //}
    }
}
