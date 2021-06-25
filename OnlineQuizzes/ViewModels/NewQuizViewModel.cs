using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class NewQuizViewModel
    {
        public IEnumerable<Category> trainerMajors { get; set; }

        public Quiz quiz { get; set; }

        public IEnumerable<StudentInterest> studentInterests { get; set; }

        [Required(ErrorMessage = "Please Select Users")]
        [Display(Name = "Students")]
        public IEnumerable<int> StudentIDs { get; set; }
    }
}