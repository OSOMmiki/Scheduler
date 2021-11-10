namespace Domain
{
    public class Scheduler 
    {

        public static SchedulerResult ScheduleNextDate(Configuration configuration)
        {
            ConfigurationValidator.ValidateConfigurationEnabled(configuration.Enabled);

            switch (GetExecutionType(configuration))
            {
                case ExecutionType.Once:
                    return GetOnceResult(configuration);
                default:
                    return new SchedulerResult(DateTime.MinValue, "Error");
            }
        }
        private static ExecutionType GetExecutionType(Configuration configuration)
        {
            if(configuration.ConfigurationType == ConfigurationType.Once)
            {
                return ExecutionType.Once;
            }
            switch (configuration.RecurringType)
            {
                case RecurringType.Daily:
                    return ExecutionType.DailyRecurring;
                case RecurringType.Weekly:
                    return ExecutionType.WeeklyRecurring;
            }
            throw new SchedulerException("Invalid Configuration");
        }

        private static SchedulerResult GetOnceResult(Configuration configuration)
        {
            ConfigurationValidator.ValidateOnceConfiguration(configuration.OnceDate);

            DateTime schedulerDate = configuration.OnceDate.Value;
            string resultDescription = ConfigurationDescriptionGenerator.GenerateDescription(configuration);
            return new SchedulerResult(schedulerDate, resultDescription);
        }
    }
}
