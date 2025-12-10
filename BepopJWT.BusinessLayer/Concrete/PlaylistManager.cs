using BepopJWT.BusinessLayer.Abstract;
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
        public Task TAddAsync(Playlist entity)
        {
            throw new NotImplementedException();
        }

        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Playlist>> TGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Playlist> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(Playlist entity)
        {
            throw new NotImplementedException();
        }
    }
}
