using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class NewQuizViewModel
    {
        public IEnumerable<Category> categories { get; set; }
        public Quiz quiz { get; set; }
    }
}