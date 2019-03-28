using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Students_Site.DAL.EF;
using System.Linq.Expressions;
using Students_Site.DAL.Infrastructure;

namespace Students_Site.DAL.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected RepositoryBase(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public virtual TEntity Get(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.Where(where);
        }

        public virtual void Delete(int id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            if (entityToDelete != null)
                _dbSet.Remove(entityToDelete);
        }
    }
}