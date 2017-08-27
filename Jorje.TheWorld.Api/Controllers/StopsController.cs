using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jorje.TheWorld.Bll.IBusiness;

namespace Jorje.TheWorld.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Stops")]
    public class StopsController : Controller
    {
        private IStopBus _stopBus;

        public StopsController(IStopBus stopBus)
        {
            _stopBus = stopBus;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int stopId)
        {
            return Ok(await _stopBus.GetStop(stopId));
        }
    }
}