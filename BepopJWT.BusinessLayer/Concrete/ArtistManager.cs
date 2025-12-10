using BepopJWT.BusinessLayer.Abstract;
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
        
        public Task TAddAsync(Artist entity)
        {
            throw new NotImplementedException();
        }

        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Artist>> TGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Artist> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(Artist entity)
        {
            throw new NotImplementedException();
        }
    }
}
