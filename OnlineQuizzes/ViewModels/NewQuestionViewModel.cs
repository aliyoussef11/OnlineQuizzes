using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class NewQuestionViewModel
    {
        public IEnumerable<QuestionType> questionTypes { get; set; }
        public Question question { get; set; }
        public int quizID { get; set; }
    }
}