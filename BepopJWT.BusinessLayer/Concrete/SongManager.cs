using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DTOLayer.DTOs.SongDTOs;
using BepopJWT.DTOLayer.FileUploadDTOs;
using BepopJWT.DTOLayer.SongDTOs;
using BepopJWT.EntityLayer.Entities;
using Newtonsoft.Json.Linq;
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
        private readonly IUserService _userService;
        public SongManager(ISongDal songDal, IFileUploadService fileUploadService, IUserService userService)
        {
            _songDal = songDal;
            _fileUploadService = fileUploadService;
            _userService = userService;
        }

        public async Task AddSongWithFileAsync(CreateSongDTO createSongDto)
        {
            var newSong = new Song
            {
                SongTitle = createSongDto.SongTitle,
                CategoryId = createSongDto.CategoryId,
                ArtistId = createSongDto.ArtistId,
                MinLevelRequired = createSongDto.MinLevelRequired,
            };

            // Müzik Yükleme
            if (createSongDto.SongFile != null)
            {
                var musicDto = new UploadMusicDTO { musicFile = createSongDto.SongFile };
                newSong.FileUrl = await _fileUploadService.UploadMusicAsync(musicDto);
            }

            // Resim Yükleme (HATANIN ÇÖZÜLDÜĞÜ YER) 👇
            if (createSongDto.ImageFile != null)
            {
                var imageDto = new UploadImageDTO { imageFile = createSongDto.ImageFile };

                // Buraya "bepop_covers" ekledik, artık patlamaz.
                newSong.ImageUrl = await _fileUploadService.UploadImageAsync(imageDto, "bepop_covers");
            }

            await _songDal.AddAsync(newSong);
        }

        public async Task<bool> CheckSongAccessAsync(int songId, int userId)
        {
           var song = await _songDal.GetByIdAsync(songId);
            if(song == null) return false;
           var user = await _userService.TGetUserWithPackageAsync(userId);
            if (user == null) return false;
            int userLevel = user.Package?.PackageLevel ?? 0;

            return userLevel >= song.MinLevelRequired;
        }

        public async Task DeleteWithFileAsync(int id)
        {
            var song = await _songDal.GetByIdAsync(id);
            if(song==null) throw new Exception("Şarkı Bulunamadı");

            await _fileUploadService.DeleteMusicAsync(song.FileUrl);
            await _fileUploadService.DeleteImageAsync(song.ImageUrl);

            await _songDal.DeleteAsync(id);
        }

        public async Task<List<ResultSongWithArtists>> GetSongsWithArtistsAsync()
        {
            var values = await _songDal.GetSongWithArtist();
            var songDtos = values.Select(x => new ResultSongWithArtists
            {
                SongId = x.SongId,
                SongTitle = x.SongTitle,
                ImageUrl = x.ImageUrl,
                FileUrl = x.FileUrl,
               MinLevelRequired = x.MinLevelRequired,
                CategoryId = x.CategoryId,
                CategoryName = x.Category != null ? x.Category.CategoryName : "Bilinmeyen Kategori",
                Name = x.Artist != null ? x.Artist.Name : "Bilinmeyen Sanatçı"

            }).ToList();
            return songDtos;
        }

        public async Task<List<GetSongsWithCategoryDTO>> GetSongsWithCategoryAsync()
        {
          var values = await _songDal.GetSongsWithCategory();
            var songwithcategorydtos = values.Select(x => new GetSongsWithCategoryDTO
            {
                SongId = x.SongId,
                SongTitle = x.SongTitle,
                ImageUrl = x.ImageUrl,
                FileUrl = x.FileUrl,
                MinLevelRequired = x.MinLevelRequired,
                CategoryName = x.Category != null ? x.Category.CategoryName : "Bilinmeyen Kategori"
            }).ToList();
            return songwithcategorydtos;
        }

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

        public async Task UpdateWithFileAsync(UpdateSongDTO updateSongDto)
        {
            var oldSong = await _songDal.GetByIdAsync(updateSongDto.SongId);
            if (oldSong == null) throw new Exception("Şarkı Bulunamadı");

            oldSong.FileUrl = await _fileUploadService.UpdateMusicAsync(updateSongDto.SongFile, oldSong.FileUrl);
            oldSong.ImageUrl = await _fileUploadService.UpdateImageAsync(updateSongDto.ImageFile, oldSong.ImageUrl, "bepop_covers");

            oldSong.SongTitle = updateSongDto.SongTitle;
            oldSong.MinLevelRequired = updateSongDto.MinLevelRequired;
            oldSong.ArtistId = updateSongDto.ArtistId;
            oldSong.CategoryId = updateSongDto.CategoryId;
            await _songDal.UpdateAsync(oldSong);
        }
    }
}
