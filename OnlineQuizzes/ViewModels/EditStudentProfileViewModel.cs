using OnlineQuizzes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.ViewModels
{
    public class EditStudentProfileViewModel
    {
        public Student student { get; set; }

        public IEnumerable<StudentInterest> studentInterests { get; set; }
        public IEnumerable<Category> allMajors { get; set; }

        [Required(ErrorMessage = "Please Select Interest(S)")]
        public IEnumerable<int> Interests { get; set; }

        public Student studentBackEdit { get; set; }
    }
}