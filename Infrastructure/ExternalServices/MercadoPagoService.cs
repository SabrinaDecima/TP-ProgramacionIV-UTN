using Application.Abstraction.ExternalService;
using Contracts.Payment.Request;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices
{
    public class MercadoPagoService : IPaymentGateway
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MercadoPagoService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<(string InitPoint, string PreferenceId)> CreatePreferenceAsync(CreateMercadoPagoRequest request)
        {
            var client = _httpClientFactory.CreateClient("MercadoPago");
            var accessToken = _configuration["MercadoPago:AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("Access Token de Mercado Pago no configurado.");

            client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var frontendBase = _configuration["Frontend:BaseUrl"] ?? "http://localhost:4200";

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
                    success = frontendBase + "/pagos",
                    failure = frontendBase + "/pagos",
                    pending = frontendBase + "/pagos"
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

            var initPoint = doc.RootElement.GetProperty("init_point").GetString()
        ?? throw new InvalidOperationException("Respuesta sin init_point.");

            var preferenceId = doc.RootElement.GetProperty("id").GetString()
                ?? throw new InvalidOperationException("Respuesta sin preferenceId.");



            return (initPoint, preferenceId);
        }

        public async Task<(string Status, string PreferenceId)> GetPaymentAsync(string paymentId)
        {
            var client = _httpClientFactory.CreateClient("MercadoPago");
            var accessToken = _configuration["MercadoPago:AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("Access Token de Mercado Pago no configurado.");

            client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"/v1/payments/{paymentId}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error Mercado Pago ({response.StatusCode}): {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            var status = doc.RootElement.GetProperty("status").GetString() ?? string.Empty;
            var preferenceId = doc.RootElement.TryGetProperty("preference_id", out var pref) ? pref.GetString() ?? string.Empty : string.Empty;

            return (status, preferenceId);
        }

    }
}

