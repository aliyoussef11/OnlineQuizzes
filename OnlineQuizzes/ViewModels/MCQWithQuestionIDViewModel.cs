using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class MCQWithQuestionIDViewModel
    {
        public MCQAnswers MCQAnswers { get; set; }
        public int questionID { get; set; }
        public int quizID { get; set; }
    }
}