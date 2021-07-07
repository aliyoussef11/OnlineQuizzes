using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class QuizMCQsAnswer
    {
        [Key]
        public int mcqAnswerID { get; set; }

        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual Student Student { get; set; }

        public int QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }

        public int QuestionID { get; set; }
        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }

        public string Answer { get; set; }
    }
}