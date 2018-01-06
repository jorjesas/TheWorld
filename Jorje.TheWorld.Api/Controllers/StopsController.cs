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
using Jorje.TheWorld.Api.Helpers;
using Microsoft.Extensions.Logging;
using Jorje.TheWorld.Common.Helpers;
using Jorje.TheWorld.Common.Helpers.Pagination;
using Jorje.TheWorld.Common.Helpers.Serialization;
using Jorje.TheWorld.Common.Helpers.Sorting;
using Jorje.TheWorld.Dal.Sort;
using Jorje.TheWorld.Bll.Containers;
using Jorje.TheWorld.Common.Helpers.Extensions;
using Jorje.TheWorld.Common.Services.Contract;

namespace Jorje.TheWorld.Api.Controllers
{
    //[Authorize]
    [Route("api/stops")]
    public class StopsController : Controller
    {
        private IStopBus _stopBus;
        private ILogger<StopsController> _logger;
        private IUrlHelper _urlHelper;
        private ITypeHelperService _typeHelperService;

        //const int MaxStopPageSize = 20;

        public StopsController( IStopBus stopBus, 
                                ILogger<StopsController> logger, 
                                IUrlHelper urlHelper, 
                                ITypeHelperService typeHelperService)
        {
            _stopBus = stopBus;
            _logger = logger;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetStops")]
        [HttpHead]
        public async Task<IActionResult> GetStops(
            PaginationProperties paginationProperties,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_typeHelperService.TypeHasProperties<StopDTO>(paginationProperties.Fields))
            {
                return BadRequest();
            }

            ResourceDataResult result = await _stopBus.GetStops(paginationProperties);

            if (result.StatusCode != 0)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            PagedList<StopDTO> stops = result.Result as PagedList<StopDTO>;

            if (stops == null || stops.Count() == 0)
            {
                return NotFound();
            }

            if (mediaType == "application/hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = stops.TotalCount,
                    pageSize = stops.PageSize,
                    currentPage = stops.CurrentPage,
                    totalPages = stops.TotalPages,
                };

                Response.Headers.Add("X-Pagination", Serializer.JsonSerialize(paginationMetadata));

                var links = CreateLinksForStops(paginationProperties,
                    stops.HasNext, stops.HasPrevious);

                var shapedStops = stops.ShapeEnumerableData(paginationProperties.Fields);

                var shapedStopsWithLinks = shapedStops.Select(stop =>
                {
                    var stopAsDictionary = stop as IDictionary<string, object>;
                    var stopLinks = CreateLinksForStop(
                        (int)stopAsDictionary["Id"], paginationProperties.Fields);

                    stopAsDictionary.Add("links", stopLinks);

                    return stopAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedStopsWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = stops.HasPrevious ? CreateStopsResourceUri(paginationProperties, ResourceUriType.PreviousPage) : null;
                var nextPageLink = stops.HasNext ? CreateStopsResourceUri(paginationProperties, ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    totalCount = stops.TotalCount,
                    pageSize = stops.PageSize,
                    currentPage = stops.CurrentPage,
                    totalPages = stops.TotalPages,
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink
                };

                Response.Headers.Add("X-Pagination", Serializer.JsonSerialize(paginationMetadata));

                return Ok(stops.ShapeEnumerableData(paginationProperties.Fields));
            }
        }

        [HttpGet("{id}", Name = "GetStop")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<StopDTO>(fields))
            {
                return BadRequest();
            }

            StopDTO stop = await _stopBus.GetStop(id);

            if (stop == null)
            {
                return NotFound();
            }

            var links = CreateLinksForStop(id, fields);

            var linkedResourceToReturn = stop.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStop([FromBody]StopForCreationDTO stopInput)
        {
            if (stopInput == null)
            {
                return BadRequest();
            }

            if (stopInput.Name.Equals(stopInput.Description, StringComparison.CurrentCultureIgnoreCase))
            {
                ModelState.AddModelError(nameof(StopForCreationDTO), "Stop name and description must be different!");
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            StopDTO stop = await _stopBus.CreateStop(stopInput);

            if (stop == null)
            {
                return StatusCode(500, "Insert failure");
            }

            return CreatedAtRoute("GetStop", new { id = stop.Id}, stop);
        }

        [HttpDelete("{id}", Name = "DeleteStop")]
        public async Task<IActionResult> DeleteStop(int id)
        {
            StopDTO stop = await _stopBus.GetStop(id);

            if (stop == null)
            {
                return NotFound();
            }

            if (!await _stopBus.DeleteStop(stop))
            {
                return StatusCode(500, "Delete failure");
            }

            _logger.LogInformation(1, $"Stop with id={id}and name={stop.Name} deleted");
            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateStop")]
        public async Task<IActionResult> UpdateStop(int id, [FromBody]StopForUpdateDTO stopInput)
        {
            if (stopInput == null)
            {
                return BadRequest();
            }

            if (stopInput.Latitude > 360 || stopInput.Longitude > 360)
            {
                ModelState.AddModelError(nameof(StopForUpdateDTO), "Latitude and longitude can be specified up to 360 degrees");
            }

            if (string.IsNullOrWhiteSpace(stopInput.Name))
            {
                ModelState.AddModelError(nameof(StopForUpdateDTO), "Empty name is invalid!");
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var stopResult = await _stopBus.UpdateStop(id, stopInput);

            if (stopResult == null || stopResult.Result == null)
            {
                return StatusCode(500, "Update failure");
            }

            StopDTO stop = stopResult.Result as StopDTO;

            if (stopResult.StatusCode == 201)
            {
                return CreatedAtRoute("GetStop",
                    new { id = stop.Id },
                    stop);
            }

            return NoContent();
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateStop")]
        public async Task<IActionResult> PartiallyUpdateStop(int id, [FromBody]JsonPatchDocument<StopForUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            StopForUpdateDTO stopModel = await _stopBus.GetUpdateModelForStop(id);

            if (stopModel == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(stopModel, ModelState);

            TryValidateModel(stopModel);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var stopResult = await _stopBus.UpdateStop(id, stopModel);

            if (stopResult == null || stopResult.Result == null)
            {
                return StatusCode(500, "Update failure");
            }

            return NoContent();
        }

        private string CreateStopsResourceUri(
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

        private IEnumerable<LinkDTO> CreateLinksForStop(int id, string fields)
        {
            var links = new List<LinkDTO>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDTO(_urlHelper.Link("GetStop", new { id = id }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDTO(_urlHelper.Link("GetStop", new { id = id, fields = fields }),
                  "self",
                  "GET"));
            }

            links.Add(
              new LinkDTO(_urlHelper.Link("DeleteStop", new { id = id }),
              "delete_stop",
              "DELETE"));

            links.Add(
              new LinkDTO(_urlHelper.Link("CreateStop", new { stopId = id }),
              "create_stop",
              "POST"));

            links.Add(
               new LinkDTO(_urlHelper.Link("GetTripsForStop", new { stopId = id }),
               "trips_for_stop",
               "GET"));

            return links;
        }

        private IEnumerable<LinkDTO> CreateLinksForStops(
            PaginationProperties paginationProperties,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDTO>();

            // self 
            links.Add(
               new LinkDTO(CreateStopsResourceUri(paginationProperties,
               ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDTO(CreateStopsResourceUri(paginationProperties,
                  ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDTO(CreateStopsResourceUri(paginationProperties,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }

        [HttpOptions]
        public IActionResult GetStopsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,PATCH,DELETE");
            return Ok();
        }
    }
}