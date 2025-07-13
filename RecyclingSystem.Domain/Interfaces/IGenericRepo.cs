using RecyclingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace RecyclingSystem.Domain.Interfaces
{
    public interface IGenericRepo<T, TId> where T : BaseModel<TId>
    {
        Task Add(T obj);
        Task Remove(TId id);
        Task Update(TId id, T obj);
        Task<T> GetById(TId id);
        IQueryable<T> GetAllWithFilter(Expression<Func<T, bool>> filter);
        Task<T> GetSpecificWithFilter(Expression<Func<T, bool>> filter);
        public Task<bool> RemoveByExpression(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAll();
    }
}

