using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Carts.Data.Repository.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);
        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);

        bool Remove(TEntity entity);
        bool RemoveRange(IEnumerable<TEntity> entities);

        int Count();
        int Count(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        TEntity Get(int id);
        TEntity Get(string id);

        IEnumerable<TEntity> GetAll();
    }
}
