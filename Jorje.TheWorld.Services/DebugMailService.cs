using Jorje.TheWorld.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jorje.TheWorld.Models;

namespace Jorje.TheWorld.Services
{
    public class DebugMailService : IMailService
    {
        void IMailService.SendMail(EmailDTO email)
        {
            //throw new NotImplementedException();
        }
    }
}
