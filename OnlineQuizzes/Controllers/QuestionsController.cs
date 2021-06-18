using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineQuizzes.ViewModels;
using System.Data.Entity;
using OnlineQuizzes.Extensions;

namespace OnlineQuizzes.Controllers
{
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db;

        public QuestionsController()
        {
            db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
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
                return View("~/Views/MCQAnswers/AddMCQAnswers.cshtml", viewModel);
            }
            else
            {
                this.AddNotification("Question Added Successfully", NotificationType.SUCCESS);
                return RedirectToAction("DisplayQuestionsPage", "Questions", new { QuizID = QuizID });
            }
        }

        public ActionResult AddQuestionToListPage(int QuizID)
        {
            var typeOfQuestions = db.QuestionTypes.ToList();
            var viewModel = new NewQuestionViewModel
            {
                questionTypes = typeOfQuestions,
                quizID = QuizID,
            };

            return View("AddQuestionToListFormPage", viewModel);
        }

        [HttpPost]
        public ActionResult ShowAnswerPageForList(int QuizID, Question question)
        {
            if (!ModelState.IsValid)
            {
                var typeOfQuestions = db.QuestionTypes.ToList();
                var viewModel = new NewQuestionViewModel
                {
                    questionTypes = typeOfQuestions,
                    quizID = QuizID,
                };

                return View("AddQuestionToListFormPage", viewModel);
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
                return View("~/Views/MCQAnswers/AddMCQAnswersToList.cshtml", viewModel);
            }
            else
            {
                this.AddNotification("Question Added Successfully!", NotificationType.SUCCESS);
                return RedirectToAction("QuizDetails", "Quizs", new { id = QuizID });
            }
        }

        public ActionResult QuestionDetails(int id, int QuizID)
        {
            var Question = db.Questions.Find(id);
            var type = Question.TypeId;

            if (type == 1)
            {
                var mcqanswer = db.MCQAnswers.ToList().Where(c => c.QuestionID == id);
                var viewModel = new MCQWithQuestionIDViewModel
                {
                    MCQAnswersList = mcqanswer,
                    quizID = QuizID,
                    questionID = id,
                };
                return View(viewModel);
            }
            this.AddNotification("This Type Of Questions Doesn't Contain Answers!", NotificationType.WARNING);
            return RedirectToAction("QuizDetails", "Quizs", new { id = QuizID });
        }
    }
}