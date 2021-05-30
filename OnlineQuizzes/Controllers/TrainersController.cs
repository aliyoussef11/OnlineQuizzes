using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineQuizzes.Controllers
{
    public class TrainersController : Controller
    {
        // GET: Trainers
        public ActionResult Index()
        {
            return View();
        }
    }
}