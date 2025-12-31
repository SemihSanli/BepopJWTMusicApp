using System.Net.Http.Headers;

namespace BepopJWT.Consume.Helpers
{
    public class ApiClientHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClientHelper(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpClient GetClient()
        {
            var client = _httpClientFactory.CreateClient();

            // Oturum açmış mı kontrol et
            var token = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "AccessToken")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
