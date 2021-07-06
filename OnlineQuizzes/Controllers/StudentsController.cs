﻿using OnlineQuizzes.Models;
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
            var studentId = User.Identity.GetUserId();
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

                var viewModel = new AttemptQuizViewModel
                {
                    FillQuestions = FillInTheBlankQuestions,
                    MCQQuestions = mCQQuestions,
                    MCQAnswers = mCQs,
                    QuizID = QuizID,
                    quiz = Quiz,
                    StudentID = studentId
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
        public ActionResult SubmitQuiz(AttemptQuizViewModel attemptQuizViewModel, int QuizID)
        {
            var studentId = User.Identity.GetUserId();

            double grade = 0;
            double TotalQuizGrade = 0;
            double SuccessGrade = 0;
            double GoodGrade = 0;
            double VeryGoodGrade = 0;
            double ExcellentGrade = 0;
            string Result = "";
            if (attemptQuizViewModel.QuizFillInTheBlankAnswers == null)
            {
                foreach (var MCQanswer in attemptQuizViewModel.MCQQuestionsAnswers)
                {
                    var QuestionDetails = db.Questions.Find(MCQanswer.QuestionID);
                    var AnswersOfCurrentQuestion = db.MCQAnswers.Find(MCQanswer.QuestionID);
                    var CorrectAnswer = AnswersOfCurrentQuestion.CorrectAnswer;

                    //db.AttemptedQuizMCQAnswers.Add(MCQanswer);
                    //db.SaveChanges();

                    if (MCQanswer.Answer == CorrectAnswer)
                    {
                        grade += QuestionDetails.GradeOfQuestion;
                    }

                    TotalQuizGrade += QuestionDetails.GradeOfQuestion;
                }

                SuccessGrade = TotalQuizGrade / 2;
                GoodGrade = TotalQuizGrade / 1.4;
                VeryGoodGrade = TotalQuizGrade / 1.2;
                ExcellentGrade = TotalQuizGrade / 1.1;

                if (grade < SuccessGrade)
                {
                    Result = "Failed! Try Again With Different Quiz!";
                }
                else if (grade > SuccessGrade && grade < GoodGrade)
                {
                    Result = "You Success But You must Work on Your Abilities!";
                }
                else if (grade > GoodGrade && grade < VeryGoodGrade)
                {
                    Result = "Good Work!";
                }
                else if (grade > VeryGoodGrade && grade < ExcellentGrade)
                {
                    Result = "Very Good Work Keep Going!";
                }
                else if (grade > ExcellentGrade)
                {
                    Result = "Excellent Work .. !";
                }

            }


            var viewModel = new GradeViewModel
            {
                Grade = grade,
                TotalQuizGrade = TotalQuizGrade,
                Result = Result
            };


            var StudentGrade = new StudentGrade
            {
                Id = studentId,
                QuizId = QuizID,
                Grade = grade,
                TotalGrade = TotalQuizGrade,
                Result = Result
            };

            db.StudentGrades.Add(StudentGrade);
            db.SaveChanges();

            return View(viewModel);
        }

        public ActionResult MyGrades()
        {
            var studentId = User.Identity.GetUserId();
            List<StudentGrade> studentGrades = new List<StudentGrade>();

            var grades = db.StudentGrades.Include(c => c.Quiz).Where(c => c.Id == studentId).ToList();

            foreach(var grade in grades)
            {
                studentGrades.Add(grade);
            }
            IEnumerable<StudentGrade> IE_StudentsGrade = studentGrades;

            return View(IE_StudentsGrade);
        }

        public ActionResult StudentProfile()
        {
            var StudentId = User.Identity.GetUserId();
            var StudentsEmail = User.Identity.GetUserName();

            var StudentInfo = db.Students.Find(StudentId);

            List<Category> CurrentInterests = new List<Category>();
            var majors = db.studentInterests.Include(c => c.Category).Where(s => s.Id == StudentId).Select(c => c.CategoryID).Distinct().ToList();

            foreach (var OneMajor in majors)
            {
                var major = db.Categories.Find(OneMajor);
                CurrentInterests.Add(major);
            }
            IEnumerable<Category> studentInterests = CurrentInterests;

            var ViewModel = new StudentProfileViewModel
            {
                student = StudentInfo,
                studentInterests = studentInterests,
                StudentEmail = StudentsEmail
            };

            return View(ViewModel);
        }

    }
}