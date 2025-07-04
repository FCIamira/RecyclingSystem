using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Infrastructure.Repository
{
    public class IdentityRepository<T> : IIdentityRepository<T> where T : IdentityUser<int> 
    {
        private readonly RecyclingDbContext _context;
        private readonly DbSet<T> _dbSet;

        public IdentityRepository(RecyclingDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Add(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public async Task Remove(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task Update(int id, T obj)
        {
            var existing = await _dbSet.FindAsync(id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(obj);
            }
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> GetAllWithFilter(Expression<Func<T, bool>> filter)
        {
            return _dbSet.Where(filter);
        }

        public async Task<bool> RemoveByExpression(Expression<Func<T, bool>> predicate)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
