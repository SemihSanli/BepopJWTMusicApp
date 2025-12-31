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
    public class EfPlaylistDal:GenericRepository<Playlist>, IPlaylistDal
    {
        private readonly AppDbContext _appDbContext;
        public EfPlaylistDal(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Playlist>> GetPlaylistByUserId(int userId)
        {
            return await _appDbContext.Playlists
             .Where(x => x.UserId == userId)
             .Include(x => x.PlaylistSongs)
                 .ThenInclude(x => x.Song)
                 .ThenInclude(x => x.Artist)
             .ToListAsync(); 
        }

        public async Task<Playlist> GetPlaylistWithSongsByIdAsync(int userId)
        {
            return await _appDbContext.Playlists
           .Include(x => x.PlaylistSongs)
               .ThenInclude(x => x.Song)
                   .ThenInclude(x => x.Artist)
           .FirstOrDefaultAsync(x => x.PlaylistId == userId);
        }

        public async Task<List<Playlist>> GetPlaylistWithUserAndSongsAsync(int userId)
        {
            return await _appDbContext.Playlists
                .Include(u=>u.User)
               .Include(x => x.PlaylistSongs)
               .ThenInclude(y => y.Song)
               .ThenInclude(v=>v.Artist)
               .Where(z => z.UserId == userId).ToListAsync();
        }
    }
}
