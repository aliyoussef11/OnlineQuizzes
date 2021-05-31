using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineQuizzes.Models;
using OnlineQuizzes.ViewModels;

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

            return RedirectToAction("DisplayQuestionsPage", "Trainers", new { QuizID = quiz.QuizID });
        }

        public ActionResult DisplayQuestionsPage(int QuizID)
        {
            var QuestionsOfThisQuiz = db.Questions.Include(c => c.Quiz).Where(s => s.QuizId == QuizID).ToList();
            var viewModel = new QuestionsWithQuizID
            {
                questions = QuestionsOfThisQuiz,
                quizID = QuizID,
            };
            return View("AddQuestions", viewModel);
        }

        public ActionResult AddQuestionPage(int QuizID)
        {
            var typeOfQuestions = db.QuestionTypes.ToList();
            var viewModel = new NewQuestionViewModel
            {
                questionTypes = typeOfQuestions,
                quizID = QuizID,
            };

            return View("AddQuestionFormPage", viewModel);
        }

        [HttpPost]
        public ActionResult ShowAnswerPage(int QuizID, Question question)
        {
            if (!ModelState.IsValid)
            {
                var typeOfQuestions = db.QuestionTypes.ToList();
                var viewModel = new NewQuestionViewModel
                {
                    questionTypes = typeOfQuestions,
                    quizID = QuizID,
                };

                return View("AddQuestionFormPage", viewModel);
            }

            db.Questions.Add(question);
            db.SaveChanges();

            if (question.TypeId == 1)
            {
                var viewModel = new MCQWithQuestionIDViewModel
                {
                    questionID = question.QuestionID,
                    quizID = QuizID,
                };
                return View("AddMCQAnswers", viewModel);
            }
            else
            {
                return View();
            }
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

            return RedirectToAction("DisplayQuestionsPage", "Trainers", new { QuizID = QuizID});
        }
    }
}