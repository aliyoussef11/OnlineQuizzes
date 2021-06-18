using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineQuizzes.ViewModels;
using System.Data.Entity;

namespace OnlineQuizzes.Controllers
{
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
            var categories = db.Categories.ToList();
            var viewModel = new NewQuizViewModel
            {
                categories = categories,
            };
            return View("CreateQuiz", viewModel);
        }

        [HttpPost]
        public ActionResult CreateNewQuiz(Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                var categories = db.Categories.ToList();
                var viewModel = new NewQuizViewModel
                {
                    categories = categories,
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