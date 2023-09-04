using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected AppDbContext context;
        protected DbSet<TEntity> dbSet;

        public BaseRepository(AppDbContext context)
        {
            this.context = (AppDbContext)context;
            dbSet = ((DbContext)context).Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query);
            return query;

        }

        public virtual IEnumerable<TEntity> Get(Func<TEntity, bool> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }


            return query.Where(filter);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet;
        }
        public virtual IEnumerable<TEntity> Get(string includeProperties)
        {
            IQueryable<TEntity> query = dbSet;
            foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }


        public async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }


        public virtual void Delete(object id)
        {

            TEntity entityToDelete = dbSet.Find(id);
            _ = dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {

            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;

        }

        public TEntity Add(TEntity obj)
        {
            dbSet.Add(obj);
            return obj;
        }

        public virtual Int32 Count()
        {
            return dbSet.Count();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
