using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace loginregister.Models
{
    public class Activity : BaseEntity
    {
        public int ActivityId { get; set; }
        public string Title { get; set; }


        [Required]
        [ValidateDate]
        public DateTime ActivityDate { get; set; }
        public DateTime ActivityTime { get; set; }
        public int Duration { get; set; }
        public string DurationType { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByFirstName { get; set; }
        public string Description { get; set; }
        public List<Participant> Participants { get; set; }
        public Activity()
        {
            Participants = new List<Participant>();
        }     
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 

         public class ValidateDate : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                DateTime Today = DateTime.Now;
                if (value is DateTime)
                {
                    DateTime InputDate = (DateTime)value;
                    if (InputDate > Today)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult("Cannot have your activity in the past");
                    }
                }
                return new ValidationResult("Please enter valid date");
            }

        }
    }

    
}