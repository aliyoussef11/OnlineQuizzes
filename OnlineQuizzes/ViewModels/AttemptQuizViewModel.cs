using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class AttemptQuizViewModel
    {
        public List<Question> FillQuestions { get; set; }
        public List<Question> MCQQuestions { get; set; }
        public IEnumerable<MCQAnswers> MCQAnswers { get; set; }
        public int QuizID { get; set; }
        public Quiz quiz { get; set; }

        public List<QuizMCQsAnswer> MCQQuestionsAnswers { get; set; }
        public List<QuizFillIBAnswer> QuizFillInTheBlankAnswers { get; set; }

        public string StudentID { get; set; }
    }
}