﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.ViewModels;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class AttemptedStudentListHelperViewModel
    {
        public IEnumerable<Students_sAttemptedQuizViewModel> students_S { get; set; }
    }
}