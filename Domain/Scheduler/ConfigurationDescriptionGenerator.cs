using System.Text;

namespace Domain
{
    public static class ConfigurationDescriptionGenerator
    {
        public static string GenerateDescription(Configuration configuration, DateTime executionDate)
        {
            var description = new StringBuilder();
            description.Append(GetConfigurationTypeDescription(configuration));
            description.Append(GetSchedulerNextDateDescription(executionDate));
            description.Append(GetStartingDateDescription(configuration.StartDate));
            description.Append(GetEndingDateDescription(configuration.EndDate));

            return description.ToString();
        }
        private static string GetConfigurationTypeDescription(Configuration configuration)
        {
            if(configuration.ConfigurationType == ConfigurationType.Once)
            {
                return "Occurs once.";
            }
            return GetRecurringTypeDescription(configuration) + ".";
        }
        private static string GetRecurringTypeDescription(Configuration configuration)
        {
            int periodicity = configuration.RecurringType == RecurringType.Daily
                ? configuration.Periodicity.Value
                : configuration.WeeklyConfigPeriodicity;

            StringBuilder recurringTypeDescription = 
                GetRecurringPeriodicityDescription(configuration.RecurringType, periodicity);

            if(configuration.RecurringType == RecurringType.Weekly)
            {
                recurringTypeDescription.Append(GetDaysWeekDescription(configuration.WeeklyConfigActiveDays));
            }

            recurringTypeDescription.Append(GetDailyConfigurationDescription(configuration));
            return recurringTypeDescription.ToString();

        }
        private static StringBuilder GetRecurringPeriodicityDescription(RecurringType recurringType, int? periodicity)
        {
            var recurringPeriodicityDescription = new StringBuilder();

            recurringPeriodicityDescription.Append($"Occurs every");
            if (periodicity.Value > 1)
            {
                recurringPeriodicityDescription.Append($" {periodicity.Value}");
            }
            switch (recurringType)
            {
                case RecurringType.Daily:
                    recurringPeriodicityDescription.Append(" day");
                    break;
                case RecurringType.Weekly:
                    recurringPeriodicityDescription.Append(" week");
                    break;
            }
            if (periodicity.Value > 1)
            {
                recurringPeriodicityDescription.Append('s');
            }
            
            return recurringPeriodicityDescription;
        }
        private static string GetDaysWeekDescription(DayWeek[] activeDays)
        {
            var activeDaysDescription = new StringBuilder();
            activeDaysDescription.Append(" on ");

            var daysWeek = activeDays.OrderBy(D => D).ToArray();
            activeDaysDescription.Append(activeDays[0].ToString());

            for (int i = 1; i < activeDays.Length-1; i++)
            {
                activeDaysDescription.Append($", {activeDays[i]}");
            }
            if( activeDays.Length > 1)
            {
                activeDaysDescription.Append($" and {activeDays.Last()}");
            }
            return activeDaysDescription.ToString();

        }
        private static string GetDailyConfigurationDescription(Configuration configuration)
        {

            if (configuration.DailyConfType == ConfigurationType.Once)
            {
                return $" at {configuration.DailyConfOnceTime:HH:mm:ss}";
            }
            return GetDailyRecurringDescription(configuration);
        }
        private static string GetDailyRecurringDescription(Configuration configuration)
        {
            var recurringDailyPeriodicityDescription = new StringBuilder();

            recurringDailyPeriodicityDescription.Append($" every");
            if (configuration.DailyConfPeriodicity > 0)
            {
                recurringDailyPeriodicityDescription.Append($" {configuration.DailyConfPeriodicity}");
            }
            switch (configuration.DailyConfFrecuency)
            {
                case DailyFrecuency.Hours:
                    recurringDailyPeriodicityDescription.Append(" hour");
                    break;
                case DailyFrecuency.Minutes:
                    recurringDailyPeriodicityDescription.Append(" minute");
                    break;
                case DailyFrecuency.Seconds:
                    recurringDailyPeriodicityDescription.Append(" second");
                    break;
            }
            if (configuration.DailyConfPeriodicity > 1)
            {
                recurringDailyPeriodicityDescription.Append('s');
            }

            recurringDailyPeriodicityDescription.Append(GetDailyStartingEndingDescription(configuration.DailyConfStartingTime, configuration.DailyConfEndingTime));
            return recurringDailyPeriodicityDescription.ToString();
        }
        private static string GetDailyStartingEndingDescription(TimeOnly startingTime, TimeOnly endingTime)
        {
            return $" between {startingTime:HH:mm:ss} and {endingTime:HH:mm:ss}";
        }
        private static string GetSchedulerNextDateDescription(DateTime executionDate)
        {
            string schedulerNextDateDescription = $" Scheduler will be used on {executionDate:dd/MM/yyyy}";
            if(executionDate.TimeOfDay > TimeSpan.Zero)
            {
                schedulerNextDateDescription += $" at {executionDate:HH:mm:ss}";
            }

            return schedulerNextDateDescription;
        }
        private static string GetStartingDateDescription(DateOnly startingDate)
        {
            return $" starting on {startingDate:dd/MM/yyyy}";
        }
        private static string GetEndingDateDescription(DateOnly? endingDate)
        {
            string endingDateDescription =  endingDate.HasValue 
                ? $" ending on {endingDate:dd/MM/yyyy}"
                : string.Empty;

            return endingDateDescription;
        }

    }
}
