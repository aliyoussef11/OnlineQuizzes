using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineQuizzes.Controllers.Api
{
    public class QuizzesController : ApiController
    {
        private ApplicationDbContext db;

        public QuizzesController()
        {
            db = new ApplicationDbContext();
        }

        [HttpGet]
        public IEnumerable<Quiz> GetStudents()
        {
            return db.Quizzes.ToList();
        }

    }
}
