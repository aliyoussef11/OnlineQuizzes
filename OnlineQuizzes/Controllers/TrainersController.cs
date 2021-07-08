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

        public ActionResult EditTrainerProfile()
        {
            var TrainerId = User.Identity.GetUserId();

            var trainerInfo = db.Trainers.Find(TrainerId);
            var Majors = db.TrainerMajors.Include(s=>s.Category).Where(s => s.Id == TrainerId).ToList();

            var AllMajors = db.Categories.ToList();

            var viewModel = new EditTrainerProfileViewModel{
                trainer = trainerInfo,
                trainerMajors = Majors,
                allMajors = AllMajors
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditTrainerProfile(EditTrainerProfileViewModel editTrainer)
        {
            
            if (!ModelState.IsValid)
            {
                var TrainerId = User.Identity.GetUserId();

                var trainerInfo = db.Trainers.Find(TrainerId);
                var Majors = db.TrainerMajors.Include(s => s.Category).Where(s => s.Id == TrainerId).ToList();

                var AllMajors = db.Categories.ToList();

                var viewModel = new EditTrainerProfileViewModel
                {
                    trainer = trainerInfo,
                    trainerMajors = Majors,
                    allMajors = AllMajors,
                    trainerBackEdit = editTrainer.trainer
                };

                return View(viewModel);
            }

            var ID = User.Identity.GetUserId();
            var TrainerCurrentMajors = db.TrainerMajors.Where(s => s.Id == ID).ToList();

            if (TrainerCurrentMajors != null)
            {
                foreach (var major in TrainerCurrentMajors)
                {
                    db.TrainerMajors.Remove(major);
                }
            }

            db.Entry(editTrainer.trainer).State = EntityState.Modified;
            db.SaveChanges();

            var OneMajor = new TrainerMajors
            {
                Id = ID
            };
            foreach (var major in editTrainer.Majors)
            {
                OneMajor.CategoryID = major;
                db.TrainerMajors.Add(OneMajor);
                db.SaveChanges();
            }

            this.AddNotification("Profile Edited Successfully ...", NotificationType.SUCCESS);
            return RedirectToAction("TrainerProfile");
        }

            public ActionResult Notification()
        {
            var TrainerId = User.Identity.GetUserId();
            var myQuizzes = db.Quizzes.Where(c => c.TrainerName == User.Identity.Name).ToList();

            var StudentName = "";
            var Helper = new QuizMCQsAnswer();
            var Helper2 = new QuizFillIBAnswer();
            var student = new Student();
            
            List<Students_sAttemptedQuizViewModel> students_SAttemptedQuizzes = new List<Students_sAttemptedQuizViewModel>();

            if (myQuizzes != null)
            {
                foreach (var OneQuiz in myQuizzes)
                {
                    Helper = db.QuizMCQsAnswers.Include(c => c.Student).Where(s => s.QuizId == OneQuiz.QuizID).FirstOrDefault();
                    Helper2 = db.QuizFillIBAnswers.Include(c => c.Student).Where(s => s.QuizId == OneQuiz.QuizID).FirstOrDefault();
                    if (Helper != null || Helper2 != null)
                    {
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
                    else
                    {
                        IEnumerable<Students_sAttemptedQuizViewModel> viewModelNull = students_SAttemptedQuizzes;

                        var HelpViewModel = new AttemptedStudentListHelperViewModel
                        {
                            students_S = viewModelNull,
                        };

                        return View(HelpViewModel);
                    }
                }
            }
            IEnumerable<Students_sAttemptedQuizViewModel> viewModels = students_SAttemptedQuizzes;

            var HelperViewModel = new AttemptedStudentListHelperViewModel
            {
                students_S = viewModels,
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
            double TotalGrade = 0;
            foreach(var mcqAnswer in QuizMCQAnswers)
            {
                var MCQAnswer = db.MCQAnswers.Where(s=>s.QuestionID == mcqAnswer.QuestionID).SingleOrDefault();
                var question = db.Questions.Find(mcqAnswer.QuestionID);

                TotalGrade += question.GradeOfQuestion;

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

                TotalGrade += question.GradeOfQuestion;

                var viewModel = new FillInBlankQuestionsWithQuestionViewModel
                {
                    quizFillIBAnswer = FIBQ,
                    question = question         
                };

                fillInBlankQuestions.Add(viewModel);
            }

            IEnumerable<FillInBlankQuestionsWithQuestionViewModel> fillInBlanks = fillInBlankQuestions;

            var student = db.Students.Where(s => s.StudentName == StudentName).FirstOrDefault();

            var FillInBlankViewModelHelper = new FInBlankQuestionWithMCQGrade
            {
                fillInBlanks = fillInBlanks,
                GradeWithoutFillInBlankQuestion = GradeWithoutFillinTheBlankCorrection,
                TotalGrade = TotalGrade,
                student = student,
                quizID = QuizID
        };

            return View(FillInBlankViewModelHelper);
        }

        [HttpPost]
        public ActionResult PostGrade(FInBlankQuestionWithMCQGrade fIn, double GradeWithoutFill, double TotalGrade, string StudentID, int QuizID)
        {
            double FinalStudentGrade = GradeWithoutFill;
            foreach(var oneFillInTheBlankGrade in fIn.IDGrade)
            {
                FinalStudentGrade += oneFillInTheBlankGrade.QuestionGrade;
            }

            double SuccessGrade = TotalGrade / 2;
            double GoodGrade = TotalGrade / 1.4;
            double VeryGoodGrade = TotalGrade / 1.2;
            double ExcellentGrade = TotalGrade / 1.1;
            string Result = "";

            if (FinalStudentGrade < SuccessGrade)
            {
                Result = "Failed! Try Again With Different Quiz!";
            }
            else if (FinalStudentGrade > SuccessGrade && FinalStudentGrade < GoodGrade)
            {
                Result = "You Success But You must Work on Your Abilities!";
            }
            else if (FinalStudentGrade > GoodGrade && FinalStudentGrade < VeryGoodGrade)
            {
                Result = "Good Work!";
            }
            else if (FinalStudentGrade > VeryGoodGrade && FinalStudentGrade < ExcellentGrade)
            {
                Result = "Very Good Work Keep Going!";
            }
            else if (FinalStudentGrade > ExcellentGrade)
            {
                Result = "Excellent Work .. !";
            }

            var StudentGrade = new Student_Grade
            {
                Id = StudentID,
                QuizId = QuizID,
                Grade = FinalStudentGrade,
                TotalGrade = TotalGrade,
                Result = Result
            };

            db.StudentGrades.Add(StudentGrade);
            db.SaveChanges();

            //Remove MCQ  Answers Related to this student
            var MCQAnswers = db.QuizMCQsAnswers.Where(s => s.Id == StudentID).ToList();
            foreach(var mcq in MCQAnswers)
            {
                db.QuizMCQsAnswers.Remove(mcq);
                db.SaveChanges();
            }

            //Remove Fill in the blank Answers Related to this student
            var FillInBlankAnswers = db.QuizFillIBAnswers.Where(s => s.Id == StudentID).ToList();
            foreach (var fill in FillInBlankAnswers)
            {
                db.QuizFillIBAnswers.Remove(fill);
                db.SaveChanges();
            }

            this.AddNotification("Quiz Corrected Successfully ...", NotificationType.SUCCESS);
            return RedirectToAction("ListStudentGrades");
        }

        public ActionResult ListStudentGrades()
        {
            var TrainerId = User.Identity.GetUserId();
            var myQuizzes = db.Quizzes.Where(c => c.TrainerName == User.Identity.Name).ToList();

            List<Student_Grade> student_Grades = new List<Student_Grade>();
            
            foreach(var quiz in myQuizzes)
            {
                var StudentAttemptedOneQuiz = db.StudentGrades.Where(s => s.QuizId == quiz.QuizID).ToList();
                foreach(var oneStudent in StudentAttemptedOneQuiz)
                {
                    student_Grades.Add(oneStudent);
                }
            }

            IEnumerable<Student_Grade> _Grades = student_Grades;

            return View(_Grades);
        }
    }
}