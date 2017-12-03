using Jorje.TheWorld.Domain;
using Jorje.TheWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.Mappers
{
    public class PersonMapper
    {
        public static PersonDTO ConvertEntityToModel(Person person)
        {
            return new PersonDTO()
            {
                Id = person.Id,
                DateOfBirth = person.DateOfBirth,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Description = person.PersonAdditionalData?.Description,
                PersonImagePath = person.PersonAdditionalData?.PersonImagePath
            };
        }

        public static Person ConvertModelToEntity(PersonDTO personModel)
        {
            PersonAdditionalData personAdditionalData = new PersonAdditionalData()
            {
                Description = personModel.Description,
                PersonImagePath = personModel.PersonImagePath
            };

            Person person = new Person()
            {
                FirstName = personModel.FirstName,
                LastName = personModel.LastName,
                DateOfBirth = personModel.DateOfBirth
            };

            person.PersonAdditionalData = personAdditionalData;
            personAdditionalData.Person = person;

            return person;
        }

        public static Person UpdateEntityFromModel(Person person, PersonDTO personModel)
        {
            if (personModel.FirstName != null)
            {
                person.FirstName = personModel.FirstName;
            }
            if (personModel.LastName != null)
            {
                person.LastName = personModel.LastName;
            }
            if (personModel.DateOfBirth != null)
            {
                person.DateOfBirth = personModel.DateOfBirth;
            }
            
            if (person.PersonAdditionalData == null)
            {
                person.PersonAdditionalData = new PersonAdditionalData();
            }
            if (personModel.Description != null)
            {
                person.PersonAdditionalData.Description = personModel.Description;
            }
            if (personModel.PersonImagePath != null)
            {
                person.PersonAdditionalData.PersonImagePath = personModel.PersonImagePath;
            }

            return person;

        }
    }
}
