using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineQuizzes.Models
{
    public class CorrectAnswerChecking : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Answers = (MCQAnswers)validationContext.ObjectInstance;

            if (Answers.FirstPossibleAnswer != Answers.CorrectAnswer && Answers.SecondPossibleAnswer != Answers.CorrectAnswer
                && Answers.ThirdPossibleAnswer != Answers.CorrectAnswer && Answers.FourthPossibleAnswer != Answers.CorrectAnswer)
            {
                return new ValidationResult("Correct Answer Should Be Same OF One Possible Answer!");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
