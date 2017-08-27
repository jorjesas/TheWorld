using Jorje.TheWorld.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.IBusiness
{
    public interface IStopBus
    {
        Task<StopModel> GetStop(int stopId);
        Task<StopModel> GetStopsByTrip(int tripId);


    }
}
