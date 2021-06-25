using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class QuizPermission
    {      
        public int QuizID { get; set; }
        [ForeignKey("QuizID")]
        public virtual Quiz Quiz { get; set; }

        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual Student Student { get; set; }
    }
}