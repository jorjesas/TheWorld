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
            return GenericMapper.ConvertEntityToModel<Person, PersonDTO>(person);
        }

        public static Person ConvertModelToEntity(PersonDTO personModel)
        {
            return GenericMapper.ConvertModelToEntity<Person, PersonDTO>(personModel);
        }

        public static Person UpdateEntityFromModel(Person person, PersonDTO personModel)
        {
            return GenericMapper.UpdateEntityFromModel<Person, PersonDTO>(person, personModel);
        }
    }
}
