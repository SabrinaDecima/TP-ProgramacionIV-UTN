using Application.Abstraction;
using Contracts.GymClass.Request;
using Contracts.Payment.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var payments = _paymentService.GetAllPayments();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var payment = _paymentService.GetPaymentById(id);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create(CreatePaymentRequest request)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type== "UserId");

            if (request.Monto <= 0)
                return BadRequest("El monto debe ser mayor a cero.");
            request.UserId = int.Parse(userId.Value);   

            var result = _paymentService.CreatePayment(request);

            if (result == null)
                return BadRequest("No se pudo crear el pago.");
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdatePaymentRequest request)
        {
            var result = _paymentService.UpdatePayment(id, request);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _paymentService.DeletePayment(id);
            if (!deleted)
                return NotFound();
            return Ok();
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetPaymentsByUser(int userId)
        {
            if (userId <= 0)
                return BadRequest("El ID de usuario no es válido.");

            var payments = _paymentService.GetPaymentsByUserId(userId);
            return Ok(payments);
        }

        [HttpGet("user/{userId}/payments/pending")]
        public IActionResult GetPendingPaymentsByUser(int userId)
        {
            if (userId <= 0)
                return BadRequest("El ID de usuario no es válido.");

            var pendingPayments = _paymentService.GetPendingPaymentsByUserId(userId);
            return Ok(pendingPayments);
        }

        [HttpPost("mercadopago")]
        public async Task<IActionResult> CreateMercadoPagoPayment([FromBody] CreateMercadoPagoRequest request)
        {
            if (request.Monto <= 0)
                return BadRequest("El monto debe ser mayor a cero.");

            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userId != null)
                request.UserId = int.Parse(userId.Value);

            try
            {
                var url = await _paymentService.CreatePaymentPreferenceAsync(request);
                return Ok(new { Url = url });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Configuración incorrecta: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error de Mercado Pago: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno al procesar el pago.");
            }
        }



    }
}