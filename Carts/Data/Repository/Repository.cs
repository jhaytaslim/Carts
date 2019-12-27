using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Carts.Data.Repository.Interface;
using System.Threading.Tasks;

namespace Carts.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _entities;
        protected readonly ILogger _log;

        public Repository(DbContext context, ILogger<dynamic> log)
        {
            _context = context;
            _entities = context.Set<TEntity>();
            _log = log;
        }

        public virtual TEntity Add(TEntity entity)
        {
            try
            {
                _entities.Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message + " : " + ex.InnerException + " : " + ex.StackTrace);
                return null;
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                await _entities.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message + " : " + ex.InnerException + " : " + ex.StackTrace);
                return null;
            }
        }

        public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _entities.AddRange(entities);
                _context.SaveChanges();
                return entities;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message + " : " + ex.InnerException + " : " + ex.StackTrace);
                return null;
            }
        }


        public virtual TEntity Update(TEntity entity)
        {
            try
            {
                _entities.Update(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message + " : " + ex.InnerException + " : " + ex.StackTrace);
                return null;
            }
        }

        public virtual IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _entities.UpdateRange(entities);
                _context.SaveChanges();
                return entities;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message + " : " + ex.InnerException + " : " + ex.StackTrace);
                return null;
            }
        }



        public virtual bool Remove(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _entities.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message + " : " + ex.InnerException + " : " + ex.StackTrace);
                return false;
            }
        }

        public virtual bool RemoveRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _entities.RemoveRange(entities);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message + " : " + ex.InnerException + " : " + ex.StackTrace);
                return false;
            }
        }


        public virtual int Count()
        {
            return _entities.Count();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate).Count();
        }


        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate).ToList();
        }

        public virtual TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.FirstOrDefault(predicate);
        }

        public virtual TEntity Get(int id)
        {
            return _entities.Find(id);
        }

        public virtual TEntity Get(string id)
        {
            return _entities.Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
        }
    }
}
