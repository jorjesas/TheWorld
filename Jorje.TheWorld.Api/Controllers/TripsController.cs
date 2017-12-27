using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jorje.TheWorld.Bll.IBusiness;
using Jorje.TheWorld.Models;
using Microsoft.AspNetCore.JsonPatch;

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

        [HttpGet(Name = "GetTrips")]
        public async Task<IActionResult> Get()
        {
            var trips = new List<TripDTO>() { new TripDTO() { Name = "Barcelona", StartDate = DateTime.Now },
                               new TripDTO() { Name = "Milano", StartDate = DateTime.Now }};

            return Ok(trips);
        }

        [HttpGet("{id}", Name = "GetTrip")]
        public async Task<IActionResult> Get(int id)
        {
            TripDTO trip = await _tripBus.GetTrip(id);

            if (trip == null)
            {
                return NotFound();
            }

            return Ok(trip);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody]TripDTO tripInput)
        {
            if (tripInput == null)
            {
                return BadRequest();
            }

            TripDTO trip = await _tripBus.CreateTrip(tripInput);

            if (trip == null)
            {
                return StatusCode(500, "Insert failure");
                //throw new Exception("Insert failure");
            }

            return CreatedAtRoute("GetTrips", new { id = trip.Id }, trip);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            TripDTO trip = await _tripBus.GetTrip(id);

            if (trip == null)
            {
                return NotFound();
            }

            if (!await _tripBus.DeleteTrip(trip))
            {
                return StatusCode(500, "Delete failure");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(int id, [FromBody]TripDTO tripInput)
        {
            if (tripInput == null)
            {
                return BadRequest();
            }

            var trip = await _tripBus.UpdateTrip(id, tripInput);

            if (trip == null)
            {
                return StatusCode(500, "Update failure");
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateTrip(int id, [FromBody]JsonPatchDocument<TripDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var trip = await _tripBus.PartialUpdateTrip(id, patchDoc);

            if (trip == null)
            {
                return StatusCode(500, "Update failure");
            }

            return NoContent();
        }



    }
}