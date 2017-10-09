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
    public class TripRepository : AbstractRepository<Trip>, ITripRepository
    {
        public TripRepository(WorldDBContext context) : base(context)
        {
        }

        public Task<Person> AddPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public Task<Stop> AddStop(Stop stop)
        {
            throw new NotImplementedException();
        }

        public async Task<Trip> CreateTrip(Trip trip)
        {
            if (trip != null)
            {
                Add(trip);
                await SaveChanges();
            }

            return trip;
        }

        public Task<Trip> DeleteTrip(int tripId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Stop>> GetStopsByTripId(int tripId)
        {
            throw new NotImplementedException();
        }

        public async Task<Trip> GetTripById(int tripId)
        {
            var query = GetAll().Where(m => m.Id == tripId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Trip> UpdateTrip(Trip trip)
        {
            if (trip != null)
            {
                Update(trip);
                await SaveChanges();
            }

            return trip;
        }
    }
}
