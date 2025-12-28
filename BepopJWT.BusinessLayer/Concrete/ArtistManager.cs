using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DTOLayer.ArtistDTOs;
using BepopJWT.DTOLayer.FileUploadDTOs;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class ArtistManager : IArtistService
    {
        private readonly IArtistDal _artistDal;
        private readonly IFileUploadService _fileUploadService;
        public ArtistManager(IArtistDal artistDal, IFileUploadService fileUploadService)
        {
            _artistDal = artistDal;
            _fileUploadService = fileUploadService;
        }

        public async Task CreateArtistWithImageFileAsync(CreateArtistDTO createArtistDto)
        {
            string imageUrl = "";

            if (createArtistDto.ImageUrl != null)
            {
                // Artist resimleri "bepop_artists" klasörüne gider
                imageUrl = await _fileUploadService.UploadImageAsync(
                    new UploadImageDTO { imageFile = createArtistDto.ImageUrl },
                    "bepop_artists"
                );
            }

            var artist = new Artist
            {
                Name = createArtistDto.Name,
                Bio = createArtistDto.Bio,
                ImageUrl = imageUrl
            };

            await _artistDal.AddAsync(artist);
        }

        public async Task DeleteArtistWithImageFileAsync(int id)
        {
            var artist = await _artistDal.GetByIdAsync(id);
            if (artist == null) throw new Exception("Silinecek sanatçı bulunamadı.");

           
            await _fileUploadService.DeleteImageAsync(artist.ImageUrl);

            
            await _artistDal.DeleteAsync(id); 
        }

        public async Task TAddAsync(Artist entity)
        {
           await _artistDal.AddAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            var value = await _artistDal.GetByIdAsync(id);
            if (value == null)
            {
                throw new Exception("Artis Bulunamadı");
            }
            else
            {
                await _artistDal.DeleteAsync(id);
            }
        }
        public async Task<List<Artist>> TGetAllAsync()
        {
           return await _artistDal.GetAllAsync();
        }

        public async Task<Artist> TGetByIdAsync(int id)
        {
            return await _artistDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Artist entity)
        {
           await _artistDal.UpdateAsync(entity);
        }

        public async Task UpdateArtistWithImageFileAsync(UpdateArtistDTO updateArtistDto)
        {
            var artist = await _artistDal.GetByIdAsync(updateArtistDto.ArtistId);
            if (artist == null) throw new Exception("Sanatçı bulunamadı.");

          
            artist.ImageUrl = await _fileUploadService.UpdateImageAsync(
                updateArtistDto.ImageUrl,
                artist.ImageUrl,
                "bepop_artists" // Klasör adı önemli!
            );

          
            artist.Name = updateArtistDto.Name;
            artist.Bio = updateArtistDto.Bio;

            await _artistDal.UpdateAsync(artist);
        }
    }
}
