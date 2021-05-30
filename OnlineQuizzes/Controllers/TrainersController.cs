using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.Controllers
{
    public class TrainersController : Controller
    {
        private ApplicationDbContext db;

        public TrainersController()
        {
            db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        // GET: Trainers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateQuizPage()
        {
            return View("CreateQuiz");
        }

        [HttpPost]
        public ActionResult CreateNewQuiz(Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateQuiz", quiz);
            }

            db.Quizzes.Add(quiz);
            db.SaveChanges();

            return RedirectToAction("DisplayQuestionsPage", "Trainers");
        }

        public ActionResult DisplayQuestionsPage()
        {
            return View("AddQuestions");
        }
    }
}