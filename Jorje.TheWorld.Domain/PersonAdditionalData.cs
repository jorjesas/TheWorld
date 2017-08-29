using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Domain
{
    public class PersonAdditionalData
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string PersonImagePath { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
