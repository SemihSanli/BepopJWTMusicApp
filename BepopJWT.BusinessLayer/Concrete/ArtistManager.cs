using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DataAccessLayer.Abstract;
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
        private readonly IArtistDal _artistDal;

        public ArtistManager(IArtistDal artistDal)
        {
            _artistDal = artistDal;
        }

        public async Task TAddAsync(Artist entity)
        {
           await _artistDal.AddAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            var value = await _artistDal.GetByIdAsync(id);
            if (value == null)
            {
                throw new Exception("Artis Bulunamadı");
            }
            else
            {
                await _artistDal.DeleteAsync(id);
            }
        }
        public async Task<List<Artist>> TGetAllAsync()
        {
           return await _artistDal.GetAllAsync();
        }

        public async Task<Artist> TGetByIdAsync(int id)
        {
            return await _artistDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Artist entity)
        {
           await _artistDal.UpdateAsync(entity);
        }
    }
}
