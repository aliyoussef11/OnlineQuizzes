using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineQuizzes.Models;

namespace OnlineQuizzes.ViewModels
{
    public class QuizPermissionViewModel
    {
        public int QuizId { get; set; }
        public Student student { get; set; }
        public IEnumerable<QuizPermission> QuizPermissions { get; set; }
    }
}