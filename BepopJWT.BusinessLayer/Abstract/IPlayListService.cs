using BepopJWT.DTOLayer.PlaylistDTOs;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Abstract
{
    public interface IPlayListService:IGenericService<Playlist>
    {
        Task<List<ResultPlaylistWithSongsDTO>> GetPlaylistsByUserIdAsync(int userId);
        Task CreatePlayListAsync(CreatePlaylistDTO createPlaylistDto);
        Task AddSongToPlaylistAsync(AddSongToPlaylistDTO addSongToPlaylistDto);
        Task<ResultPlaylistWithSongsDTO> GetPlaylistWithSongsByIdAsync(int id);
        Task<List<GetPlaylistByUserId>> GetPlaylistByUserId(int userId);
    }
}
