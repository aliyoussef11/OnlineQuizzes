using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class NewPermissionViewModel
    {
        public Quiz quiz { get; set; }

        public IEnumerable<Student> students { get; set; }

        [Required(ErrorMessage = "Please Select Student")]
        [Display(Name = "Students")]
        public IEnumerable<string> StudentsIDs { get; set; }
    }

}