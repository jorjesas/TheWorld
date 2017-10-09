using Jorje.TheWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Services.Contract
{
    public interface IMailService
    {
        void SendMail(EmailDTO email);
    }
}
