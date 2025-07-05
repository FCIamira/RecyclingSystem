using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Interfaces
{
    public interface IIdentityRepository<T> where T : IdentityUser<int>
    {
        Task Add(T obj);
        Task Remove(int id);
        Task Update(int id, T obj);
        Task<T> GetById(int id);
        IQueryable<T> GetAllWithFilter(Expression<Func<T, bool>> filter);
        public Task<bool> RemoveByExpression(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAll();
    }
}
