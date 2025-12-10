using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DataAccessLayer.Context;
using BepopJWT.DataAccessLayer.Repositories;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DataAccessLayer.EntityFramework
{
    public class EfSongDal:GenericRepository<Song>,ISongDal
    {
        public EfSongDal(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
