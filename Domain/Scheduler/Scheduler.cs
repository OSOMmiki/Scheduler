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
            throw new NotImplementedException(); //TODO
        }

        private static bool IsInDaySchedulePossible(Configuration configuration)
        {
            int currentTimeSecs = configuration.CurrentDate.TimeOfDay.TotalCompleteSeconds();
            int startingSecs = configuration.DailyConfStartingTime.ToTimeSpan().TotalCompleteSeconds(); 
            int endingSecs = configuration.DailyConfEndingTime.ToTimeSpan().TotalCompleteSeconds();
            int dailyperiod = ConfigurationOperations.GetTotalSecondsPeriocity(configuration.DailyConfPeriodicity, configuration.DailyConfFrecuency);

            return ((currentTimeSecs + dailyperiod) <= endingSecs); 
        }
    }
}
