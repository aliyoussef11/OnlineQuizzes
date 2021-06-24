using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineQuizzes.ViewModels;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace OnlineQuizzes.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class QuizsController : Controller
    {
        private ApplicationDbContext db;

        public QuizsController()
        {
            db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        public ActionResult CreateQuizPage()
        {
            var userId = User.Identity.GetUserId();
            var majors = db.TrainerMajors.Include(c => c.Category).Where(s => s.Id == userId).Select(c => c.CategoryID).Distinct().ToList();

            List<Category> CurrentMajors = new List<Category>();

            foreach(var OneMajor in majors) {
                var major = db.Categories.Find(OneMajor);
                CurrentMajors.Add(major);
            }

            IEnumerable<Category> categories = CurrentMajors;

            var viewModel = new NewQuizViewModel
            {
                trainerMajors = categories,
            };
            return View("CreateQuiz", viewModel);
        }

        [HttpPost]
        public ActionResult CreateNewQuiz(Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var majors = db.TrainerMajors.Include(c => c.Category).Where(s => s.Id == userId).Select(c => c.CategoryID).Distinct().ToList();

                List<Category> CurrentMajors = new List<Category>();

                foreach (var OneMajor in majors)
                {
                    var major = db.Categories.Find(OneMajor);
                    CurrentMajors.Add(major);
                }

                IEnumerable<Category> categories = CurrentMajors;

                var viewModel = new NewQuizViewModel
                {
                    trainerMajors = categories,
                    quiz = quiz
                };
                return View("CreateQuiz", viewModel);
            }

            db.Quizzes.Add(quiz);
            db.SaveChanges();

            return RedirectToAction("DisplayQuestionsPage", "Questions", new { QuizID = quiz.QuizID });
        }

        public ActionResult QuizDetails(int id)
        {
            var questionsDetails = db.Questions.Include(c => c.Quiz).Where(s => s.QuizId == id).ToList();
            var viewModel = new QuestionsWithQuizID
            {
                questions = questionsDetails,
                quizID = id,
            };
            return View(viewModel);
        }

    }
}