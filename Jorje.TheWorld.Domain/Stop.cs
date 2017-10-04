using System;
using System.Collections.Generic;
using System.Text;

namespace Jorje.TheWorld.Domain
{
    public class Stop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Order { get; set; }
        public int TripId { get; set; }
        public DateTime Arrival { get; set; }
    }
}
