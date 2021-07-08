using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class EditTrainerProfileViewModel
    {
        public Trainer trainer { get; set; }

        public IEnumerable<TrainerMajors> trainerMajors { get; set; }
        public IEnumerable<Category> allMajors { get; set; }

        [Required(ErrorMessage = "Please Select Major")]
        public IEnumerable<int> Majors { get; set; }

        public Trainer trainerBackEdit { get; set; }
    }
}