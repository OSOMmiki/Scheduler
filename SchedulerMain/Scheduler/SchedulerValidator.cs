using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class SchedulerValidator
    {
        public static void ValidateConfigurationEnabled(bool isEnabled)
        {
            if(isEnabled == false)
            {
                GenerateError("Scheduler Is Disabled");
            }
        }
        public static void ValidateRecurringConfiguration(RecurringType? recurringType, int? recurringDelay) 
        {
            if(recurringType == null)
            {
                GenerateError("You mus select a recurring type.");
            }
            
            if(recurringDelay== null || recurringDelay <= 0)
            {
                GenerateError("You must select a valid recurring dealy.");
            }
        }
        public static void ValidateOnceConfiguration(DateTime? date)
        {
            if(date == null)
            {
                GenerateError("You must introduce a valid date to schedule");
            }
        }
        public static void ValidateLimits(DateTime? startDate, DateTime? endDate)
        {
            if(startDate != null && endDate != null && endDate < startDate)
            {
                GenerateError("Start date must be earlier than end date.");
            }
        }
        public static void ValidateDateBetweenLimits(DateTime? startDate, DateTime? endDate, DateTime? date)
        {
            if((startDate >= date && startDate != null) || 
                (date >= endDate && endDate != null))
            {
                GenerateError("The date to Scheduled is out of limits range.");
            }
        }
        private static void GenerateError(string errorMsg)
        {
            throw new ValidatorException(errorMsg);
        }
        
    }
}
