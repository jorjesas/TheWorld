using System;
using System.Collections.Generic;
using System.Text;

namespace Jorje.TheWorld.Dal.DomainModels
{
    public class TripDto
    {
        public int TripId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public int StopId { get; set; }
        //public ICollection<StopDTO> Stops { get; set; }
    }
}
