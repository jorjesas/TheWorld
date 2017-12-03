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
        public async Task<IActionResult> GetStops(/*[FromQuery(Name = "page")]int pageNumber = 1,
                                                  [FromQuery(Name ="size")]  int pageSize = 10*/
                                                  PaginationProperties paginationProperties)
        {
            if (!_typeHelperService.TypeHasProperties<StopDTO>(paginationProperties.Fields))
            {
                return BadRequest();
            }

            ResourceDataResult result = await _stopBus.GetStops(paginationProperties);

            if (result.StatusCode != 0)
            {
                return BadRequest();
            }

            PagedList<StopDTO> stops = result.Result as PagedList<StopDTO>;

            if (stops == null || stops.Count() == 0)
            {
                return NotFound();
            }

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

            return Ok(stops.ShapeData(paginationProperties.Fields));
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

            return Ok(stop.ShapeData(fields));
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

        [HttpDelete("{id}")]
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

            var stop = await _stopBus.UpdateStop(id, stopModel);

            if (stop == null)
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
    }
}