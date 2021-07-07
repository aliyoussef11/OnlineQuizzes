using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class FillInBlankQuestionsWithQuestionViewModel
    {
        public QuizFillIBAnswer quizFillIBAnswer { get; set; }
        public Question question { get; set; }
    }
}