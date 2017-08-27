using System;
using System.Collections.Generic;
using System.Text;

namespace Jorje.TheWorld.Domain
{
    public class Stop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Order { get; set; }
        public DateTime Arrival { get; set; }
    }
}
