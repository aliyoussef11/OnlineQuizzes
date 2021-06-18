using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineQuizzes.Models;
using OnlineQuizzes.ViewModels;
using OnlineQuizzes.Extensions;

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
                this.AddNotification("Question Added Successfully", NotificationType.SUCCESS);
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

            this.AddNotification("Question Added Successfully", NotificationType.SUCCESS);
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
                this.AddNotification("Question Added Successfully!", NotificationType.SUCCESS);
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

            this.AddNotification("Question Added Successfully!", NotificationType.SUCCESS);
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
            this.AddNotification("This Type Of Questions Doesn't Contain Answers!", NotificationType.WARNING);
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

            this.AddNotification("Answers Added Successfully To The Question", NotificationType.SUCCESS);
            return RedirectToAction("QuizDetails", new { id = QuizID });
        }

        public ActionResult DeleteQuiz(int id)
        {
            var QuestionsIDs = db.Questions.Where(c => c.QuizId == id).Select(c => c.QuestionID).ToList();
            var mCQAnswersIDs = new List<int> ();
            for (int i=0; i<QuestionsIDs.Count; i++)
            {
                int QuestionID = Convert.ToInt32(QuestionsIDs[i]);
                mCQAnswersIDs.Add(db.MCQAnswers.Where(c => c.QuestionID == QuestionID).Select(c => c.MCQID).SingleOrDefault());
            }

            for (int i = 0; i < mCQAnswersIDs.Count; i++)
            {
                var OneAnswer = db.MCQAnswers.Find(mCQAnswersIDs[i]);
                if (!(OneAnswer == null))
                {
                    db.MCQAnswers.Remove(OneAnswer);
                    db.SaveChanges();
                }
            }

            for (int i = 0; i < QuestionsIDs.Count; i++)
            {
                var OneQuestion = db.Questions.Find(QuestionsIDs[i]);
                db.Questions.Remove(OneQuestion);
                db.SaveChanges();
            }

            var Quiz = db.Quizzes.Find(id);
            db.Quizzes.Remove(Quiz);
            db.SaveChanges();

            this.AddNotification("Quiz Deleted Successfully!", NotificationType.SUCCESS);
            return RedirectToAction("ListOfMyQuizzes");

        }

        public ActionResult DeleteQuestion(int id, int QuizID)
        {
            var McqID = db.MCQAnswers.Where(c => c.QuestionID == id).Select(c => c.MCQID).SingleOrDefault();
            var MCQ_RelatedTo_Question = db.MCQAnswers.Find(McqID);
            var Question = db.Questions.Find(id);

            if(!(MCQ_RelatedTo_Question == null)){
                db.MCQAnswers.Remove(MCQ_RelatedTo_Question);
                db.Questions.Remove(Question);
                db.SaveChanges();
            }
            else
            {
                db.Questions.Remove(Question);
                db.SaveChanges();
            }

            this.AddNotification("Question Deleted Successfully!", NotificationType.SUCCESS);
            return RedirectToAction("QuizDetails", new { id = QuizID });
        }

        public ActionResult EditQuiz(int id)
        {
            var categories = db.Categories.ToList();
            var Quiz = db.Quizzes.Find(id);

            if (Quiz == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", Quiz.CategoryID);
            return View(Quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuiz(Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", quiz.CategoryID);
                return View(quiz);
            }

            db.Entry(quiz).State = EntityState.Modified;
            db.SaveChanges();

            this.AddNotification("Quiz Edited Successfully!", NotificationType.SUCCESS);
            return RedirectToAction("ListOfMyQuizzes");
        }

        public ActionResult EditQuestion(int id, int QuizID)
        {
            var typeOfQuestions = db.QuestionTypes.ToList();
            var question = db.Questions.Find(id);
            var viewModel = new NewQuestionViewModel
            {
                questionTypes = typeOfQuestions,
                question = question,
                quizID = QuizID
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion(Question question, int QuizID)
        {
            if (!ModelState.IsValid)
            {
                var typeOfQuestions = db.QuestionTypes.ToList();
                var viewModel = new NewQuestionViewModel
                {
                    questionTypes = typeOfQuestions,
                    question = question,
                    quizID = QuizID
                };
                return View(viewModel);
            }

            db.Entry(question).State = EntityState.Modified;
            db.SaveChanges();

            this.AddNotification("Question Edited Successfully!", NotificationType.SUCCESS);
            return RedirectToAction("QuizDetails", new { id = QuizID});
        }

        public ActionResult EditAnswer(int id, int QuestionID, int QuizID)
        {
            var MCQ = db.MCQAnswers.Find(id);
            var viewModel = new MCQWithQuestionIDViewModel
            {
                MCQAnswers = MCQ,
                questionID = QuestionID,
                quizID = QuizID
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAnswer(MCQAnswers mCQAnswers, int QuestionID, int QuizID)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MCQWithQuestionIDViewModel
                {
                    MCQAnswers = mCQAnswers,
                    questionID = QuestionID,
                    quizID = QuizID
                };
                return View(viewModel);
            }

            db.Entry(mCQAnswers).State = EntityState.Modified;
            db.SaveChanges();

            this.AddNotification("Answers Edited Successfully!", NotificationType.SUCCESS);
            return RedirectToAction("QuestionDetails", new { id = QuestionID, QuizID = QuizID });
        }
    }
}