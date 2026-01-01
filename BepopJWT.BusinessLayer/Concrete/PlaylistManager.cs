using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DTOLayer.PlaylistDTOs;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class PlaylistManager : IPlayListService
    {
        private readonly IPlaylistDal _playlistDal;
        private readonly IPlaylistSongDal _playlistSongDal;
        public PlaylistManager(IPlaylistDal playlistDal, IPlaylistSongDal playlistSongDal)
        {
            _playlistDal = playlistDal;
            _playlistSongDal = playlistSongDal;
        }

        public async Task AddSongToPlaylistAsync(AddSongToPlaylistDTO addSongToPlaylistDto)
        {
            var playlistSong = new PlaylistSong
            {
                PlaylistId = addSongToPlaylistDto.PlaylistId,
                SongId = addSongToPlaylistDto.SongId,
                AddedAt = DateTime.UtcNow 
            };

            await _playlistSongDal.AddAsync(playlistSong);
        }

        public async Task CreatePlayListAsync(CreatePlaylistDTO createPlaylistDto)
        {
            var playlist = new Playlist
            {
                PlaylistName = createPlaylistDto.PlaylistName,
                UserId = createPlaylistDto.UserId
                
            };
            await _playlistDal.AddAsync(playlist);
        }

        public async Task<List<GetPlaylistByUserId>> GetPlaylistByUserId(int userId)
        {
            var playlists = await _playlistDal.GetPlaylistByUserId(userId);

            var result = playlists.Select(x => new GetPlaylistByUserId
            {
                PlaylistId = x.PlaylistId,
                PlaylistName = x.PlaylistName,
                UserId = x.UserId,

             
                Songs = x.PlaylistSongs.Select(ps => new ResultsSongsForPlaylistDTO
                {
                    SongId = ps.Song.SongId,
                    SongTitle = ps.Song.SongTitle,
                    ArtistName = ps.Song.Artist?.Name,
                    ImageUrl = ps.Song.ImageUrl,
                    MinLevelRequired = ps.Song.MinLevelRequired,
                    FileUrl = ps.Song.FileUrl
                }).ToList()
            }).ToList();

            return result;
        }

        public async Task<List<ResultPlaylistWithSongsDTO>> GetPlaylistsByUserIdAsync(int userId)
        {
            var playlistsEntity = await _playlistDal.GetPlaylistWithUserAndSongsAsync(userId);
            var playlistDtos = playlistsEntity.Select(p => new ResultPlaylistWithSongsDTO
            {
                PlaylistId = p.PlaylistId,
                PlaylistName = p.PlaylistName,

                
                UserId = p.UserId,
                Username = p.User !=null ? $"{p.User.FullName} {p.User.Username}" : "Bilinmeyen Kullanıcı",
                                                             

                Songs = p.PlaylistSongs.Select(ps => new ResultsSongsForPlaylistDTO
                {
                    SongId = ps.Song.SongId,
                    SongTitle = ps.Song.SongTitle,
                    ArtistName = ps.Song.Artist.Name, 
                    FileUrl = ps.Song.FileUrl,
                    ImageUrl = ps.Song.ImageUrl,
                    AddedAt = ps.AddedAt
                }).OrderByDescending(x => x.AddedAt).ToList() 

            }).ToList();

            return playlistDtos;
        }

        public async Task<ResultPlaylistWithSongsDTO> GetPlaylistWithSongsByIdAsync(int id)
        {
            var playlist = await _playlistDal.GetPlaylistWithSongsByIdAsync(id);

            if (playlist == null) return null;

           
            return new ResultPlaylistWithSongsDTO
            {
                PlaylistId = playlist.PlaylistId,
                PlaylistName = playlist.PlaylistName,
                Username = playlist.User?.Username, 
                Songs = playlist.PlaylistSongs.Select(ps => new ResultsSongsForPlaylistDTO
                {
                    SongId = ps.Song.SongId,
                    SongTitle = ps.Song.SongTitle,
                    ArtistName = ps.Song.Artist?.Name,
                    ImageUrl = ps.Song.ImageUrl,
                    FileUrl = ps.Song.FileUrl
                }).ToList()
            };
        }

        public async Task TAddAsync(Playlist entity)
        {
           await _playlistDal.AddAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _playlistDal.DeleteAsync(id);
        }

        public async Task<List<Playlist>> TGetAllAsync()
        {
            return await _playlistDal.GetAllAsync();
        }

        public async Task<Playlist> TGetByIdAsync(int id)
        {
           return await _playlistDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Playlist entity)
        {
          await _playlistDal.UpdateAsync(entity);
        }
    }
}
