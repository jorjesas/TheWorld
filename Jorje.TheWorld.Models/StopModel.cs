using System;
using System.Collections.Generic;
using System.Text;

namespace Jorje.TheWorld.Models
{
    public class StopModel
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Arrival { get; set; }
    }
}
