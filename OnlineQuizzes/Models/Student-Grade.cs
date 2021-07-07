using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class Student_Grade
    {
        [Key]
        public int AttemptID { get; set; }

        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual Student Student { get; set; }

        // Generate Foreign Key to Quiz
        public int QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }

        public double Grade { get; set; }

        public double TotalGrade { get; set; }

        public string Result { get; set; }
    }
}