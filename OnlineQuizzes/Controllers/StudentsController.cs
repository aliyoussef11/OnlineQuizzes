using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using OnlineQuizzes.ViewModels;
using OnlineQuizzes.Extensions;

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

        public ActionResult AttemptQuiz(int QuizID)
        {
            var Quiz = db.Quizzes.Find(QuizID);
            if (Quiz.TimeOfQuiz < DateTime.Now)
            {

                var Questions = db.Questions.Include(c => c.QuestionType).Where(c => c.QuizId == QuizID).ToList();

                List<MCQAnswers> mCQAnswers = new List<MCQAnswers>();
                List<Question> mCQQuestions = new List<Question>();
                List<Question> FillInTheBlankQuestions = new List<Question>();

                foreach (var question in Questions)
                {
                    if (question.QuestionType.Type == "Multiple Choice")
                    {
                        mCQQuestions.Add(question);
                        mCQAnswers.Add(db.MCQAnswers.Find(question.QuestionID));
                    }
                    else
                    {
                        FillInTheBlankQuestions.Add(question);
                    }
                }

                IEnumerable<MCQAnswers> mCQs = mCQAnswers;
                IEnumerable<Question> MCQquestions = mCQQuestions;
                IEnumerable<Question> FillBlank = FillInTheBlankQuestions;

                var viewModel = new AttemptQuizViewModel
                {
                    FillQuestions = FillInTheBlankQuestions,
                    MCQQuestions = mCQQuestions,
                    MCQAnswers = mCQs,
                    QuizID = QuizID,
                    quiz = Quiz
                };

                return View(viewModel);
            }
            else
            {
                this.AddNotification("This Quiz is Not Available Yet!", NotificationType.INFO);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Check(AttemptQuizViewModel attemptQuizViewModel)
        {
            return View();
        }
    }
}