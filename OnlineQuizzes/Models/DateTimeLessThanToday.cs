using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class DateTimeLessThanToday : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var quiz = (Quiz)validationContext.ObjectInstance;
            DateTime now = DateTime.Now;

            if (quiz.TimeOfQuiz >= now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date & Time Should Be more than Today's Date & Time!");
            }
        }
    }
}