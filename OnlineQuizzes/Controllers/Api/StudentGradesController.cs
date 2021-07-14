using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineQuizzes.Controllers.Api
{
    public class StudentGradesController : ApiController
    {
        private ApplicationDbContext db;

        public StudentGradesController()
        {
            db = new ApplicationDbContext();
        }
        [HttpGet]
        public IEnumerable<Student_Grade> GetStudents()
        {
            return db.StudentGrades.ToList();
        }

    }
}
