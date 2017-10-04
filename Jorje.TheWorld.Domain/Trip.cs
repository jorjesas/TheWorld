using System;
using System.Collections.Generic;
using System.Text;

namespace Jorje.TheWorld.Domain
{
    public class Trip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public ICollection<Stop> Stops { get; set; }
        public ICollection<PersonTrip> PersonTrips { get; set; }
    }
}
