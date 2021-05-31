using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class Question
    {
        public int QuestionID { get; set; }

        // Generate Foreign Key to Quiz
        public int QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }

        // Generate Foreign Key to Type Of Question
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual QuestionType QuestionType { get; set; }

        [Display(Name = "Question")]
        [Required]
        public string QuestionText { get; set; }

        [Display(Name = "Grade Of This Question")]
        public int GradeOfQuestion { get; set; }

    }
}