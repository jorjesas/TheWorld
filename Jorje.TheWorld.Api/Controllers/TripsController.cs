using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jorje.TheWorld.Bll.IBusiness;

namespace Jorje.TheWorld.Api.Controllers
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ITripBus _tripBus;

        public TripsController(ITripBus tripBus)
        {
            _tripBus = tripBus;
        }



    }
}