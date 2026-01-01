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
        private readonly IOpenAIService _openAIService;
        private readonly IPackageService _packageService;
        public SongManager(ISongDal songDal, IFileUploadService fileUploadService, IUserService userService, IOpenAIService openAIService, IPackageService packageService)
        {
            _songDal = songDal;
            _fileUploadService = fileUploadService;
            _userService = userService;
            _openAIService = openAIService;
            _packageService = packageService;
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

           
            if (createSongDto.ImageFile != null)
            {
                var imageDto = new UploadImageDTO { imageFile = createSongDto.ImageFile };

              
                newSong.ImageUrl = await _fileUploadService.UploadImageAsync(imageDto, "bepop_covers");
            }

            await _songDal.AddAsync(newSong);
        }

        public async Task<bool> CheckSongAccessAsync(int songId, int userId)
        {
            var song = await _songDal.GetByIdAsync(songId);
            if (song == null) return false;
            var user = await _userService.TGetUserWithPackageAsync(userId);
            if (user == null) return false;
            int userLevel = user.Package?.PackageLevel ?? 0;

            return userLevel >= song.MinLevelRequired;
        }

        public async Task DeleteWithFileAsync(int id)
        {
            var song = await _songDal.GetByIdAsync(id);
            if (song == null) throw new Exception("Şarkı Bulunamadı");

            await _fileUploadService.DeleteMusicAsync(song.FileUrl);
            await _fileUploadService.DeleteImageAsync(song.ImageUrl);

            await _songDal.DeleteAsync(id);
        }

        public async Task<List<ResultSongWithArtists>> GetSongsByIdsAsync(List<int> ids)
        {
            var allSongs = await _songDal.GetAllAsync();


            var dtos = allSongs
                .Where(x => ids.Contains(x.SongId))
                .Select(x => new ResultSongWithArtists
                {
                    SongId = x.SongId,
                    SongTitle = x.SongTitle,
                    ImageUrl = x.ImageUrl,
                    FileUrl = x.FileUrl,

                    Name = x.Artist != null ? x.Artist.Name : "Bilinmeyen Sanatçı"
                })
                .ToList();

            return dtos;
        }

        public async Task<List<ResultSongWithArtists>> GetSongSuggestionByMoodAsync(int userId, int userPackageId, string userMood)
        {
            var userPackage = await _packageService.TGetByIdAsync(userPackageId);

            if (userPackage == null) return null;

            int userPackageLevel = userPackage.PackageLevel;

        
            var allSongs = await _songDal.GetSongWithArtist();

         
            var authorizedSongs = allSongs
                .Where(song => song.MinLevelRequired <= userPackageLevel)
                .ToList();

            if (!authorizedSongs.Any())
            {
                return new List<ResultSongWithArtists>();
            }

        
            var randomPool = authorizedSongs
                .OrderBy(x => Guid.NewGuid()) 
                .Take(30) 
                .ToList();

            // 4️⃣ AI için liste metni oluşturuyoruz (Artık randomPool üzerinden)
            var songListText = string.Join("\n", randomPool.Select(s =>
                $"- {s.SongTitle} - {s.Artist?.Name ?? "Bilinmiyor"}"
            ));

            string systemPrompt = $@"
Sen 'Bepop DJ' isimli müzik asistanısın.
Kullanıcının modu: '{userMood}'
Aşağıdaki aday listesinden bu moda en uygun 3 şarkıyı seç.
Lütfen şarkı isimlerini listedeki haliyle birebir aynı yaz.

ŞARKI LİSTESİ:
{songListText}
";

         
            List<string> aiSuggestions = await _openAIService.GetSongSuggestionsAsync(systemPrompt, userMood);

            var resultList = new List<ResultSongWithArtists>();

            foreach (var suggestion in aiSuggestions)
            {
             
                var matchedSong = randomPool.FirstOrDefault(s =>
                    suggestion.Contains(s.SongTitle, StringComparison.OrdinalIgnoreCase));

                if (matchedSong != null)
                {
                   
                    if (!resultList.Any(r => r.SongId == matchedSong.SongId))
                    {
                        resultList.Add(new ResultSongWithArtists
                        {
                            SongId = matchedSong.SongId,
                            SongTitle = matchedSong.SongTitle,
                            Name = matchedSong.Artist?.Name ?? "Bilinmeyen Sanatçı",
                            CategoryName = matchedSong.Category?.CategoryName ?? "Genel",
                            FileUrl = matchedSong.FileUrl,
                            ImageUrl = matchedSong.ImageUrl,
                            MinLevelRequired = matchedSong.MinLevelRequired
                        });
                    }
                }
            }

          
            if (!resultList.Any() && randomPool.Any())
            {
                return randomPool.Take(3).Select(x => new ResultSongWithArtists
                {
                    SongId = x.SongId,
                    SongTitle = x.SongTitle,
                    Name = x.Artist?.Name ?? "Bilinmiyor",
                    CategoryName = x.Category?.CategoryName ?? "Genel",
                    FileUrl = x.FileUrl,
                    ImageUrl = x.ImageUrl,
                    MinLevelRequired = x.MinLevelRequired
                }).ToList();
            }

            return resultList;
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
            await _songDal.AddAsync(entity);
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
