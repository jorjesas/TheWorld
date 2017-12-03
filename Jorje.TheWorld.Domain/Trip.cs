using System;
using System.Collections.Generic;
using System.Text;

namespace Jorje.TheWorld.Domain
{
    public class Trip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<TripStop> TripStops { get; set; }
        public ICollection<PersonTrip> PersonTrips { get; set; }
    }
}
