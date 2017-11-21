using Jorje.TheWorld.Dal.Context;
using Jorje.TheWorld.Dal.IRepositories;
using Jorje.TheWorld.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.Repositories
{
    public class PersonRepository : AbstractRepository<Person>, IPersonRepository
    {
        public PersonRepository(WorldDBContext context) : base(context)
        {
        }

        public async Task<Person> GetPersonById(int personId)
        {
            var query = GetAll().Where(m => m.Id == personId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> CreatePerson(Person person)
        {
            return await InsertEntity(person);
        }

        public async Task<bool> DeletePerson(int personId)
        {
            Person person = await GetPersonById(personId);

            return await DeleteEntity(person);
        }

        public async Task<bool> UpdatePerson(Person person)
        {
            return await UpdateEntity(person);
        }
    }
}
