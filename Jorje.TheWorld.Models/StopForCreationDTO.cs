using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Models
{
    public class StopForCreationDTO
    {
        [Required(ErrorMessage = "Stop Name is mandatory")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Stop Description is mandatory")]
        [MaxLength(500)]
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
