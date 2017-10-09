using Jorje.TheWorld.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.IRepositories
{
    public interface ITripRepository: IAbstractRepository<Trip>
    {
        Task<Trip> GetTripById(int tripId);
        Task<IEnumerable<Stop>> GetStopsByTripId(int tripId);
        Task<Trip> CreateTrip(Trip trip);
        Task<Trip> DeleteTrip(int tripId);
        Task<Trip> UpdateTrip(Trip trip);
        Task<Stop> AddStop(Stop stop);
        Task<Person> AddPerson(Person person);
    }
}
