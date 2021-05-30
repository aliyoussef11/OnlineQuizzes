using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class QuestionType
    {
        [Key]
        public int TypeID { get; set; }

        [Display(Name= "Type Of The Question")]
        public string Type { get; set; }
    }
}