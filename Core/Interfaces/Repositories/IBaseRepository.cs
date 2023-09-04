using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        IEnumerable<TEntity> Get(Func<TEntity, bool> filter, string includeProperties = "");
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        IEnumerable<TEntity> GetAll();
        Task<TEntity> GetByIDAsync(object id);
        TEntity Add(TEntity entity);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        Int32 Count();
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
