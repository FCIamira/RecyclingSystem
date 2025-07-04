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
        protected readonly RecyclingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public IdentityRepository(RecyclingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task Add(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false).ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Remove(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                var prop = typeof(T).GetProperty("IsDeleted");
                if (prop != null)
                {
                    prop.SetValue(entity, true);
                    _context.Entry(entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> RemoveByExpression(Expression<Func<T, bool>> predicate)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                var prop = typeof(T).GetProperty("IsDeleted");
                if (prop != null)
                {
                    prop.SetValue(entity, true);
                    _context.Entry(entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }

        public async Task Update(int id, T obj)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(obj);
            }
        }

        public IQueryable<T> GetAllWithFilter(Expression<Func<T, bool>> filter)
        {
            return _dbSet.Where(filter).Where(e => EF.Property<bool>(e, "IsDeleted") == false);
        }
    }
}
