using Jorje.TheWorld.Models.ViewModels;
using Jorje.TheWorld.Services.Contract;
using Microsoft.AspNetCore.Authorization;
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

        public AppController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Trips()
        {
            //try
            //{
            //    var data = _repository.GetAllTrips();

            //    return View(data);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Failed to get trips in Index page: {ex.Message}");
            //    return Redirect("/error");
            //}

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            //if (model.Email.Contains("aol.com"))
            //{
            //    ModelState.AddModelError("", "We don't support AOL addresses");
            //}

            //if (ModelState.IsValid)
            //{
            //    _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From TheWorld", model.Message);

            //    ModelState.Clear();

            //    ViewBag.UserMessage = "Message Sent";
            //}

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

    }
}
