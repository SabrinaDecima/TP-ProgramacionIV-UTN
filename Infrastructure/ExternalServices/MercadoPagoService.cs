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

            // Estructura correcta según la API de MercadoPago
            var preferenceRequest = new
            {
                items = new[]
                {
                    new
                    {
                        id = request.UserId.ToString(),
                        title = request.Descripcion ?? "Membresía Gym",
                        quantity = 1,
                        currency_id = "ARS",
                        unit_price = Math.Round(request.Monto, 2)
                    }
                },
                back_urls = new
                {
                    success = $"{frontendBase}/pagos",
                    failure = $"{frontendBase}/pagos",
                    pending = $"{frontendBase}/pagos"
                },
                auto_return = "approved",
                binary_mode = true,
                external_reference = request.UserId.ToString()
            };

            var json = JsonSerializer.Serialize(preferenceRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine($"AccessToken: {accessToken}");
            Console.WriteLine($"Sending to Mercado Pago: {json}");

            HttpResponseMessage response = null;
            string responseJson = string.Empty;

            try
            {
                response = await client.PostAsync("https://api.mercadopago.com/checkout/preferences", content);
                responseJson = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Mercado Pago Response Status: {response.StatusCode}");
                Console.WriteLine($"Mercado Pago Response Body: {responseJson}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error Mercado Pago ({response.StatusCode}): {responseJson}");
                }

                using var doc = JsonDocument.Parse(responseJson);
                var root = doc.RootElement;

                if (!root.TryGetProperty("init_point", out var initPointElement))
                {
                    throw new InvalidOperationException($"Respuesta sin init_point. Respuesta: {responseJson}");
                }

                if (!root.TryGetProperty("id", out var idElement))
                {
                    throw new InvalidOperationException($"Respuesta sin id. Respuesta: {responseJson}");
                }

                var initPoint = initPointElement.GetString();
                var preferenceId = idElement.GetString();

                if (string.IsNullOrEmpty(initPoint) || string.IsNullOrEmpty(preferenceId))
                {
                    throw new InvalidOperationException($"initPoint o preferenceId están vacíos. Respuesta: {responseJson}");
                }

                return (initPoint, preferenceId);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                Console.WriteLine($"Response: {responseJson}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CreatePreferenceAsync: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<(string Status, string PreferenceId)> GetPaymentAsync(string paymentId)
        {
            var client = _httpClientFactory.CreateClient("MercadoPago");
            var accessToken = _configuration["MercadoPago:AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("Access Token de Mercado Pago no configurado.");

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                var response = await client.GetAsync($"https://api.mercadopago.com/v1/payments/{paymentId}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error Mercado Pago ({response.StatusCode}): {errorContent}");
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseJson);

                var status = doc.RootElement.GetProperty("status").GetString() ?? string.Empty;
                var preferenceId = doc.RootElement.TryGetProperty("preference_id", out var pref) 
                    ? pref.GetString() ?? string.Empty 
                    : string.Empty;

                return (status, preferenceId);
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Error al obtener pago de Mercado Pago: {ex.Message}", ex);
            }
        }

    }
}