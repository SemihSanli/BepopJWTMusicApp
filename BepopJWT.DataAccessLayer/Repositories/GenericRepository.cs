using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly AppDbContext _appDbContext;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(T entity)
        {
          await _appDbContext.Set<T>().AddAsync(entity);
          await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var value = await _appDbContext.Set<T>().FindAsync(id);
            _appDbContext.Set<T>().Remove(value);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
           return await _appDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
           return await _appDbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _appDbContext.Set<T>().Update(entity);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
