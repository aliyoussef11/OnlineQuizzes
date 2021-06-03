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
                return RedirectToAction("DisplayQuestionsPage", "Trainers", new { QuizID = QuizID });
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

            return RedirectToAction("DisplayQuestionsPage", "Trainers", new { QuizID = QuizID });
        }

        public ActionResult ListOfMyQuizzes()
        {
            var myQuizzes = db.Quizzes.Where(c => c.TrainerName == User.Identity.Name).ToList();
            return View(myQuizzes);
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
                return View("AddMCQAnswersToList", viewModel);
            }
            else
            {
                return RedirectToAction("QuizDetails", "Trainers", new { id = QuizID });
            }
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

            return RedirectToAction("QuizDetails", "Trainers", new { id = QuizID });
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
            return RedirectToAction("QuizDetails", "Trainers", new { id = QuizID });
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

            return RedirectToAction("QuizDetails", new { id = QuizID });
        }
    }
}