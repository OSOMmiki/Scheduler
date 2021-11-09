﻿namespace Domain
{
    public static class SchedulerValidator
    {
        public static void ValidateConfigurationEnabled(bool isEnabled)
        {
            if(!isEnabled) 
            {
                GenerateError("Scheduler Is Disabled");
            }
        }
        public static void ValidateRecurringConfiguration( int? periodicity)
        { 
            if(periodicity == null || periodicity <= 0)
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
            if((startDate > date && startDate != null) || 
                (date > endDate && endDate != null))
            {
                GenerateError("The date to Scheduled is out of limits range.");
            }
        }

        public static void ValidateOnceTimeNotNull(TimeOnly? onceTime)
        {
            if(onceTime == null)
            {
                GenerateError("The Once at time can't be null.");
            }
        }

        public static void ValidateStartingEndingDaily(TimeOnly starting, TimeOnly ending)
        {
            if(ending < starting)
            {
                GenerateError("The ending time for daily configurations cant be before the starting time.");
            }
        }
        public static void ValidateWeeklyConfiguration(DayWeek[] daysOfWeek)
        {
            if(daysOfWeek.Length<=0)
            {
                GenerateError("There must be at least a day of the week selected.");
            }
        }
        private static void GenerateError(string errorMsg)
        {
            throw new ValidatorException(errorMsg);
        }
        
    }
}
