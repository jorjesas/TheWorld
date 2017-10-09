using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Models
{
    public class TripStopDTO
    {

        public int OrderId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public string Comment { get; set; }
        public TripDTO Trip { get; set; }
        public StopDTO Stop { get; set; }
    }
}
