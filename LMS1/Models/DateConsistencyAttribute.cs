using LMS1.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace LMS1.Models
{
    internal class DateConsistencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var tt = validationContext.ObjectInstance as CourseActivity; //Null
            if(validationContext.ObjectInstance is CourseActivity activity) 
            {
                var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
                var module = _context.CourseModule.Find(activity.ModuleId);
                if (activity.StartDate.Date > activity.EndDate.Date ||
                    activity.StartDate.Date < module.StartDate.Date ||
                    activity.EndDate.Date > module.EndDate.Date)
                {
                    return new ValidationResult("The activity must begin before it ends and be completely within the time span of the module.");
                }
            }

            return ValidationResult.Success;
        }
    }
}