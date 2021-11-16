namespace Domain
{
    public class Scheduler
    {
        public static SchedulerResult NextScheduleResult(Configuration configuration)
        {
            DateTime scheduledDate = ScheduleNextDate(configuration);
            string resultDescription = ConfigurationDescriptionGenerator.GenerateDescription(configuration, scheduledDate);
            return new SchedulerResult(scheduledDate, resultDescription);
        }
        private static DateTime ScheduleNextDate(Configuration configuration)
        {
            ConfigurationValidator.ValidateConfigurationEnabled(configuration.Enabled);

            if (configuration.ConfigurationType == ConfigurationType.Once)
            {
                return GetOnceResult(configuration);
            }
            return GetRecurringResult(configuration);
        }
        private static DateTime GetOnceResult(Configuration configuration)
        {
            ConfigurationValidator.ValidateOnceConfiguration(configuration.OnceDate);

            return configuration.OnceDate.Value;
        }
        private static DateTime GetRecurringResult(Configuration configuration)
        {

            if(configuration.DailyConfType == ConfigurationType.Once)
            {
                ConfigurationValidator.ValidateOnceTimeNotNull(configuration.DailyConfOnceTime);
                return GetNextDateRecurring(configuration).Add(configuration.DailyConfOnceTime.Value.ToTimeSpan());
            }
            var recurringDate = GetNextDateRecurring(configuration);
            //recurringDate = recurringDate.Add(GetNextDailyExecution(configuration));
            return recurringDate;
        }

        #region NextDateRecurring
        private static DateTime GetNextDateRecurring(Configuration configuration)
        {
            if(configuration.RecurringType  == RecurringType.Daily)
            {
                return GetNextDateRecurringDaily(configuration.CurrentDate, configuration.Periodicity);
            }
            else
            {
                return GetNextDateRecurringWeekly(configuration);
            }   
        }
        private static DateTime GetNextDateRecurringDaily(DateTime currentDate, int? periodicity)
        {
            ConfigurationValidator.ValidateRecurringConfiguration( periodicity);
            return currentDate.Date.AddDays((double)periodicity);
        }
        private static DateTime GetNextDateRecurringWeekly(Configuration configuration)
        {
            throw new NotImplementedException();
        }
        #endregion

        private static int GetNextDailyIteraction(Configuration configuration)
        {
            int currentTimeSecs = configuration.CurrentDate.TimeOfDay.TotalCompleteSeconds();
            int startingSecs = configuration.DailyConfStartingTime.ToTimeSpan().TotalCompleteSeconds(); 
            int endingSecs = configuration.DailyConfEndingTime.ToTimeSpan().TotalCompleteSeconds();
            int dailyperiod = ConfigurationOperations.GetTotalSecondsPeriocity(configuration.DailyConfPeriodicity, configuration.DailyConfFrecuency);

            if((endingSecs - startingSecs) < dailyperiod)
            {
                return -1;
            }
            else
            {
                return ConfigurationOperations.GetCurrentDailyIteration(currentTimeSecs, startingSecs, dailyperiod) + 1;
            }
        }
    }
}
