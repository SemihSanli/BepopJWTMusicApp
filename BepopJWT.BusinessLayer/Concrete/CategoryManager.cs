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
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public async Task TAddAsync(Category entity)
        {
            await _categoryDal.AddAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _categoryDal.DeleteAsync(id);
        }

        public async Task<List<Category>> TGetAllAsync()
        {
           return await _categoryDal.GetAllAsync();
        }

        public async Task<Category> TGetByIdAsync(int id)
        {
           return await _categoryDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Category entity)
        {
            await _categoryDal.UpdateAsync(entity);
        }
    }
}
