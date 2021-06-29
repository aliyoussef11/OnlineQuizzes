using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class TrainerProfile
    {
        public string TrainerEmail { get; set; }
        public Trainer trainer { get; set; }
        public IEnumerable<Category> TrainerMajors { get; set; }
    }
}