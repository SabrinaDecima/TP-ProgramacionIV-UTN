using Application.Abstraction;
using Application.Interfaces;
using Application.Services;
using Contracts.Payment.Request;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        private readonly ISubscriptionService _subscriptionService;

        public PaymentController(IPaymentService paymentService, ISubscriptionService subscriptionService)
        {
            _paymentService = paymentService;
            _subscriptionService = subscriptionService;
        }

        //  OBTENER TODOS LOS PAGOS (Solo Admins)
        [Authorize(Roles = "Administrador, SuperAdministrador")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        //  OBTENER PAGO POR ID
        [Authorize(Roles = "Administrador, SuperAdministrador")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        // MIS PAGOS (Usuario logueado)
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyPayments()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var payments = await _paymentService.GetPaymentsByUserIdAsync(userId.Value);
            return Ok(payments);
        }

        //  CREAR PREFERENCIA MERCADO PAGO
        [Authorize]
        [HttpPost("mercadopago")]
        public async Task<IActionResult> CreateMercadoPagoPayment([FromBody] CreateMercadoPagoRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            request.UserId = userId.Value;

            try
            {
                var result = await _paymentService.CreatePaymentPreferenceAsync(request);
                return Ok(new { initPoint = result.InitPoint, preferenceId = result.PreferenceId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el pago: {ex.Message}");
            }
        }

        //  WEBHOOK DE MERCADO PAGO (Punto de entrada para notificaciones)
        [AllowAnonymous]
        [HttpPost("mercadopago/webhook")]
        public async Task<IActionResult> MercadoPagoWebhook([FromBody] JsonElement payload)
        {
            string? paymentId = null;
            try
            {
                // Extraer ID del JSON que envía MP
                if (payload.ValueKind == JsonValueKind.Object)
                {
                    if (payload.TryGetProperty("data", out var data) && data.TryGetProperty("id", out var idProp))
                    {
                        paymentId = idProp.GetString();
                    }
                }

                // Fallback: buscar en Query String
                if (string.IsNullOrEmpty(paymentId) && Request.Query.ContainsKey("data.id"))
                {
                    paymentId = Request.Query["data.id"];
                }

                if (string.IsNullOrEmpty(paymentId)) return Ok(); // MP requiere 200 OK siempre

                await _paymentService.HandlePaymentNotificationAsync(paymentId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando webhook: {ex.Message}");
                return Ok(); // Devolvemos 200 para que MP deje de reintentar si el error es de lógica
            }
        }

        // CONFIRMACIÓN MANUAL DESDE EL FRONTEND(Simula el Webhook en local)
    
        [Authorize]
        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromQuery] string paymentId)
        {
            if (string.IsNullOrEmpty(paymentId))
                return BadRequest("El ID de pago es obligatorio.");

            try
            {
                // Reutilizamos la lógica del Webhook para procesar el pago y guardar en DB
                await _paymentService.HandlePaymentNotificationAsync(paymentId);
                return Ok(new { message = "Suscripción actualizada correctamente." });
            }
            catch (Exception ex)
            {
                // Si el pago no existe o no se pudo procesar
                return StatusCode(500, $"Error al confirmar pago: {ex.Message}");
            }
        }

        [Authorize(Roles = "Administrador, SuperAdministrador")]
        [HttpPost("manual")]
        public async Task<IActionResult> ProcessManualPayment([FromBody] ManualPaymentRequest request)
        {
            // 🔹 VALIDAR PRIMERO SI EL USUARIO YA TIENE SUSCRIPCIÓN ACTIVA
            var existingSubscription = await _subscriptionService.GetActiveSubscriptionAsync(request.UserId);
            if (existingSubscription != null && existingSubscription.EndDate > DateTime.Now)
            {
                return BadRequest(new
                {
                    message = "El usuario ya tiene una suscripción activa."
                });
            }

            // 🔹 SI NO TIENE SUSCRIPCIÓN, PROCEDER CON EL PAGO
            try
            {
                var success = await _paymentService.ProcessManualPaymentAsync(
                    request.UserId,
                    request.PlanId,
                    request.Monto
                );

                if (!success)
                    return BadRequest(new { message = "No se pudo procesar el pago manual." });

                return Ok(new { message = "Suscripción activada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en pago manual: {ex.Message}");
                return StatusCode(500, new { message = "Error interno al procesar el pago." });
            }
        }

        [HttpGet("active/{userId}")]
        public async Task<IActionResult> GetActiveSubscription(int userId)
        {
            var subscription = await _subscriptionService.GetActiveSubscriptionAsync(userId);

            return Ok(new
            {
                hasActiveSubscription = subscription != null,
                subscription = subscription
            });
        }

        // Helper para obtener el UserId
        private int? GetUserIdFromClaims()
        {
            var claim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            return claim != null ? int.Parse(claim.Value) : null;
        }
    }



    // DTO auxiliar para pagos manuales
    public class ManualPaymentRequest
    {
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public decimal Monto { get; set; }
    }
}