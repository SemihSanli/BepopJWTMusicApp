using BepopJWT.Consume.CategoryDTOs;
using BepopJWT.Consume.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _GenresCategoryComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiClientHelper _apiClientHelper;
        public _GenresCategoryComponentPartial(IHttpClientFactory httpClientFactory, ApiClientHelper apiClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiClientHelper = apiClientHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _apiClientHelper.GetClient();
            var client2 = _httpClientFactory.CreateClient();

           
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
