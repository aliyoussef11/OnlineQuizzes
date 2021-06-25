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
            List<StudentInterest> SameInterestsStudent = new List<StudentInterest>();

            foreach (var OneMajor in majors) {
                var major = db.Categories.Find(OneMajor);
                CurrentMajors.Add(major);

                var studentSameInterests = db.studentInterests.Include(c => c.Student).Include(c => c.Category)
                    .Where(s => s.CategoryID == OneMajor).ToList();
                foreach(var OneInterest in studentSameInterests)
                {
                    SameInterestsStudent.Add(OneInterest);
                }
            }

            IEnumerable<Category> categories = CurrentMajors;
            IEnumerable<StudentInterest> studentsInterests = SameInterestsStudent;

            var viewModel = new NewQuizViewModel
            {
                trainerMajors = categories,
                studentInterests = studentsInterests
            };
            return View("CreateQuiz", viewModel);
        }

        [HttpPost]
        public ActionResult CreateNewQuiz(NewQuizViewModel quizViewModel)
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
                    quiz = quizViewModel.quiz
                };
                return View("CreateQuiz", viewModel);
            }

            db.Quizzes.Add(quizViewModel.quiz);
            db.SaveChanges();

            return RedirectToAction("DisplayQuestionsPage", "Questions", new { QuizID = quizViewModel.quiz.QuizID });
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