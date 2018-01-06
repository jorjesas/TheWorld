using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.Containers
{
    public class ResourceDataResult
    {
        public ResourceDataResult()
        {
            StatusCode = 200;
            ErrorMessage = string.Empty;
            Result = null;
        }

        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}
