using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class Students_sAttemptedQuizViewModel
    {
        public Quiz quiz { get; set; }
        public string StudentName { get; set; }
    }
}