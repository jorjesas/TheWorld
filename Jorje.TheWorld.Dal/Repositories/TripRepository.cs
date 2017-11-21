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

        public async Task<IEnumerable<Person>> GetPersonsByTrip(int tripId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Stop>> GetStopsByTrip(int tripId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddStop(Stop stop)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateTrip(Trip trip)
        {
            return await InsertEntity(trip);
        }

        public async Task<bool> DeleteTrip(int tripId)
        {
            Trip trip = await GetTripById(tripId);

            return await DeleteEntity(trip);
        }

        public async Task<Trip> GetTripById(int tripId)
        {
            var query = GetAll().Where(m => m.Id == tripId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateTrip(Trip trip)
        {
            return await UpdateEntity(trip);
        }
    }
}
