using Application.Abstraction;
using Application.Abstraction.ExternalService;
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
        private readonly IPaymentGateway _paymentGateway;
        private readonly IConfiguration _configuration;



        public PaymentService(IPaymentRepository paymentRepository, IPaymentGateway paymentGateway, IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _paymentGateway = paymentGateway;
            _configuration = configuration;
        }


        public async Task<(string InitPoint, string PreferenceId)> CreatePaymentPreferenceAsync(CreateMercadoPagoRequest request)
        {
            if (request.Monto <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero.");

            var (initPoint, preferenceId) = await _paymentGateway.CreatePreferenceAsync(request);

            // Crear el pago en la base de datos como pendiente
            var payment = new Payment
            {
                UserId = request.UserId,
                Monto = request.Monto,
                Fecha = DateTime.Now,
                Pagado = false,
                PreferenceId = preferenceId,
                InitPoint = initPoint
            };

            _paymentRepository.CreatePayment(payment);

            return (initPoint, preferenceId);
        }

        public async Task HandlePaymentNotificationAsync(string mercadoPagoPaymentId)
        {
            if (string.IsNullOrEmpty(mercadoPagoPaymentId)) return;

            var (status, externalReference) = await _paymentGateway.GetPaymentAsync(mercadoPagoPaymentId);
            if (string.IsNullOrEmpty(externalReference)) return;

            // El external_reference contiene el UserId
            if (!int.TryParse(externalReference, out int userId)) return;

            // Buscar el pago pendiente más reciente de este usuario
            var pendingPayments = _paymentRepository.GetPaymentsByUserId(userId)
                .Where(p => !p.Pagado)
                .OrderByDescending(p => p.Fecha)
                .ToList();

            if (!pendingPayments.Any()) return;

            var payment = pendingPayments.First();
            bool isPaid = status == "approved";

            if (isPaid && !payment.Pagado)
            {
                payment.Pagado = true;
                payment.Fecha = DateTime.Now;
                _paymentRepository.UpdatePayment(payment);
            }
        }

        public async Task VerifyUserPaymentAsync(int userId)
        {
            // 1. Obtener todos los pagos pendientes del usuario
            var pendingPayments = _paymentRepository.GetPaymentsByUserId(userId)
                .Where(p => !p.Pagado)
                .OrderByDescending(p => p.Id)
                .ToList();

            if (!pendingPayments.Any()) return;

            // 2. Intentar verificar primero por PreferenceId (más preciso)
            foreach (var payment in pendingPayments.Where(p => !string.IsNullOrEmpty(p.PreferenceId)))
            {
                var (status, mpPaymentId) = await _paymentGateway.GetPaymentByPreferenceIdAsync(payment.PreferenceId!);
                if (status == "approved")
                {
                    payment.Pagado = true;
                    payment.Fecha = DateTime.Now;
                    _paymentRepository.UpdatePayment(payment);
                    return; // Ya encontramos y procesamos el pago
                }
            }

            // 3. Si no funcionó o no había PreferenceId, intentar por ExternalReference (UserId)
            // Esto es un fallback para pagos viejos
            var (statusGeneric, paymentIdGeneric) = await _paymentGateway.GetPaymentByPreferenceAsync(userId.ToString());

            if (statusGeneric == "approved" && !string.IsNullOrEmpty(paymentIdGeneric))
            {
                var paymentToUpdate = pendingPayments.First();
                paymentToUpdate.Pagado = true;
                paymentToUpdate.Fecha = DateTime.Now;
                _paymentRepository.UpdatePayment(paymentToUpdate);
            }
        }

        public List<PaymentResponse> GetAllPayments()
        {
            var payments = _paymentRepository.GetAll();
            return payments.Select(p => new PaymentResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                Monto = p.Monto,
                Fecha = p.Fecha,
                Pagado = p.Pagado
            }).ToList();
        }

        public PaymentResponse? GetPaymentById(int id)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment == null) return null;

            return new PaymentResponse
            {
                Id = payment.Id,
                UserId = payment.UserId,
                Monto = payment.Monto,
                Fecha = payment.Fecha,
                Pagado = payment.Pagado
            };
        }

        public PaymentResponse? CreatePayment(CreatePaymentRequest request)
        {
            if (request.Monto <= 0)
                return null;
            //validar que el usuario existe (esta logueado)


            var payment = new Payment
            {
                UserId = request.UserId,
                Monto = request.Monto,
                Fecha = request.Fecha,
                Pagado = request.Pagado
            };

            var created = _paymentRepository.CreatePayment(payment);
            if (created == null) return null;

            return new PaymentResponse
            {
                Id = created.Id,
                UserId = created.UserId,
                Monto = created.Monto,
                Fecha = created.Fecha,
                Pagado = created.Pagado
            };
        }

        public PaymentResponse? UpdatePayment(int id, UpdatePaymentRequest request)
        {
            var existing = _paymentRepository.GetById(id);
            if (existing == null)
                return null;

            existing.UserId = request.UserId;
            existing.Monto = request.Monto;
            existing.Fecha = request.Fecha;
            existing.Pagado = request.Pagado;

            var updated = _paymentRepository.UpdatePayment(existing);
            if (updated == null) return null;

            return new PaymentResponse
            {
                Id = updated.Id,
                UserId = updated.UserId,
                Monto = updated.Monto,
                Fecha = updated.Fecha,
                Pagado = updated.Pagado
            };
        }

        public bool DeletePayment(int id)
        {
            var existing = _paymentRepository.GetById(id);
            return existing != null && _paymentRepository.DeletePayment(id);
        }

        public List<PaymentResponse> GetPaymentsByUserId(int userId)
        {
            var payments = _paymentRepository.GetPaymentsByUserId(userId);
            return payments.Select(p => new PaymentResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                Monto = p.Monto,
                Fecha = p.Fecha,
                Pagado = p.Pagado
            }).ToList();
        }

        public List<PaymentResponse> GetPendingPaymentsByUserId(int userId)
        {
            var payments = _paymentRepository.GetPaymentsByUserId(userId);
            var pending = payments.Where(p => !p.Pagado).ToList();
            return pending.Select(p => new PaymentResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                Monto = p.Monto,
                Fecha = p.Fecha,
                Pagado = p.Pagado
            }).ToList();
        }
    }
}

