using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.CategoryDTOs;
using BepopJWT.EntityLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var Categories = await _categoryService.TGetAllAsync();
            return Ok(Categories);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            try
            {
                var Category = new Category
                {
                    CategoryName = createCategoryDTO.CategoryName
                };
                await _categoryService.TAddAsync(Category);
                return StatusCode(201, new { Message = "Sanatçı Başarıyla Eklendi" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO updateCategoryDTO)
        {
            try
            {

                var Category = new Category
                {
                    CategoryId = updateCategoryDTO.CategoryId,
                    CategoryName = updateCategoryDTO.CategoryName

                };


                await _categoryService.TUpdateAsync(Category);

                return Ok(new { Message = "Sanatçı başarıyla güncellendi." });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            try
            {

                await _categoryService.TDeleteAsync(id);

                return Ok(new { Message = "Sanatçı başarıyla silindi." });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var values = await _categoryService.TGetByIdAsync(id);

            if (values == null)
            {
                return NotFound(new { Message = "Böyle bir sanatçı bulunamadı." });
            }

            return Ok(values);
        }
    }
}
