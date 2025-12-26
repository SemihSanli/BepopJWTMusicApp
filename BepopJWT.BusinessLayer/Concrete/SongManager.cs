using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DTOLayer.FileUploadDTOs;
using BepopJWT.DTOLayer.SongDTOs;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class SongManager : ISongService
    {
        private readonly ISongDal _songDal;
        private readonly IFileUploadService _fileUploadService;
        public SongManager(ISongDal songDal, IFileUploadService fileUploadService)
        {
            _songDal = songDal;
            _fileUploadService = fileUploadService;
        }

        public async Task AddSongWithFileAsync(CreateSongDTO createSongDto, string rootPath)
        {
            var newSong = new Song
            {
                SongTitle = createSongDto.SongTitle,
                CategoryId = createSongDto.CategoryId,
                ArtistId = createSongDto.ArtistId,
                MinLevelRequired = createSongDto.MinLevelRequired,
            };
            if (createSongDto.SongFile != null)
            {
                var musicDto = new UploadMusicDTO
                {
                    musicFile = createSongDto.SongFile,
                    rootPath = rootPath
                };
                newSong.FileUrl = await _fileUploadService.UploadMusicAsync(musicDto);
            }
            if (createSongDto.ImageFile != null)
            {
                var imageDto = new UploadImageDTO
                {
                    imageFile = createSongDto.ImageFile,
                    rootPath = rootPath
                };
                newSong.ImageUrl = await _fileUploadService.UploadImageAsync(imageDto);
            }
            await _songDal.AddAsync(newSong);
        }
        //URL ile şarkı eklenmek isterse direkt olarak bu metodu kullanırız
        public async Task TAddAsync(Song entity)
        {
          await  _songDal.AddAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _songDal.DeleteAsync(id);
        }

        public async Task<List<Song>> TGetAllAsync()
        {
           return await _songDal.GetAllAsync();
        }

        public async Task<Song> TGetByIdAsync(int id)
        {
            return await _songDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Song entity)
        {
           await _songDal.UpdateAsync(entity);
        }
    }
}
