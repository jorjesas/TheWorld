using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.IRepositories
{
    public interface IAbstractRepository<T>
    {
        IQueryable<T> GetAll();

        Task<bool> InsertEntity(T entity);

        Task<bool> UpdateEntity(T entity);

        Task<bool> DeleteEntity(T entity);

        Task<bool> SaveChanges();

        void Add(T entity);

        T Update(T entity);

        void Delete(T entityToDelete);

        Task ExecuteProcedure(String procedureCommand, params SqlParameter[] sqlParams);
    }
}
