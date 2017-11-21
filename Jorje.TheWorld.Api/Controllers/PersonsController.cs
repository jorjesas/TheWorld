using Jorje.TheWorld.Bll.IBusiness;
using Jorje.TheWorld.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Api.Controllers
{
    [Route("api/persons")]
    public class PersonsController : Controller
    {
        private IPersonBus _personBus;

        public PersonsController(IPersonBus personBus)
        {
            _personBus = personBus;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            PersonDTO person = await _personBus.GetPerson(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody]PersonDTO personIput)
        {
            if (personIput == null)
            {
                return BadRequest();
            }

            PersonDTO person = await _personBus.CreatePerson(personIput);

            if (person == null)
            {
                return StatusCode(500, "Insert failure");
                //throw new Exception("Insert failure");
            }

            return CreatedAtRoute("", new { id = person.Id }, person);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            PersonDTO person = await _personBus.GetPerson(id);

            if (person == null)
            {
                return NotFound();
            }

            if (!await _personBus.DeletePerson(person))
            {
                return StatusCode(500, "Delete failure");
                //throw new Exception("Insert failure");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody]PersonDTO personInput)
        {
            if (personInput == null)
            {
                return BadRequest();
            }

            var stop = await _personBus.UpdatePerson(id, personInput);

            if (stop == null)
            {
                return StatusCode(500, "Update failure");
                //throw new Exception("Insert failure");
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdatePerson(int id, [FromBody]JsonPatchDocument<PersonDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var person = await _personBus.PartialUpdatePerson(id, patchDoc);

            if (person == null)
            {
                return StatusCode(500, "Update failure");
                //throw new Exception("Insert failure");
            }

            return NoContent();
        }

    }
}
