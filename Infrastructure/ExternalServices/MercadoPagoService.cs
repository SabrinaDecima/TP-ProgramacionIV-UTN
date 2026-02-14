using Application.Abstraction.ExternalService;
using Azure;
using Contracts.Payment.Request;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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

      

            var preference = new Dictionary<string, object>
            {
                ["items"] = new[]
                {
                    new
                    {
                        title = request.Title?? "Membresía Gym",
                        quantity = 1,
                        currency_id = "ARS",
                        unit_price = request.Monto
                    }
                },
                ["external_reference"] = request.ExternalReference ?? request.UserId.ToString(),
                ["back_urls"] = new Dictionary<string, string>
                {
                    ["success"] = "http://localhost:4200/pagos",
                    ["pending"] = "http://localhost:4200/pagos",
                    ["failure"] = "http://localhost:4200/pagos"
                },
                ["binary_mode"] = true

            };

            var options = new JsonSerializerOptions();
           

            var json = JsonSerializer.Serialize(preference, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.mercadopago.com/checkout/preferences", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Error Mercado Pago: {responseJson}");

            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            var preferenceId = root.GetProperty("id").GetString();
            var initPoint = root.GetProperty("init_point").GetString();

            return (initPoint!, preferenceId!);

        }

        public async Task<(string Status, string ExternalReference, string PreferenceId, decimal Amount)> GetPaymentAsync(string paymentId)
        {
            var client = GetConfiguredClient();
            var response = await client.GetAsync($"https://api.mercadopago.com/v1/payments/{paymentId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error Mercado Pago al obtener pago {paymentId}: {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();


            // 1. ESTO ES PARA VOS: Mira la consola de Visual Studio cuando ejecutes esto
            Console.WriteLine("--- JSON RECIBIDO DE MERCADO PAGO ---");
            Console.WriteLine(responseJson);
            Console.WriteLine("-------------------------------------");



            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            // 1. Extraemos datos básicos
            var status = root.GetProperty("status").GetString() ?? string.Empty;
            var extRef = root.TryGetProperty("external_reference", out var ex) ? ex.GetString() : string.Empty;
            var amount = root.TryGetProperty("transaction_amount", out var amt) ? amt.GetDecimal() : 0;

            // 2. BUSQUEDA ROBUSTA DEL PREFERENCE_ID (Solución al error NULL)
            string prefId = string.Empty;

            // INTENTO 1: Buscar en preference_id
            if (root.TryGetProperty("preference_id", out var p) && p.ValueKind != JsonValueKind.Null)
                prefId = p.GetString() ?? "";

            // INTENTO 2: Buscar en order.id
            if (string.IsNullOrEmpty(prefId) && root.TryGetProperty("order", out var order))
                if (order.TryGetProperty("id", out var orderId))
                    prefId = orderId.GetString() ?? "";

            // INTENTO 3 (EL SALVAVIDAS): Buscar en metadata (a veces se guarda ahí)
            if (string.IsNullOrEmpty(prefId) && root.TryGetProperty("metadata", out var meta))
                if (meta.TryGetProperty("preference_id", out var mId))
                    prefId = mId.GetString() ?? "";

            // SI DESPUÉS DE TODO ESTO SIGUE VACÍO, USAREMOS LA REFERENCIA EXTERNA COMO PLAN B
            return (status, extRef, prefId, amount);

        }

      
    

    // --- HELPER PARA CONFIGURAR EL CLIENTE HTTP Y EL TOKEN ---
        private HttpClient GetConfiguredClient()
        {
            var client = _httpClientFactory.CreateClient("MercadoPago");
            var accessToken = _configuration["MercadoPago:AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("Access Token de Mercado Pago no configurado en appsettings.json.");

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            return client;
        }
    }
}