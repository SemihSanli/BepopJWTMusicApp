using BepopJWT.Consume.ArtistDTOs;
using BepopJWT.Consume.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _MainPageComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiClientHelper _apiClientHelper;
        public _MainPageComponentPartial(IHttpClientFactory httpClientFactory, ApiClientHelper apiClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiClientHelper = apiClientHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _apiClientHelper.GetClient();
            var client2 = _httpClientFactory.CreateClient();

          
            var response = await client.GetAsync("https://localhost:7209/api/Artists");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultArtistDTO>>(jsonData);

              
                return View(values.Take(10).ToList());
            }

          
            return View(new List<ResultArtistDTO>());
        }
    }
}
