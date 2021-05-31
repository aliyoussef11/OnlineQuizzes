using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class MCQAnswers
    {
        [Key]
        public int MCQID { get; set; }

        // Generate Foreign Key to Question
        public int QuestionID { get; set; }
        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }

        [Display(Name = "First Possible Answer")]
        [Required]
        public string FirstPossibleAnswer { get; set; }

        [Display(Name = "Second Possible Answer")]
        [Required]
        public string SecondPossibleAnswer { get; set; }

        [Display(Name = "Third Possible Answer")]
        public string ThirdPossibleAnswer { get; set; }

        [Display(Name = "Fourth Possible Answer")]
        public string FourthPossibleAnswer { get; set; }

        [Display(Name = "The Correct Answer")]
        [Required]
        public string CorrectAnswer { get; set; }


    }
}