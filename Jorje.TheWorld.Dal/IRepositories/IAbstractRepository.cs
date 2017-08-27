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

        Task SaveChanges();

        void Add(T entity);

        T Update(T entity);

        void Delete(T entityToDelete);

        Task ExecuteProcedure(String procedureCommand, params SqlParameter[] sqlParams);
    }
}
