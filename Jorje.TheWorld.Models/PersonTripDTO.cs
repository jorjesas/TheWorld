using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Models
{
    public class PersonTripDTO
    {
        public int TripId { get; set; }
        public TripDTO Trip { get; set; }
        public int PersonId { get; set; }
        public PersonDTO Person { get; set; }
        public string Comment { get; set; }
    }
}
