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
    public class EfSongDal:GenericRepository<Song>,ISongDal
    {
        private readonly AppDbContext _appDbContext;
        public EfSongDal(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Song>> GetSongsWithCategory()
        {
            return await _appDbContext.Songs.Include(c => c.Category).ToListAsync();
        }

        public async Task<List<Song>> GetSongWithArtist()
        {
            return await _appDbContext.Songs.Include(a=>a.Artist).ToListAsync();
        }
    }
}
