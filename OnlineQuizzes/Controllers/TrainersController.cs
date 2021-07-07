using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineQuizzes.Models;
using OnlineQuizzes.ViewModels;
using OnlineQuizzes.Extensions;
using Microsoft.AspNet.Identity;

namespace OnlineQuizzes.Controllers
{
    [Authorize(Roles = "Trainer")]
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

        public ActionResult ListOfMyQuizzes()
        {
            var myQuizzes = db.Quizzes.Where(c => c.TrainerName == User.Identity.Name).ToList();
            return View(myQuizzes);
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
            return RedirectToAction("QuizDetails", "Quizs", new { id = QuizID });
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
            return RedirectToAction("QuizDetails", "Quizs", new { id = QuizID});
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
            return RedirectToAction("QuestionDetails", "Questions", new { id = QuestionID, QuizID = QuizID });
        }

        public ActionResult TrainerProfile()
        {
            var TrainerId = User.Identity.GetUserId();
            var TrainerEmail = User.Identity.GetUserName();

            var TrainerInfo = db.Trainers.Find(TrainerId);

            List<Category> CurrentMajors = new List<Category>();
            var majors = db.TrainerMajors.Include(c => c.Category).Where(s => s.Id == TrainerId).Select(c => c.CategoryID).Distinct().ToList();

            foreach (var OneMajor in majors)
            {
                var major = db.Categories.Find(OneMajor);
                CurrentMajors.Add(major);
            }
            IEnumerable<Category> trainerMajors = CurrentMajors;

            var ViewModel = new TrainerProfile
            {
                trainer = TrainerInfo,
                TrainerMajors = trainerMajors,
                TrainerEmail = TrainerEmail
            };

            return View(ViewModel);
        }

        public ActionResult Notification()
        {
            var TrainerId = User.Identity.GetUserId();
            var myQuizzes = db.Quizzes.Where(c => c.TrainerName == User.Identity.Name).ToList();

            var StudentName = "";
            var Helper = new QuizMCQsAnswer();
            var Helper2 = new QuizFillIBAnswer();
            
            List<Students_sAttemptedQuizViewModel> students_SAttemptedQuizzes = new List<Students_sAttemptedQuizViewModel>();

            foreach (var OneQuiz in myQuizzes)
            {
                Helper = db.QuizMCQsAnswers.Include(c=>c.Student).Where(s => s.QuizId == OneQuiz.QuizID).FirstOrDefault();
                Helper2 = db.QuizFillIBAnswers.Include(c => c.Student).Where(s => s.QuizId == OneQuiz.QuizID).FirstOrDefault();
                if (Helper == null)
                {
                    StudentName = Helper2.Student.StudentName;
                }
                else
                {
                    StudentName = Helper.Student.StudentName;
                }

                var viewModel = new Students_sAttemptedQuizViewModel
                {
                    StudentName = StudentName,
                    quiz = OneQuiz
                };

                students_SAttemptedQuizzes.Add(viewModel);
            }

            IEnumerable<Students_sAttemptedQuizViewModel> viewModels = students_SAttemptedQuizzes;

            var HelperViewModel = new AttemptedStudentListHelperViewModel
            {
                students_S = viewModels
            };

            return View(HelperViewModel);
        }

        public ActionResult CorrectQuiz(string StudentName, int QuizID)
        {
            var StudentID = db.Students.Where(s => s.StudentName == StudentName)
                .Select(c => c.Id).SingleOrDefault();

            var QuizMCQAnswers = db.QuizMCQsAnswers.Where(c => c.Id == StudentID).Where(c => c.QuizId == QuizID)
                .ToList();

            double GradeWithoutFillinTheBlankCorrection = 0;
            foreach(var mcqAnswer in QuizMCQAnswers)
            {
                var MCQAnswer = db.MCQAnswers.Where(s=>s.QuestionID == mcqAnswer.QuestionID).SingleOrDefault();
                var question = db.Questions.Find(mcqAnswer.QuestionID);

                if(mcqAnswer.Answer == MCQAnswer.CorrectAnswer)
                {
                    GradeWithoutFillinTheBlankCorrection += question.GradeOfQuestion;
                }
            }

            var FillIBAnswers = db.QuizFillIBAnswers.Where(c => c.Id == StudentID).Where(c => c.QuizId == QuizID)
                .ToList();

            List<FillInBlankQuestionsWithQuestionViewModel> fillInBlankQuestions = new List<FillInBlankQuestionsWithQuestionViewModel>();
            foreach(var FIBQ in FillIBAnswers)
            {
                var question = db.Questions.Find(FIBQ.QuestionID);

                var viewModel = new FillInBlankQuestionsWithQuestionViewModel
                {
                    quizFillIBAnswer = FIBQ,
                    question = question
                };

                fillInBlankQuestions.Add(viewModel);
            }

            IEnumerable<FillInBlankQuestionsWithQuestionViewModel> fillInBlanks = fillInBlankQuestions;

            var FillInBlankViewModelHelper = new FInBlankQuestionWithMCQGrade
            {
                fillInBlanks = fillInBlanks,
                GradeWithoutFillInBlankQuestion = GradeWithoutFillinTheBlankCorrection
            };

            return View(FillInBlankViewModelHelper);
        }

        [HttpPost]
        public ActionResult Check(FInBlankQuestionWithMCQGrade fIn, double GradeWithoutFill)
        {
            return View();
        }
    }
}