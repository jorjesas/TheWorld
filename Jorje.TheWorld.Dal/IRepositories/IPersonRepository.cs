using Jorje.TheWorld.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.IRepositories
{
    public interface IPersonRepository : IAbstractRepository<Person>
    {
        Task<Person> GetPersonById(int personId);
    }
}
