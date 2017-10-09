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

        public async Task<Person> CreatePerson(Person person)
        {
            if (person != null)
            {
                Add(person);
                await SaveChanges();
            }

            return person;
        }

        public async Task<Person> DeletePerson(int personId)
        {
            var query = GetAll().Where(m => m.Id == personId);

            Person person = await query.FirstOrDefaultAsync();

            if (person != null)
            {
                Delete(person);
                await SaveChanges();
            }

            return person;
        }

        public async Task<Person> GetPersonByIdAsync(int personId)
        {
            var query = GetAll().Where(m => m.Id == personId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            if (person != null)
            {
                Update(person);
                await SaveChanges();
            }

            return person;
        }
    }
}
