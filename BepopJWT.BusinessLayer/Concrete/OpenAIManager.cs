using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.BusinessLayer.Options.OpenAIOptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class OpenAIManager : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAISettings _settings;

        public OpenAIManager(HttpClient httpClient, IOptions<OpenAISettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value; // Ayarlara buradan ulaşıyoruz
        }

        public async Task<List<string>> GetSongSuggestionsAsync(string prompt, string userMood)
        {
            try
            {
                var requestBody = new
                {
                    // Model ismini appsettings'den alıyorsun, doğru olduğundan emin ol.
                    // Test için buraya elle "gpt-4o-mini" yazıp deneyebilirsin.
                    model = _settings.Model,
                    messages = new[]
                    {
                new { role = "system", content = prompt },
                new { role = "user", content = userMood }
            },
                    temperature = 1.0,
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
                request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");
                request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                    string aiContent = result.choices[0].message.content;
                    return new List<string> { aiContent };
                }
                else
                {
                    // --- GÜNCELLEME BURADA ---
                    // Hata durumunda sadece kodu değil, hatanın içeriğini de okuyoruz.
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Hata mesajını konsola veya loga yazdırabilirsin debugging için
                    // Örnek çıktı: "API Hatası: {error: { message: 'You exceeded your current quota', ... }}"
                    return new List<string> { $"API Hatası ({response.StatusCode}): {errorContent}" };
                }
            }
            catch (Exception ex)
            {
                return new List<string> { $"Sistem Hatası: {ex.Message}" };
            }
        }
    }
}
