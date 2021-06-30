using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineQuizzes.ViewModels;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using OnlineQuizzes.Extensions;

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
            }
            IEnumerable<StudentInterest> studentsInterests = SameInterestsStudent;

            ViewBag.CurrentMajors = new SelectList(CurrentMajors, "CategoryID", "CategoryName");
            return View("CreateQuiz");
        }

        public ActionResult GetStudents(int CategoryID)
        {
            List<StudentInterest> SameInterestsStudent = db.studentInterests.Include(c => c.Student).Where(c => c.CategoryID == CategoryID)
                .ToList();
            ViewBag.InterestStudents = new SelectList(SameInterestsStudent, "Id", "Student.StudentName");
            return PartialView("DisplayStudents");
        }

        [HttpPost]
        public ActionResult CreateNewQuiz(NewQuizViewModel quizViewModel)
        {
            if (!ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var majors = db.TrainerMajors.Include(c => c.Category).Where(s => s.Id == userId).Select(c => c.CategoryID).Distinct().ToList();

                List<Category> CurrentMajors = new List<Category>();
                List<StudentInterest> SameInterestsStudent = new List<StudentInterest>();

                foreach (var OneMajor in majors)
                {
                    var major = db.Categories.Find(OneMajor);
                    CurrentMajors.Add(major);
                }
                IEnumerable<StudentInterest> studentsInterests = SameInterestsStudent;

                ViewBag.CurrentMajors = new SelectList(CurrentMajors, "CategoryID", "CategoryName");

                var viewModel = new NewQuizViewModel
                {
                    quiz = quizViewModel.quiz
                };
                return View("CreateQuiz", viewModel);
            }

            db.Quizzes.Add(quizViewModel.quiz);
            db.SaveChanges();

            
            foreach (var Student in quizViewModel.StudentIDs)
            {
                var QuizPermission = new QuizPermission();
                QuizPermission.QuizID = quizViewModel.quiz.QuizID;
                QuizPermission.Id = Student;
                db.QuizPermissions.Add(QuizPermission);
                db.SaveChanges();
            }

            this.AddNotification("Quiz Created Successfully", NotificationType.SUCCESS);
            return RedirectToAction("DisplayQuestionsPage", "Questions", new { QuizID = quizViewModel.quiz.QuizID });
        }

        public ActionResult QuizStudents(int id)
        {
            var StudentsOfTheQuiz = db.QuizPermissions.Include(c => c.Quiz).Include(c => c.Student).Where(c => c.QuizID == id).ToList();
            var viewModel = new QuizPermissionViewModel
            {
                QuizId = id,
                QuizPermissions = StudentsOfTheQuiz
            };
            return View(viewModel);
        }

        public ActionResult DeleteStudentPermission(string StudentId, int QuizID)
        {
            var Student = db.Students.Find(StudentId);

            var QuizPermission = db.QuizPermissions.Where(c => c.Id == StudentId).Where(c=>c.QuizID == QuizID).SingleOrDefault();
            db.QuizPermissions.Remove(QuizPermission);
            db.SaveChanges();

            this.AddNotification("Permission For Student " + Student.StudentName + " Has Been Deleted Successfully", NotificationType.INFO);
            return RedirectToAction("QuizStudents", new { id = QuizID });
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

        public ActionResult NewPermission(int QuizID)
        {
            var Quiz = db.Quizzes.Include(c => c.Category).Where(c => c.QuizID == QuizID).SingleOrDefault();

            var AllInterestStudents = db.studentInterests.Include(c => c.Student)
                .Where(c => c.CategoryID == Quiz.CategoryID).Select(c => c.Id).ToList();

            var CurrentStudentPermission = db.QuizPermissions.Include(c => c.Student)
                .Where(c => c.QuizID == QuizID).Select(c => c.Id).ToList();

            List<Student> NonMentionedStudents = new List<Student>();

            foreach (var OneStudent in AllInterestStudents)
            {
                if (!CurrentStudentPermission.Contains(OneStudent))
                {
                    var ThisStudent = db.Students.Where(c => c.Id == OneStudent).SingleOrDefault();
                    NonMentionedStudents.Add(ThisStudent);
                }
            }

            IEnumerable<Student> NotMentionedStudents = NonMentionedStudents;

            var viewModel = new NewPermissionViewModel
            {
                quiz = Quiz,
                students = NotMentionedStudents
            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult CreateNewPermission(NewPermissionViewModel newPermissionViewModel,int QuizID)
        {
            if (newPermissionViewModel.StudentsIDs == null)
            {
                var Quiz = db.Quizzes.Include(c => c.Category).Where(c => c.QuizID == QuizID).SingleOrDefault();

                var AllInterestStudents = db.studentInterests.Include(c => c.Student)
                    .Where(c => c.CategoryID == Quiz.CategoryID).Select(c => c.Id).ToList();

                var CurrentStudentPermission = db.QuizPermissions.Include(c => c.Student)
                    .Where(c => c.QuizID == QuizID).Select(c => c.Id).ToList();

                List<Student> NonMentionedStudents = new List<Student>();

                foreach (var OneStudent in AllInterestStudents)
                {
                    if (!CurrentStudentPermission.Contains(OneStudent))
                    {
                        var ThisStudent = db.Students.Where(c => c.Id == OneStudent).SingleOrDefault();
                        NonMentionedStudents.Add(ThisStudent);
                    }
                }

                IEnumerable<Student> NotMentionedStudents = NonMentionedStudents;

                var viewModel = new NewPermissionViewModel
                {
                    quiz = Quiz,
                    students = NotMentionedStudents,
                    StudentsIDs = newPermissionViewModel.StudentsIDs
                };

                return View("NewPermission", viewModel);
            }

            foreach (var Student in newPermissionViewModel.StudentsIDs)
            {
                var QuizPermission = new QuizPermission()
                {
                    QuizID = QuizID,
                    Id = Student
                };
                db.QuizPermissions.Add(QuizPermission);
                db.SaveChanges();
            }
            if (newPermissionViewModel.StudentsIDs.Count() > 1) {
                this.AddNotification("Students Permissions Added Successfully To Quiz Name: " + newPermissionViewModel.quiz.QuizName, NotificationType.SUCCESS);
                }
            else
            {
                this.AddNotification("Student Permission Added Successfully To Quiz Name: " + newPermissionViewModel.quiz.QuizName, NotificationType.SUCCESS);
            }
            return RedirectToAction("QuizStudents", new { id = QuizID });
        }

    }
}