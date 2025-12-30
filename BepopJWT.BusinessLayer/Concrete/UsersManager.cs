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
    public class UsersManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UsersManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task TAddAsync(User entity)
        {
           await _userDal.AddAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
           await _userDal.DeleteAsync(id);
        }

        public async Task<List<User>> TGetAllAsync()
        {
            return await _userDal.GetAllAsync();
        }

        public async Task<User?> TGetByEmailAsync(string email)
        {
           return await _userDal.GetByEmailAsync(email);
        }

        public async Task<User> TGetByIdAsync(int id)
        {
           return await _userDal.GetByIdAsync(id);
        }

        public async Task<User> TGetUserWithPackageAsync(int userId)
        {
            return await _userDal.GetUserWithPackageAsync(userId);
        }

        public async Task TUpdateAsync(User entity)
        {
            await _userDal.UpdateAsync(entity);
        }
    }
}
