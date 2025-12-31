using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.PackageDTOs;
using BepopJWT.EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPackage()
        {
            var Package = await _packageService.TGetAllAsync();
            return Ok(Package);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePackage(CreatePackageDTO createPackageDTO)
        {
            try
            {
                var Package = new Package
                {
                    PackageDescription = createPackageDTO.PackageDescription,
                    PackageTitle = createPackageDTO.PackageTitle,
                    Price = createPackageDTO.Price,
                    PackageLevel = createPackageDTO.PackageLevel

                };
                await _packageService.TAddAsync(Package);
                return StatusCode(201, new { Message = "Sanatçı Başarıyla Eklendi" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePackage(UpdatePackageDTO updatePackageDTO)
        {
            try
            {

                var Package = new Package
                {
                    PackageId = updatePackageDTO.PackageId,
                    PackageDescription = updatePackageDTO.PackageDescription,
                    PackageTitle = updatePackageDTO.PackageTitle,
                    Price = updatePackageDTO.Price,
                    PackageLevel = updatePackageDTO.PackageLevel


                };


                await _packageService.TUpdateAsync(Package);

                return Ok(new { Message = "Sanatçı başarıyla güncellendi." });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePackage(int id)
        {
            try
            {

                await _packageService.TDeleteAsync(id);

                return Ok(new { Message = "Sanatçı başarıyla silindi." });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            var values = await _packageService.TGetByIdAsync(id);

            if (values == null)
            {
                return NotFound(new { Message = "Böyle bir sanatçı bulunamadı." });
            }

            return Ok(values);
        }
    }
}
