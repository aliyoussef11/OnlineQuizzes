using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineQuizzes.ViewModels;
using OnlineQuizzes.Extensions;

namespace OnlineQuizzes.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class MCQAnswersController : Controller
    {
        private ApplicationDbContext db;

        public MCQAnswersController()
        {
            db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }

        [HttpPost]
        public ActionResult AddMCQQuestion(int QuizID, MCQAnswers mCQAnswers)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MCQWithQuestionIDViewModel
                {
                    questionID = mCQAnswers.QuestionID,
                    quizID = QuizID,
                };
                return View("AddMCQAnswers", viewModel);
            }

            db.MCQAnswers.Add(mCQAnswers);
            db.SaveChanges();

            this.AddNotification("Question Added Successfully", NotificationType.SUCCESS);
            return RedirectToAction("DisplayQuestionsPage", "Questions", new { QuizID = QuizID });
        }

        [HttpPost]
        public ActionResult AddMCQQuestionToList(int QuizID, MCQAnswers mCQAnswers)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MCQWithQuestionIDViewModel
                {
                    questionID = mCQAnswers.QuestionID,
                    quizID = QuizID,
                };
                return View("AddMCQAnswersToList", viewModel);
            }

            db.MCQAnswers.Add(mCQAnswers);
            db.SaveChanges();

            this.AddNotification("Question Added Successfully!", NotificationType.SUCCESS);
            return RedirectToAction("QuizDetails", "Quizs", new { id = QuizID });
        }

        public ActionResult AddAnswersIfNull(int QuestionID, int QuizID)
        {
            var viewModel = new MCQWithQuestionIDViewModel
            {
                questionID = QuestionID,
                quizID = QuizID,
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddAnswersToDbIfNull(MCQAnswers mCQAnswers, int QuizID)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MCQWithQuestionIDViewModel
                {
                    questionID = mCQAnswers.QuestionID,
                    MCQAnswers = mCQAnswers,
                    quizID = QuizID,
                };
                return View("AddAnswersIfNull", viewModel);
            }

            db.MCQAnswers.Add(mCQAnswers);
            db.SaveChanges();

            this.AddNotification("Answers Added Successfully To The Question", NotificationType.SUCCESS);
            return RedirectToAction("QuizDetails", "Quizs", new { id = QuizID });
        }
    }
}