using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.Controllers.Api
{
    public class AdminController : ApiController
    {
        private ApplicationDbContext db;

        public AdminController()
        {
            db = new ApplicationDbContext();
        }

        [HttpGet]
        public IEnumerable<Trainer> GetTrainers()
        {
            return db.Trainers.ToList();
        }

        [HttpPost]
        public Trainer InsertTrainer(Trainer trainer)
        {
            db.Trainers.Add(trainer);
            db.SaveChanges();
            return trainer;
        }


    }
}
