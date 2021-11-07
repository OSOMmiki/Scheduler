using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class DailyScheduler :IScheduler
    {
        private DateTime lastDateScheduled;
        private bool isFinished = false;
        private bool isFirstSchedule = true;
        private readonly Input input;
        private readonly Configuration configuration;
        private readonly Limits limits;

        public DailyScheduler(Input input, Configuration configuration, Limits limits)
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
            DateTime dateScheduled = CalculateNextDaily();
            return $"Scheduled on {dateScheduled:g}";
        }
        private DateTime CalculateNextDaily()
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
            CheckFinished();
            return lastDateScheduled;
        }
        private DateTime CalculateNextDateTime()
        {
            
            if(configuration.DailyConfiguration == null)
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
            return lastDateScheduled.AddDays(configuration.Periodicity ?? 0);
        }

        private DateTime AddTimeDelay()
        {
            switch (configuration.DailyConfiguration.DailyFrecuency)
            {
                case DailyFrecuency.Hours:
                    return lastDateScheduled.AddHours(configuration.DailyConfiguration.Periodicity);
                case DailyFrecuency.Minutes:
                    return lastDateScheduled.AddMinutes(configuration.DailyConfiguration.Periodicity);
                case DailyFrecuency.Seconds:
                    return lastDateScheduled.AddSeconds(configuration.DailyConfiguration.Periodicity);
                default:
                    throw new SchedulerException("There is selected a Daily Frecuency not avalible");
            }
        }

        private DateTime GetFirstDateTime()
        {
            DateTime output = input.InitialDate;
            if (configuration.DailyConfiguration != null)
            {
                output = output.Add(configuration.DailyConfiguration.StartingTime);
            }
            return output;
        }

        private bool IsActualDayScheduleFinished()
        {
            TimeSpan remainingTimeDay =
                configuration.DailyConfiguration.EndingTime - lastDateScheduled.TimeOfDay;

            if(remainingTimeDay.TotalSeconds > 0 
                && remainingTimeDay.TotalSeconds > configuration.DailyConfiguration.GetTotalSecondsPeriocity())
            {
                return false;
            }
            return true;
        }
        private void CheckFinished()
        {
            if (limits.EndDate != null && CalculateNextDateTime() > limits.EndDate)
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
            if (configuration.DailyConfiguration!= null)
            {
                if(configuration.DailyConfiguration.IsOnce)
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
