using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class Trainer
    {
        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser User { get; set; }


        [Required]
        [Display(Name ="Trainer Name")]
        public string TrainerName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public int PhoneNumber { get; set; }
    }
}