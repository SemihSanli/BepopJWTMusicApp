using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DataAccessLayer.Context;
using BepopJWT.DataAccessLayer.Repositories;
using BepopJWT.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DataAccessLayer.EntityFramework
{
    public class EfArtistDal:GenericRepository<Artist>,IArtistDal
    {
        private readonly AppDbContext _appDbContext;
        public EfArtistDal(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Artist> GetArtistWithSongsByIdAsync(int id)
        {
          return await _appDbContext.Artists.Include(x=>x.Songs).FirstOrDefaultAsync(y=>y.ArtistId == id);
        }
    }
}
