using Jorje.TheWorld.Models.ViewModels;
using Jorje.TheWorld.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Web.Controllers
{
    public class AppController : Controller
    {
        public AppController(IMailService mailService)
        {
            MailService = mailService;     
        }

        public IMailService MailService;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel contact)
        {
            if (contact.Email.Contains("aol.com"))
            {
                ModelState.AddModelError("", "We don't support AOL addresses");
            }

            if (ModelState.IsValid)
            {
                //mail
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";
            }

            return View();

        }
    }
}
