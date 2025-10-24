
using Application.Abstraction;
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
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public PaymentService(IPaymentRepository paymentRepository, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }


        public async Task<string> CreatePaymentPreferenceAsync(CreateMercadoPagoRequest request)
        {
            if (request.Monto <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero.");

            var client = _httpClientFactory.CreateClient("MercadoPago");
            var accessToken = _configuration["MercadoPago:AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("Access Token de Mercado Pago no configurado.");

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var preferenceRequest = new
            {
                items = new[]
                {
            new
            {
                 title = request.Descripcion ?? "Membresía Gym",
                quantity = 1,
                currency_id = "ARS",
                unit_price = (double)request.Monto
            }
        },
                back_urls = new
                {
                    success = "https://tusitio.com/success",
                    failure = "https://tusitio.com/failure",
                    pending = "https://tusitio.com/pending"
                },
                auto_return = "approved"
            };

            var json = JsonSerializer.Serialize(preferenceRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/checkout/preferences", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error Mercado Pago ({response.StatusCode}): {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            var initPoint = doc.RootElement.GetProperty("init_point").GetString();

            return initPoint ?? throw new InvalidOperationException("Respuesta de Mercado Pago sin init_point.");
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
            //juntar este post con el de mercado pago

            var payment = new Payment
            {
                UserId = request.UserId,
                Monto = request.Monto,
                Fecha = request.Fecha ?? string.Empty,
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
            existing.Fecha = request.Fecha ?? string.Empty;
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