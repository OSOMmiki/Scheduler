using System;


namespace Domain
{
    public class WeeklyScheduler : IScheduler
    {

        private DateTime lastDateScheduled;
        private bool isFinished = false;
        private bool isFirstSchedule = true;
        private readonly Input input;
        private readonly Configuration configuration;
        private readonly Limits limits;

        public WeeklyScheduler(Input input, Configuration configuration, Limits limits)
        {
            this.input = input ?? throw new ArgumentNullException(nameof(input));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.limits = limits ?? throw new ArgumentNullException(nameof(limits));
        }

        public string NextScheduledDate()
        {
            if (isFinished)
            {
                return "There is no more avalible dates to Schedule.";
            }
            DateTime dateScheduled = CalculateNextWeekly();
            return $"Scheduled on {dateScheduled:g}";
        }
        private DateTime CalculateNextWeekly()
        {

            if (isFirstSchedule)
            {
                lastDateScheduled = GetFirstDateTime();
                isFirstSchedule = false;
            }
            else
            {
                lastDateScheduled = CalculateNextDateTime();
            }
            CheckFinished(lastDateScheduled);
            return lastDateScheduled;
        }
        private DateTime CalculateNextDateTime()
        {

            if (configuration.DailyConfiguration == null)
            {
                return AddDaysDelay();
            }
            else
            {
                if (IsActualDayScheduleFinished())
                {
                    return AddDaysDelay().Date.Add(configuration.DailyConfiguration.StartingTime);
                }

                return AddTimeDelay();
            }
        }
        private DateTime AddDaysDelay()
        {
            if(lastDateScheduled.DayOfWeek < configuration.WeeklyConfiguration.GetLastDayOfWeek())
            {
                return GetNextDateInWeeklyConfiguration(lastDateScheduled.AddDays(1));
            }
            else
            {
                DateTime output = AddWeeksDelay();
                return GetNextDateInWeeklyConfiguration(output);
            }
        }
        private DateTime AddWeeksDelay()
        {
            DateTime output = lastDateScheduled.AddDays(DayOfWeek.Saturday - lastDateScheduled.DayOfWeek + 1);
            return output.AddDays(7 * (configuration.WeeklyConfiguration.Periodicity-1));
        }

        private DateTime AddTimeDelay()
        {
            switch (configuration.DailyConfiguration.DailyFrecuency)
            {
                case DailyFrecuencyEnum.Hours:
                    return lastDateScheduled.AddHours(configuration.DailyConfiguration.Periodicity);
                case DailyFrecuencyEnum.Minutes:
                    return lastDateScheduled.AddMinutes(configuration.DailyConfiguration.Periodicity);
                case DailyFrecuencyEnum.Seconds:
                    return lastDateScheduled.AddSeconds(configuration.DailyConfiguration.Periodicity);
                default:
                    throw new Exception("There is selected a Daily Frecuency not avalible");
            }
        }

        private DateTime GetFirstDateTime()
        {
            DateTime output = input.InitialDate;
            if (configuration.WeeklyConfiguration != null)
            {
                output = GetNextDateInWeeklyConfiguration(output);
            }
            if (configuration.DailyConfiguration != null)
            {
                output = output.Date.Add(configuration.DailyConfiguration.StartingTime);
            }
            return output;
        }

        private DateTime GetNextDateInWeeklyConfiguration(DateTime date)
        {
            DateTime output = date;
            while (!configuration.WeeklyConfiguration.CheckContainsDayOfWeek(output.DayOfWeek))
            {
                output = output.AddDays(1);
            }
            return output;
        }

        private bool IsActualDayScheduleFinished()
        {
            TimeSpan remainingTimeDay =
                configuration.DailyConfiguration.EndingTime - lastDateScheduled.TimeOfDay;

            if (remainingTimeDay.TotalSeconds > 0
                && remainingTimeDay.TotalSeconds > configuration.DailyConfiguration.GetTotalSecondsPeriocity())
            {
                return false;
            }
            return true;
        }
        private void CheckFinished(DateTime output)
        {
            if (limits.EndDate.HasValue && CalculateNextDateTime() > limits.EndDate.Value)
            {
                isFinished = true;
            }
        }

        public void ValidateConfig()
        {
            SchedulerValidator.ValidateConfigurationEnabled(configuration.IsEnabled);
            SchedulerValidator.ValidateLimits(limits.StartDate, limits.EndDate);
            SchedulerValidator.ValidateRecurringConfiguration(configuration.Periodicity);
            SchedulerValidator.ValidateDateBetweenLimits(limits.StartDate, limits.EndDate, input.InitialDate);

            if (configuration.DailyConfiguration != null)
            {
                if (configuration.DailyConfiguration.IsOnce)
                {
                    SchedulerValidator.ValidateOnceTimeNotNull(configuration.DailyConfiguration.OnceTime);
                }
                else
                {
                    SchedulerValidator.ValidateStartingEndingDaily(configuration.DailyConfiguration.StartingTime, configuration.DailyConfiguration.EndingTime);
                }
            }

        }
    }
}
