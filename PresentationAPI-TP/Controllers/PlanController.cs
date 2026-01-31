using Application.Interfaces;
using Contracts.Plan.Request;
using Contracts.Plan.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationAPI_TP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _planService;
        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var plans = _planService.GetAll();
            return Ok(plans);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var plan = _planService.GetById(id);
            if (plan == null)
                return NotFound();
            return Ok(plan);
        }

        [Authorize(Roles = "Administrador, SuperAdministrador")]

        [HttpPut("{id}")]
        public IActionResult UpdatePlan([FromRoute] int id, [FromBody] UpdatePlanRequest request)
        {
            if (request == null)
                return BadRequest(new UpdatePlanResponse { Success = false, Message = "Los datos del plan son requeridos." });

            if (!Enum.IsDefined(typeof(TypePlan), request.Tipo))
                return BadRequest(new UpdatePlanResponse { Success = false, Message = "El tipo de plan no es válido." });

            if (request.Precio < 0)
                return BadRequest(new UpdatePlanResponse { Success = false, Message = "El precio no puede ser negativo." });

            request.Id = id;

            var updated = _planService.UpdatePlan(request);

            if (!updated)
                return BadRequest(new UpdatePlanResponse { Success = false, Message = "No se pudo actualizar el plan. Puede que no exista o que ya exista otro plan con ese tipo." });

            return Ok(new UpdatePlanResponse { Success = true, Message = "Plan actualizado correctamente." });
        }


    }
}
