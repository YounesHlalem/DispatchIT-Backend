using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task Create(T entity);
        Task Update(T entity, Expression<Func<T, bool>> predicate);
        Task Delete(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
    }
}
