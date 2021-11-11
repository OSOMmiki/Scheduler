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
            return GetRecurringTypeDescription(configuration);
        }
        private static string GetRecurringTypeDescription(Configuration configuration)
        {
            StringBuilder recurringTypeDescription = 
                GetRecurringPeriodicityDescription(configuration.RecurringType, configuration.Periodicity);

            if(configuration.RecurringType == RecurringType.Weekly)
            {
                recurringTypeDescription.Append(GetDaysWeekDescription(configuration.WeeklyConfigActiveDays));
            }

            recurringTypeDescription.Append(GetDailyConfigurationDescription(configuration));
            return recurringTypeDescription.ToString();

        }
        private static string GetDailyConfigurationDescription(Configuration configuration)
        {
            throw new NotImplementedException(); //TODO
        }
        private static string GetDaysWeekDescription(DayWeek[] activeDays)
        {
            var activeDaysDescription = new StringBuilder();
            activeDaysDescription.Append(" on");

            var daysWeek = activeDays.OrderBy(D => D).ToArray();
            activeDaysDescription.Append(activeDays[0].ToString());

            for (int i = 1; i < activeDays.Length-1; i++)
            {
                activeDaysDescription.Append($", {activeDays[i]}");
            }

            activeDaysDescription.Append($" and {activeDays.Last()}");
            return activeDaysDescription.ToString();

        }
        private static StringBuilder GetRecurringPeriodicityDescription(RecurringType recurringType, int? periodicity)
        {
            var recurringPeriodicityDescription = new StringBuilder();

            recurringPeriodicityDescription.Append($"Occurs every");
            if (periodicity > 0)
            {
                recurringPeriodicityDescription.Append($" {periodicity}");
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
            if (periodicity > 0)
            {
                recurringPeriodicityDescription.Append('s');
            }
            
            return recurringPeriodicityDescription;
        }
        private static string GetSchedulerNextDateDescription(DateTime executionDate)
        {
            string schedulerNextDateDescription = $" Scheduler will be used on {executionDate:d}";
            if(executionDate.TimeOfDay > TimeSpan.Zero)
            {
                schedulerNextDateDescription += $" at {executionDate:t}";
            }

            return schedulerNextDateDescription;
        }
        private static string GetStartingDateDescription(DateOnly startingDate)
        {
            return $" starting on {startingDate:d}";
        }
        private static string GetEndingDateDescription(DateOnly? endingDate)
        {
            string endingDateDescription =  endingDate.HasValue 
                ? $" ending on {endingDate:d}"
                : string.Empty;

            return endingDateDescription;
        }

    }
}
