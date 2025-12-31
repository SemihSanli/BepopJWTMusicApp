using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DataAccessLayer.Abstract
{
    public interface ISongDal:IGenericDal<Song>
    {
        Task<List<Song>> GetSongWithArtist();
        Task<List<Song>> GetSongsWithCategory();
      
    }
}
