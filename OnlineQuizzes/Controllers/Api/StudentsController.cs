using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineQuizzes.Controllers.Api
{
    public class StudentsController : ApiController
    {
        private ApplicationDbContext db;

        public StudentsController()
        {
            db = new ApplicationDbContext();
        }

        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return db.Students.ToList();
        }

        [HttpPost]
        public Student InsertStudent(Student student)
        {
            db.Students.Add(student);
            db.SaveChanges();
            return student;
        }

    }
}
