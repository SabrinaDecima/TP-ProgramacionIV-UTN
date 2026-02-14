using Application.Abstraction;
using Application.Abstraction.ExternalService;
using Application.Interfaces;
using Contracts.GymClass.Request;
using Contracts.Payment.Request;
using Contracts.Payment.Response;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IConfiguration _configuration;



        public PaymentService(
            IPaymentRepository paymentRepository,
            IPlanRepository planRepository,
            IPaymentGateway paymentGateway, 
            ISubscriptionService subscriptionService,
            IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _planRepository = planRepository;
            _paymentGateway = paymentGateway;
            _subscriptionService = subscriptionService;
            _configuration = configuration;
        }


        public async Task<(string InitPoint, string PreferenceId)> CreatePaymentPreferenceAsync(CreateMercadoPagoRequest request)
        {

            var plan =  _planRepository.GetPlanById(request.PlanId);

            if (plan == null)
                throw new Exception($"No se encontró un plan con el ID {request.PlanId}");

            request.Monto = plan.Precio;

            if (request.Monto <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero.");

            //external reference para el webhook
            request.ExternalReference = $"{request.UserId}|{request.PlanId}";

            var (initPoint, preferenceId) = await _paymentGateway.CreatePreferenceAsync(request);

            var nuevoPago = new Payment
            {
                UserId = request.UserId,
                Monto = request.Monto,
                Fecha = DateTime.Now,
                Pagado = false, // Aún no se confirma el pago
                InitPoint = initPoint,
                PreferenceId = preferenceId,
                SubscriptionId = plan.Id, // O el campo que uses para vincular al plan
                MetodoPago = "Mercado Pago"
            };

            await _paymentRepository.CreateAsync(nuevoPago);

            return (initPoint, preferenceId);
        }

        public async Task HandlePaymentNotificationAsync(string mercadoPagoPaymentId)
        {
            var (status, externalReference, preferenceId, amountMP) = await _paymentGateway.GetPaymentAsync(mercadoPagoPaymentId);

            if (status != "approved") return;

            // 1. Intentamos buscar por PreferenceId (como antes)
            var pagoExistente = await _paymentRepository.GetByPreferenceIdAsync(preferenceId);

            // 2. PLAN B: Si no lo encuentra, usamos la External Reference ("7|1")
            if (pagoExistente == null && !string.IsNullOrEmpty(externalReference))
            {
                var parts = externalReference.Split('|');
                if (parts.Length > 0 && int.TryParse(parts[0], out int userId))
                {
                    // Buscamos todos los pagos de este usuario
                    var pagosUsuario = await _paymentRepository.GetByUserIdAsync(userId);

                    // Tomamos el último que todavía figure como NO PAGADO
                    pagoExistente = pagosUsuario
                        .Where(p => !p.Pagado)
                        .OrderByDescending(p => p.Fecha)
                        .FirstOrDefault();
                }
            }

            if (pagoExistente != null)
            {
                pagoExistente.Pagado = true;
                pagoExistente.Monto = amountMP;
                pagoExistente.MetodoPago = "Mercado Pago";

                await _paymentRepository.UpdateAsync(pagoExistente);

                // Aquí puedes activar la suscripción del socio
                
                await _subscriptionService.RenewSubscriptionAsync(
                    pagoExistente.UserId,
                    pagoExistente.SubscriptionId ?? 0,
                    amountMP,
                    "Mercado Pago"
                );
            }
            else
            {
                throw new Exception($"No se pudo encontrar un pago pendiente para la referencia: {externalReference}");
            }
                
            }

 

        

   
        public async Task<List<PaymentResponse>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Select(p => MapToResponse(p)).ToList();
            
        }

        public async Task<PaymentResponse?> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return payment == null ? null : MapToResponse(payment);
        }

       

        public async  Task<List<PaymentResponse>> GetPaymentsByUserIdAsync(int userId)
        {
            var payments = await _paymentRepository.GetByUserIdAsync(userId);
            return payments.Select(p => MapToResponse(p)).ToList();
        }



        // Método para pagos manuales realizados por el Admin (Efectivo)
        public async Task<bool> ProcessManualPaymentAsync(int userId, int planId, decimal amount)
        {
            return await _subscriptionService.CreateSubscriptionAsync(userId, planId, amount, "Efectivo");
        }

        // Mapper auxiliar para no repetir código
        private PaymentResponse MapToResponse(Payment p)
        {
            return new PaymentResponse
            {
                Id = p.Id,
                Monto = p.Monto,
                Fecha = p.Fecha,
                Pagado = p.Pagado,
                MetodoPago = p.MetodoPago,
                SubscriptionId = p.SubscriptionId ?? 0 
            };
        }
    }
}

