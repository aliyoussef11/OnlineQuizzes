using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class Category
    {
        [Key]
        [Display(Name ="Major Name")]
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }
    }
}