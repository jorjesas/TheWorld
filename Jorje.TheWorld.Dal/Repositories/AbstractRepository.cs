using Jorje.TheWorld.Dal.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.Repositories
{
    public class AbstractRepository<T> : IAbstractRepository<T> where T : class
    {
        protected DbContext _dbContext;

        protected DbSet<T> _dbSet;

        public AbstractRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public async Task<bool> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync(true) > 0;
        }

        public async Task<bool> InsertEntity(T entity)
        {
            bool isSaved = false;

            if (entity != null)
            {
                Add(entity);
                isSaved = await SaveChanges();
            }

            return isSaved;
        }

        public async Task<bool> UpdateEntity(T entity)
        {
            bool isSaved = false;

            //if (entity != null)
            //{
            //    Update(entity);
            //    isSaved = await SaveChanges();
            //}

            if (entity != null && _dbContext.Entry(entity).State == EntityState.Modified)
            {
                isSaved = await SaveChanges();
            }

            return isSaved;
        }

        public async Task<bool> DeleteEntity(T entity)
        {
            bool isSaved = false;

            if (entity != null)
            {
                Delete(entity);
                isSaved = await SaveChanges();
            }

            return isSaved;
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual T Update(T entity)
        {
            return _dbSet.Update(entity).Entity;
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }


        public async Task ExecuteProcedure(String procedureCommand, params SqlParameter[] sqlParams)
        {
            await _dbContext.Database.ExecuteSqlCommandAsync(procedureCommand, CancellationToken.None, sqlParams);
        }
    }
}
