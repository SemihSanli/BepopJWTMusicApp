using BepopJWT.BusinessLayer.Abstract;
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
        public Task TAddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> TGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
