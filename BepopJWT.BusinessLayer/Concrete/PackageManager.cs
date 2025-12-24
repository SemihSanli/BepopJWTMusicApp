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
    public class PackageManager : IPackageService
    {
        private readonly IPackageDal _packageDal;

        public PackageManager(IPackageDal packageDal)
        {
            _packageDal = packageDal;
        }

        public async Task TAddAsync(Package entity)
        {
          await _packageDal.AddAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _packageDal.DeleteAsync(id);
        }

        public async Task<List<Package>> TGetAllAsync()
        {
           return await _packageDal.GetAllAsync();
        }

        public async Task<Package> TGetByIdAsync(int id)
        {
           return await _packageDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Package entity)
        {
            await _packageDal.UpdateAsync(entity);
        }
    }
}
