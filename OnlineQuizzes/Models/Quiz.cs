using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class Quiz
    {
        public int QuizID { get; set; }

        [Display(Name ="Trainer Name")]
        [Required]
        public string TrainerName { get; set; }

        [Display(Name = "Quiz Name")]
        [StringLength(30)]
        [Required]
        public string QuizName { get; set; }

        [Display(Name = "Category Of The Quiz")]
        [Required]
        public string Category { get; set; }

        [Display(Name = "Duration Of The Quiz")]
        [Range(1,60)]
        [Required]
        public int DurationOfQuiz { get; set; }

        [Display(Name = "Which Time This Quiz Will Be Available?")]
        [Required]
        public DateTime TimeOfQuiz { get; set; }
    }
}