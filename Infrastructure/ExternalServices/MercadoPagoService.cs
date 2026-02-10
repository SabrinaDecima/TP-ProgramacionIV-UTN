using Application.Abstraction.ExternalService;
using Contracts.Payment.Request;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;
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

            // Crear preferencia según documentación oficial de Mercado Pago
            var backUrls = new
            {
                success = $"{frontendBase}/pagos",
                failure = $"{frontendBase}/pagos",
                pending = $"{frontendBase}/pagos"
            };

            var preference = new Dictionary<string, object>
            {
                ["items"] = new[]
                {
                    new
                    {
                        title = request.Descripcion ?? "Membresía Gym",
                        quantity = 1,
                        currency_id = "ARS",
                        unit_price = request.Monto
                    }
                },
                ["back_urls"] = backUrls,
                ["external_reference"] = request.UserId.ToString()
            };

            // Solo añadir auto_return si el frontend es HTTPS
            if (frontendBase.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                preference["auto_return"] = "approved";
            }

            // Añadir email del pagador si está disponible
            if (!string.IsNullOrEmpty(request.Email))
            {
                preference["payer"] = new { email = request.Email };
            }

            var json = JsonSerializer.Serialize(preference, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.mercadopago.com/checkout/preferences", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error Mercado Pago ({response.StatusCode}): {responseJson}");
                throw new HttpRequestException($"Error al crear preferencia en Mercado Pago: {responseJson}");
            }

            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            var preferenceId = root.GetProperty("id").GetString();
            var initPoint = accessToken.StartsWith("TEST") && root.TryGetProperty("sandbox_init_point", out var sandbox)
                ? sandbox.GetString()
                : root.GetProperty("init_point").GetString();

            if (string.IsNullOrEmpty(initPoint) || string.IsNullOrEmpty(preferenceId))
                throw new InvalidOperationException("Respuesta de Mercado Pago sin init_point o id");

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

            var response = await client.GetAsync($"https://api.mercadopago.com/v1/payments/{paymentId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error obteniendo pago {paymentId}: {errorContent}");
                throw new HttpRequestException($"Error Mercado Pago ({response.StatusCode}): {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            var status = root.GetProperty("status").GetString() ?? string.Empty;
            var preferenceId = root.TryGetProperty("external_reference", out var extRef)
                ? extRef.GetString() ?? string.Empty
                : string.Empty;

            return (status, preferenceId);
        }

        public async Task<(string Status, string PaymentId)> GetPaymentByPreferenceAsync(string externalReference)
        {
            var client = _httpClientFactory.CreateClient("MercadoPago");
            var accessToken = _configuration["MercadoPago:AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("Access Token de Mercado Pago no configurado.");

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // Buscamos pagos recientes que tengan este external_reference (UserId)
            // Ordenamos por fecha de creación descendente para obtener el más nuevo
            var searchResponse = await client.GetAsync($"https://api.mercadopago.com/v1/payments/search?external_reference={externalReference}&sort=date_created&criteria=desc");

            if (!searchResponse.IsSuccessStatusCode)
            {
                var error = await searchResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Error buscando pagos para {externalReference}: {error}");
                return (string.Empty, string.Empty);
            }

            var searchJson = await searchResponse.Content.ReadAsStringAsync();
            using var searchDoc = JsonDocument.Parse(searchJson);

            if (!searchDoc.RootElement.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
            {
                Console.WriteLine($"No se encontraron pagos en MP para el external_reference: {externalReference}");
                return (string.Empty, string.Empty);
            }

            // Buscamos si alguno de los resultados (especialmente los más recientes) está aprobado
            var payment = results.EnumerateArray()
                .FirstOrDefault(p => p.TryGetProperty("status", out var s) && s.GetString() == "approved");

            // Si ninguno está aprobado, tomamos el más reciente de todos
            if (payment.ValueKind == JsonValueKind.Undefined)
            {
                payment = results.EnumerateArray().First();
            }

            var status = payment.GetProperty("status").GetString() ?? string.Empty;
            var paymentId = payment.GetProperty("id").GetRawText(); // GetRawText para evitar problemas con números largos

            Console.WriteLine($"Resultado de búsqueda en MP para {externalReference}: Status={status}, PaymentId={paymentId}");

            return (status, paymentId);
        }

        public async Task<(string Status, string PaymentId)> GetPaymentByPreferenceIdAsync(string preferenceId)
        {
            var client = _httpClientFactory.CreateClient("MercadoPago");
            var accessToken = _configuration["MercadoPago:AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("Access Token de Mercado Pago no configurado.");

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // Buscamos pagos asociados a esta preferencia específica
            var searchResponse = await client.GetAsync($"https://api.mercadopago.com/v1/payments/search?preference_id={preferenceId}");

            if (!searchResponse.IsSuccessStatusCode)
                return (string.Empty, string.Empty);

            var searchJson = await searchResponse.Content.ReadAsStringAsync();
            using var searchDoc = JsonDocument.Parse(searchJson);

            if (!searchDoc.RootElement.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
                return (string.Empty, string.Empty);

            // Tomamos el pago aprobado si existe, sino el primero
            var payment = results.EnumerateArray()
                .FirstOrDefault(p => p.TryGetProperty("status", out var s) && s.GetString() == "approved");

            if (payment.ValueKind == JsonValueKind.Undefined)
                payment = results.EnumerateArray().First();

            var status = payment.GetProperty("status").GetString() ?? string.Empty;
            var paymentId = payment.GetProperty("id").GetRawText();

            return (status, paymentId);
        }
    }
}