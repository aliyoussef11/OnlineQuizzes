using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OnlineQuizzes.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
         
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Trainer"))
                {
                    return RedirectToAction("Index", "Trainers");
                }
                else if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index", "Students");
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}