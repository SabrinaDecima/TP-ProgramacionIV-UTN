using Application.Abstraction;
using Contracts.GymClass.Request;
using Contracts.Payment.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        [Authorize(Roles = "Administrador, SuperAdministrador")]
        [HttpGet]
        public IActionResult Get()
        {
            var payments = _paymentService.GetAllPayments();
            return Ok(payments);
        }

        [Authorize(Roles = "Administrador, SuperAdministrador")]
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
            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (request.Monto <= 0)
                return BadRequest("El monto debe ser mayor a cero.");
            request.UserId = int.Parse(userId.Value);

            var result = _paymentService.CreatePayment(request);

            if (result == null)
                return BadRequest("No se pudo crear el pago.");
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdatePaymentRequest request)
        {
            var result = _paymentService.UpdatePayment(id, request);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _paymentService.DeletePayment(id);
            if (!deleted)
                return NotFound();
            return Ok();
        }

        [Authorize(Roles = "Administrador, SuperAdministrador")]
        [HttpGet("user/{userId}")]
        public IActionResult GetPaymentsByUser(int userId)
        {
            if (userId <= 0)
                return BadRequest("El ID de usuario no es válido.");

            var payments = _paymentService.GetPaymentsByUserId(userId);
            return Ok(payments);
        }

        [Authorize(Roles = "Administrador, SuperAdministrador")]
        [HttpGet("user/{userId}/payments/pending")]
        public IActionResult GetPendingPaymentsByUser(int userId)
        {
            if (userId <= 0)
                return BadRequest("El ID de usuario no es válido.");

            var pendingPayments = _paymentService.GetPendingPaymentsByUserId(userId);
            return Ok(pendingPayments);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMyPayments()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);
            var payments = _paymentService.GetPaymentsByUserId(userId);

            return Ok(payments);
        }

        [Authorize]
        [HttpPost("mercadopago/verify")]
        public async Task<IActionResult> VerifyPayment()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);
            Console.WriteLine($"[Verify] Iniciando verificación para usuario {userId}");

            try
            {
                await _paymentService.VerifyUserPaymentAsync(userId);
                return Ok(new { message = "Verificación completada" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Verify] Error: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("me/payments/pending")]
        public IActionResult GetMyPendingPayment()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var payment = _paymentService
                .GetPendingPaymentsByUserId(userId)
                .OrderBy(p => p.Fecha)
                .FirstOrDefault();

            return Ok(payment);
        }



        [Authorize]
        [HttpPost("mercadopago")]
        public async Task<IActionResult> CreateMercadoPagoPayment([FromBody] CreateMercadoPagoRequest request)
        {
            if (request.Monto <= 0)
                return BadRequest("El monto debe ser mayor a cero.");

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim == null)
                return Unauthorized();

            request.UserId = int.Parse(userIdClaim.Value);

            try
            {
                var result = await _paymentService.CreatePaymentPreferenceAsync(request);
                return Ok(new { initPoint = result.InitPoint, preferenceId = result.PreferenceId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando preferencia: {ex.Message}");
                return StatusCode(500, "Error al procesar el pago.");
            }
        }

        [AllowAnonymous]
        [HttpPost("mercadopago/webhook")]
        public async Task<IActionResult> MercadoPagoWebhook([FromBody] JsonElement payload)
        {
            string paymentId = null;
            try
            {
                // Mercado Pago envía: { "type": "payment", "data": { "id": "123" } }
                // O bien por query string: ?topic=payment&id=123
                if (payload.ValueKind == JsonValueKind.Object)
                {
                    if (payload.TryGetProperty("type", out var type) && type.GetString() == "payment"
                        && payload.TryGetProperty("data", out var data)
                        && data.TryGetProperty("id", out var idProp))
                    {
                        paymentId = idProp.GetString();
                    }
                    else if (payload.TryGetProperty("id", out var id))
                    {
                        paymentId = id.GetString();
                    }
                }

                if (string.IsNullOrEmpty(paymentId) && Request.Query.ContainsKey("id"))
                {
                    paymentId = Request.Query["id"];
                }

                if (string.IsNullOrEmpty(paymentId))
                    return BadRequest("No se encontró id de pago en la notificación.");

                await _paymentService.HandlePaymentNotificationAsync(paymentId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando webhook: {ex.Message}");
                return StatusCode(500, "Error procesando notificación.");
            }
        }




    }
}