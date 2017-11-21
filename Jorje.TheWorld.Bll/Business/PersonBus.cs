using Jorje.TheWorld.Bll.IBusiness;
using Jorje.TheWorld.Bll.Mappers;
using Jorje.TheWorld.Dal.IRepositories;
using Jorje.TheWorld.Domain;
using Jorje.TheWorld.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.Business
{
    public class PersonBus : IPersonBus
    {
        IPersonRepository _personRepo;

        #region ctor
        public PersonBus(IPersonRepository PersonRepo)
        {
            _personRepo = PersonRepo;
        }
        #endregion

        public async Task<PersonDTO> GetPerson(int personId)
        {
            var person = await _personRepo.GetPersonById(personId);


            if (person != null)
            {
                return PersonMapper.ConvertEntityToModel(person);
            }

            return null;
        }

        public async Task<PersonDTO> CreatePerson(PersonDTO personModel)
        {
            Person person = PersonMapper.ConvertModelToEntity(personModel);

            if (await _personRepo.CreatePerson(person))
            {
                return PersonMapper.ConvertEntityToModel(person);
            }

            return null;
        }

        public async Task<bool> DeletePerson(PersonDTO personModel)
        {
            if (personModel != null && await _personRepo.DeletePerson(personModel.Id))
            {
                return true;
            }

            return false;
        }

        public async Task<PersonDTO> UpdatePerson(int personId, PersonDTO personModel)
        {
            var person = await _personRepo.GetPersonById(personId);

            //upsert
            if (person == null)
            {
                person = new Person() { Id = personId };
            }

            person = PersonMapper.UpdateEntityFromModel(person, personModel);

            if (person != null && await _personRepo.UpdatePerson(person))
            {
                return PersonMapper.ConvertEntityToModel(person);
            }

            return null;
        }

        public async Task<PersonDTO> PartialUpdatePerson(int personId, JsonPatchDocument<PersonDTO> patchDoc)
        {
            var person = await _personRepo.GetPersonById(personId);

            if (person == null)
            {
                return null;
            }

            PersonDTO updatedPerson = PersonMapper.ConvertEntityToModel(person);

            patchDoc.ApplyTo(updatedPerson);

            person = PersonMapper.UpdateEntityFromModel(person, updatedPerson);

            if (person != null && await _personRepo.UpdatePerson(person))
            {
                return PersonMapper.ConvertEntityToModel(person);
            }

            return null;
        }

    }
}
