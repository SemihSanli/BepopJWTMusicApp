using BepopJWT.DTOLayer.SongDTOs;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Abstract
{
    public interface ISongService:IGenericService<Song>
    {
        Task AddSongWithFileAsync(CreateSongDTO createSongDto);
        Task UpdateWithFileAsync(UpdateSongDTO updateSongDto);
        Task DeleteWithFileAsync(int id);
       
    }
}
