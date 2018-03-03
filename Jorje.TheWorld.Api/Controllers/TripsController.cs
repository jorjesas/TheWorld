using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jorje.TheWorld.Bll.IBusiness;
using Jorje.TheWorld.Models;
using Microsoft.AspNetCore.JsonPatch;
using Jorje.TheWorld.Common.Services.Contract;
using Microsoft.Extensions.Logging;
using Jorje.TheWorld.Common.Helpers.Extensions;
using Jorje.TheWorld.Common.Helpers;
using Jorje.TheWorld.Api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Jorje.TheWorld.Api.Controllers
{
    [Authorize]
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ITripBus _tripBus;
        private IPersonBus _personBus;
        private IStopBus _stopBus;
        private ILogger<TripsController> _logger;
        private IUrlHelper _urlHelper;
        private ITypeHelperService _typeHelperService;

        public TripsController(ITripBus tripBus,
                               IStopBus stopBus,
                               IPersonBus personBus,
                               ILogger<TripsController> logger,
                               IUrlHelper urlHelper,
                               ITypeHelperService typeHelperService)
        {
            _tripBus = tripBus;
            _personBus = personBus;
            _stopBus = stopBus;
            _logger = logger;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetTrips")]
        public async Task<IActionResult> Get()
        {
            var trips = new List<TripDTO>() { new TripDTO() { Name = "Barcelona", StartDate = DateTime.Now },
                               new TripDTO() { Name = "Milano", StartDate = DateTime.Now }};

            return Ok(trips);
        }

        [HttpGet("{id}", Name = "GetTrip")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<TripDTO>(fields))
            {
                return BadRequest();
            }

            TripDTO trip = await _tripBus.GetTrip(id);

            if (trip == null)
            {
                return NotFound();
            }

            var links = CreateLinksForTrip(id, fields);

            var linkedResourceToReturn = trip.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody]TripForCreationDTO tripInput)
        {
            if (tripInput == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            TripDTO trip = null; /*await _tripBus.CreateTrip(tripInput);*/

            if (trip == null)
            {
                return StatusCode(500, "Insert failure");
                //throw new Exception("Insert failure");
            }

            return CreatedAtRoute("GetTrip", new { id = trip.Id }, trip);
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

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var trip = await _tripBus.UpdateTrip(id, tripInput);

            if (trip == null)
            {
                return StatusCode(500, "Update failure");
            }

            return NoContent();
        }

        [HttpPut("{id}/stops", Name = "UpdateTripStops")]
        public async Task<IActionResult> UpdateTripStops([FromRoute]int id, [FromQuery][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            TripDTO trip = await _tripBus.GetTrip(id);

            if (trip == null)
            {
                return NotFound();
            }

            var stops = await _stopBus.GetStops(ids);

            if (stops == null || stops.Count() == 0)
            {
                return NotFound();
            }

            if (trip.Stops == null)
            {
                trip.Stops = new List<TripStopDTO>();
            }

            foreach (var stop in stops)
            {
                trip.Stops.Add(new TripStopDTO() { Trip=trip, Stop=stop, OrderId=0, Comment="Nice city", ArrivalTime=DateTime.Now, DepartureTime=DateTime.Now});
            }

            trip.Price = 200;

            var result = await _tripBus.UpdateTrip(id, trip);

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

        private IEnumerable<LinkDTO> CreateLinksForTrip(int id, string fields)
        {
            var links = new List<LinkDTO>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDTO(_urlHelper.Link("GetTrip", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDTO(_urlHelper.Link("GetTrip", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDTO(_urlHelper.Link("DeleteTrip", new { id = id }),
              "delete_trip",
              "DELETE"));

            links.Add(
              new LinkDTO(_urlHelper.Link("CreateTrip", new { stopId = id }),
              "create_trip",
              "POST"));

            return links;
        }

        private IEnumerable<LinkDTO> CreateLinksForTrips(
            PaginationProperties paginationProperties,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDTO>();

            // self 
            links.Add(
               new LinkDTO(CreateTripsResourceUri(paginationProperties,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDTO(CreateTripsResourceUri(paginationProperties,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDTO(CreateTripsResourceUri(paginationProperties,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        private string CreateTripsResourceUri(
            PaginationProperties paginationProperties,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetStops",
                      new
                      {
                          fields = paginationProperties.Fields,
                          orderBy = paginationProperties.OrderBy,
                          searchQuery = paginationProperties.SearchQuery,
                          pageNumber = paginationProperties.PageNumber - 1,
                          pageSize = paginationProperties.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetStops",
                      new
                      {
                          fields = paginationProperties.Fields,
                          orderBy = paginationProperties.OrderBy,
                          searchQuery = paginationProperties.SearchQuery,
                          pageNumber = paginationProperties.PageNumber + 1,
                          pageSize = paginationProperties.PageSize
                      });

                default:
                    return _urlHelper.Link("GetStops",
                    new
                    {
                        fields = paginationProperties.Fields,
                        orderBy = paginationProperties.OrderBy,
                        searchQuery = paginationProperties.SearchQuery,
                        pageNumber = paginationProperties.PageNumber,
                        pageSize = paginationProperties.PageSize
                    });
            }
        }

        [HttpOptions]
        public IActionResult GetTripsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,PATCH,DELETE");
            return Ok();
        }

    }
}