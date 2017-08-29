using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Domain
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public PersonAdditionalData PersonAdditionalData { get; set; }
        public ICollection<PersonTrip> PersonTrips { get; set; }

    }
}
