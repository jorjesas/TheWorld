using Jorje.TheWorld.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.IBusiness
{
    public interface IPersonBus
    {
        Task<PersonDTO> GetPerson(int personId);
        Task<PersonDTO> CreatePerson(PersonDTO personModel);
        Task<bool> DeletePerson(PersonDTO personModel);
        Task<PersonDTO> UpdatePerson(int personId, PersonDTO personModel);
        Task<PersonDTO> PartialUpdatePerson(int personId, JsonPatchDocument<PersonDTO> patchDoc);
    }
}
