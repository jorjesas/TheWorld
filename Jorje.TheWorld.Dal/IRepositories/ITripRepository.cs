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
        Task<IEnumerable<Stop>> GetStopsByTrip(int tripId);
        Task<IEnumerable<Person>> GetPersonsByTrip(int tripId);
        Task<bool> CreateTrip(Trip trip);
        Task<bool> DeleteTrip(int tripId);
        Task<bool> UpdateTrip(Trip trip);
        Task<bool> AddStop(Stop stop);
        Task<bool> AddPerson(Person person);
    }
}
