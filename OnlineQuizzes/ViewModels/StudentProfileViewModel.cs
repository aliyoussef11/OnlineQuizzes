using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class StudentProfileViewModel
    {
        public string StudentEmail { get; set; }
        public Student student { get; set; }
        public IEnumerable<Category> studentInterests { get; set; }
    }
}