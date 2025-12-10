using BepopJWT.BusinessLayer.Abstract;
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
        public Task TAddAsync(Song entity)
        {
            throw new NotImplementedException();
        }

        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Song>> TGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Song> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(Song entity)
        {
            throw new NotImplementedException();
        }
    }
}
