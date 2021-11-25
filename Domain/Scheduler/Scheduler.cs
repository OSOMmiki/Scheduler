namespace Domain
{
    public class Scheduler
    {
        public static SchedulerResult[] ScheduleMulitple(Configuration configuration, int numberOfSchedules)
        {
            var result = new SchedulerResult[numberOfSchedules];
            for (int i = 0; i < numberOfSchedules; i++)
            {
                result[i] = NextScheduleResult(configuration);
                configuration.CurrentDate = result[i].ScheduledDate;
            }
            return result;
        }
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
            ConfigurationValidator.ValidateDateBetweenLimits(configuration.StartDate, configuration.EndDate, configuration.CurrentDate);
            var recurringDate = GetNextDateRecurring(configuration);

            if (configuration.DailyConfType == ConfigurationType.Once)
            {
                ConfigurationValidator.ValidateOnceTimeNotNull(configuration.DailyConfOnceTime);
                return recurringDate.Add(configuration.DailyConfOnceTime.Value.ToTimeSpan());
            }
            else
            {
                return recurringDate.Add(GetNextDailyRecurring(configuration));
            }
            
        }

        

        #region NextDateRecurring
        private static DateTime GetNextDateRecurring(Configuration configuration)
        {
            if (!configuration.IsDailySchedulingFinished())
            {
                return configuration.CurrentDate.Date;
            }
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
            ConfigurationValidator.ValidateWeeklyConfiguration(configuration.WeeklyConfigActiveDays);
            DayWeek currentDay = configuration.CurrentDate.GetDayWeek();
            DayWeek[] activeDays = configuration.WeeklyConfigActiveDays;
            return configuration.CurrentDate.Date.AddDays(GetNumberOfDaysToNextActiveDay(currentDay, activeDays, configuration.WeeklyConfigPeriodicity));
        }

        private static double GetNumberOfDaysToNextActiveDay(DayWeek currentDay, DayWeek[] activeDays, int weeklyPeriodicity)
        {
            DayWeek lastDayOfWeek = ConfigurationOperations.GetLastDayOfWeek(activeDays);
            if (currentDay >= lastDayOfWeek)
            {
                double daysOffset = (double)ConfigurationOperations.GetFirstDayOfWeek(activeDays);
                daysOffset += DayWeek.Sunday - currentDay;
                if (currentDay == lastDayOfWeek)
                {
                    daysOffset += (weeklyPeriodicity - 1) * 7;
                }
                return daysOffset;
            }
            var nextDayWeek = activeDays.OrderBy(D => D).Where(D => D > currentDay).First();

            return nextDayWeek - currentDay;
        }
        #endregion

        #region NextDailyRecurring
        private static TimeSpan GetNextDailyRecurring(Configuration configuration)
        {
            ConfigurationValidator.ValidateStartingEndingDaily(configuration.DailyConfStartingTime, configuration.DailyConfEndingTime);
            var iteration = GetNextDailyIteraction(configuration);
            if(iteration == -1)
            {
                return configuration.DailyConfStartingTime.ToTimeSpan();
            }
            else
            {
                double period = (double)ConfigurationOperations.GetTotalSecondsPeriocity(configuration.DailyConfPeriodicity, configuration.DailyConfFrecuency);
                return configuration.DailyConfStartingTime.Add(TimeSpan.FromSeconds(period * iteration)).ToTimeSpan();
            }
        }
        private static int GetNextDailyIteraction(Configuration configuration)
        {
            if (configuration.IsDailySchedulingFinished())
            {
                return -1;
            }
            if(configuration.CurrentDate.GetTimeOnly() < configuration.DailyConfStartingTime)
            {
                return 0;
            }
            int currentTimeSecs = configuration.CurrentDate.TimeOfDay.TotalCompleteSeconds();
            int startingSecs = configuration.DailyConfStartingTime.ToTimeSpan().TotalCompleteSeconds();
            int dailyperiod = ConfigurationOperations.GetTotalSecondsPeriocity(configuration.DailyConfPeriodicity, configuration.DailyConfFrecuency);
            return ConfigurationOperations.GetCurrentDailyIteration(currentTimeSecs, startingSecs, dailyperiod)+1;
            
        }

        #endregion
    }
}
