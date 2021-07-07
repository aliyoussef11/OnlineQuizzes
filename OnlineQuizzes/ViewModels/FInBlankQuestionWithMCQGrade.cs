using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.ViewModels;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class FInBlankQuestionWithMCQGrade
    {
        public IEnumerable<FillInBlankQuestionsWithQuestionViewModel> fillInBlanks { get; set; }
        public double GradeWithoutFillInBlankQuestion { get; set; }
        public Student_Grade student_Grade { get; set; }
        public double GradeForEachQuestion { get; set; }

        public List<QuestionIDWithQuestionGradeViewModel> IDGrade { get; set; }
    }
}