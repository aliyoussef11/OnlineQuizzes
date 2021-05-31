using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class QuestionsWithQuizID
    {
        public IEnumerable<Question> questions { get; set; }
        public int  quizID { get; set; }
        public Question question { get; set; }
    }
}