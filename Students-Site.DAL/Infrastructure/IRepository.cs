using System;
using System.Linq;
using System.Linq.Expressions;

namespace Students_Site.DAL.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Get(int id);
        IQueryable<T> Find(Expression<Func<T, bool>> where);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}