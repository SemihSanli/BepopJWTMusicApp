using BepopJWT.Consume.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _GenresCategoryComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _GenresCategoryComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // API'den Kategorileri çekiyoruz
            var response = await client.GetAsync("https://localhost:7209/api/Categories"); 

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ResultCategoryDTO>>(jsonData);
                return View(categories);
            }

            return View(new List<ResultCategoryDTO>());
        }
    }
}
