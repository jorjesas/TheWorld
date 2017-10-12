using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jorje.TheWorld.Bll.IBusiness;
using Microsoft.AspNetCore.Authorization;
using Jorje.TheWorld.Models;
using Microsoft.AspNetCore.JsonPatch;
using Jorje.TheWorld.Bll.Mappers;

namespace Jorje.TheWorld.Api.Controllers
{
    //[Authorize]
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

        [HttpGet("{id}", Name = "GetStop")]
        public async Task<IActionResult> Get(int id)
        {
            StopDTO stop = await _stopBus.GetStop(id);

            if (stop == null)
            {
                return NotFound();
            }

            return Ok(stop);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStop([FromBody]StopForCreationDTO stopInput)
        {
            if (stopInput == null)
            {
                return BadRequest();
            }

            StopDTO stop = await _stopBus.CreateStop(stopInput);

            if (stop == null)
            {
                return StatusCode(500, "Insert failure");
                //throw new Exception("Insert failure");
            }

            return CreatedAtRoute("GetStop", new { id = stop.Id}, stop);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStop(int id)
        {
            StopDTO stop = await _stopBus.GetStop(id);

            if (stop == null)
            {
                return NotFound();
            }

            if (await _stopBus.DeleteStop(stop))
            {
                return StatusCode(500, "Delete failure");
                //throw new Exception("Insert failure");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStop(int id, [FromBody]StopForUpdateDTO stopInput)
        {
            if (stopInput == null)
            {
                return BadRequest();
            }

            var stop = await _stopBus.UpdateStop(id, stopInput);

            if (stop == null)
            {
                return StatusCode(500, "Update failure");
                //throw new Exception("Insert failure");
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateStop(int id, [FromBody]JsonPatchDocument<StopForUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            StopForUpdateDTO updatedStop = new StopForUpdateDTO();
            patchDoc.ApplyTo(updatedStop);

            var stop = await _stopBus.UpdateStop(id, updatedStop);

            if (stop == null)
            {
                return StatusCode(500, "Update failure");
                //throw new Exception("Insert failure");
            }

            return NoContent();
        }
    }
}