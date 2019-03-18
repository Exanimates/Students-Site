using Microsoft.EntityFrameworkCore;
using Students_Site.DAL.EF;
using Students_Site.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Students_Site.DAL.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        ApplicationContext _context;
        DbSet<TEntity> _dbSet;

        protected BaseRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
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

        public IEnumerable<TEntity> Find(Func<TEntity, Boolean> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public virtual void Delete(int id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            if (entityToDelete != null)
                _dbSet.Remove(entityToDelete);
        }
    }
}
