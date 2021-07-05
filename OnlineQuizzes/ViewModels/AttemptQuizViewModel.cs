using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class AttemptQuizViewModel
    {
        public IEnumerable<Question> FillQuestions { get; set; }
        public IEnumerable<Question> MCQQuestions { get; set; }
        public IEnumerable<MCQAnswers> MCQAnswers { get; set; }
        public int QuizID { get; set; }
        public Quiz quiz { get; set; }

        public List<QuizAnswer> quizAnswers { get; set; }
    }
}