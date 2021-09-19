using IT.Netic.DispatchIt.Web.Backend.Domain.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Domain.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly IDbContextCreator DbContextContextCreator; 

        protected BaseRepository(IDbContextCreator dbContextContextCreator)
        {
            DbContextContextCreator = dbContextContextCreator;
        }

        public async Task Create(T entity)
        {
            using (var dbContext = DbContextContextCreator.CreateDispatchItContext())
            {
                await dbContext.Set<T>().AddAsync(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(Expression<Func<T, bool>> predicate)
        {
            using (var dbContext = DbContextContextCreator.CreateDispatchItContext())
            {
                var entityToDelete = await dbContext.Set<T>().FirstOrDefaultAsync(predicate);
                if (entityToDelete != null)
                {
                    dbContext.Set<T>().Remove(entityToDelete);
                }

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            using (var dbContext = DbContextContextCreator.CreateDispatchItContext())
            {
                IQueryable<T> qry = dbContext.Set<T>();

                qry = includeProperties.Aggregate(qry, (current, includeProperty) => current.Include(includeProperty));

                return await qry.Where(predicate).ToListAsync();
            }
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            using (var dbContext = DbContextContextCreator.CreateDispatchItContext())
            {
                IQueryable<T> qry = dbContext.Set<T>();

                qry = includeProperties.Aggregate(qry, (current, includeProperty) => current.Include(includeProperty));

                return await qry.FirstOrDefaultAsync(predicate);
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            using (var dbContext = DbContextContextCreator.CreateDispatchItContext())
            {
                return await dbContext.Set<T>().ToListAsync();
            }
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            using (var dbContext = DbContextContextCreator.CreateDispatchItContext())
            {
                var result = dbContext.Set<T>().Where(predicate);
                if (result == null)
                    return new List<T>();
                return await result.ToListAsync();
            }
        }

        public async Task Update(T entity, Expression<Func<T, bool>> predicate)
        {
            using (var dbContext = DbContextContextCreator.CreateDispatchItContext())
            {
                var editedEntity = await dbContext.Set<T>().FirstOrDefaultAsync(predicate);
                if (editedEntity != null)
                {
                    editedEntity = entity;
                    dbContext.Set<T>().Update(editedEntity);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
