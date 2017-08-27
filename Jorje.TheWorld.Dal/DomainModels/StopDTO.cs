using System;
using System.Collections.Generic;
using System.Text;

namespace Jorje.TheWorld.Dal.DomainModels
{
    public class StopDTO
    {
        public int StopId { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Order { get; set; }
        public DateTime Arrival { get; set; }
    }
}
