using System;
using System.Collections.Generic;
using System.Text;

namespace Jorje.TheWorld.Models
{
    public class TripDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<TripStopDTO> Stops { get; set; }
        public ICollection<PersonDTO> Persons { get; set; }
    }
}
