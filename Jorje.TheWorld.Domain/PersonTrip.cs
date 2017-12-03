using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Domain
{
    public class PersonTrip
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public string Comment { get; set; }
    }
}
