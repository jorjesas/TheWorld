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
    [Produces("application/json")]
    [Route("api/Stops")]
    public class StopsController : Controller
    {
        private IStopBus _stopBus;

        public StopsController(IStopBus stopBus)
        {
            _stopBus = stopBus;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStop(int id)
        {
            StopModel stop = await _stopBus.GetStop(id);

            if (stop == null)
            {
                return NotFound();
            }

            return Ok(stop);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStop([FromBody]StopModel stop)
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