using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jorje.TheWorld.Bll.IBusiness;
using Microsoft.AspNetCore.Authorization;
using Jorje.TheWorld.Models;

namespace Jorje.TheWorld.Api.Controllers
{
    [Authorize]
    [Route("api/stops")]
    public class StopsController : Controller
    {
        private IStopBus _stopBus;
        private ITripBus _tripBus;

        public StopsController(IStopBus stopBus, ITripBus tripBus)
        {
            _stopBus = stopBus;
            _tripBus = tripBus;
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetStopsByTrip(int tripId)
        {
            IEnumerable<StopDTO> stops = await _tripBus.GetStopsByTrip(tripId);

            return Ok(null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStop(int id)
        {
            StopDTO stop = await _stopBus.GetStop(id);

            if (stop == null)
            {
                return NotFound();
            }

            return Ok(stop);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStop([FromBody]StopDTO stop)
        {
            return Ok(await _stopBus.CreateStop(stop));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStop(int stopId)
        {
            return Ok(await _stopBus.GetStop(stopId));
        }
    }
}