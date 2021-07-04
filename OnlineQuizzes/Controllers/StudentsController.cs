using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace OnlineQuizzes.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentsController : Controller
    {
        private ApplicationDbContext db;

        public StudentsController()
        {
            db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }


        // GET: Students
        public ActionResult Index()
        {
            var studentId = User.Identity.GetUserId();

            //Send AvailableQuizzes To Student Page
            var AvailableQuizzes = db.QuizPermissions.Include(c => c.Quiz).Where(s => s.Id == studentId)
                .ToList();

            return View(AvailableQuizzes);
        }
    }
}