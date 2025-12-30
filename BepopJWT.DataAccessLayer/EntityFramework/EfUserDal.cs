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
    public class EfUserDal:GenericRepository<User>,IUserDal
    {
        private readonly AppDbContext _appDbContext;
        public EfUserDal(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
          return await _appDbContext.Users.FirstOrDefaultAsync(u=>u.Email == email);
        }

        public async Task<User> GetUserWithPackageAsync(int userId)
        {
          return await _appDbContext.Users.Include(p=>p.Package).FirstOrDefaultAsync(u=>u.UserId == userId);
        }
    }
}
