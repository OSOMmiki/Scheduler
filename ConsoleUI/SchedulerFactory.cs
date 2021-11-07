using Domain;
using System;
namespace ConsoleUI
{
    internal class SchedulerFactory : ISchedulerFactory
    {
        public IScheduler GetScheduler(RecurringType recurringType, Frecuency frecuencyEnum)
        { 
           
            if(frecuencyEnum == Frecuency.Once)
            { 
                Configuration configuration = GetBasicConfiguration();
                Limits limits = GetLimits();
                return new OnceScheduler(configuration, limits);
            }
            else
            {
                if(recurringType == RecurringType.Daily)
                {
                    Input input = GetInput();
                    Configuration configuration = GetConfigurationWithDaily();
                    Limits limits = GetLimits();
                    return new DailyScheduler(input, configuration, limits);
                }
                else
                {
                    Input input = GetInput();
                    Configuration configuration = GetConfigurationWithWeekly();
                    Limits limits = GetLimits();
                    return new WeeklyScheduler(input, configuration, limits);
                }
            }
        }

        private static Input GetInput()
        { 
            return new Input();
        }
        
        private static Configuration GetBasicConfiguration()
        {
            Configuration configuration = new Configuration()
            {
                IsEnabled = true,
                Date = DateTime.Today,
                Periodicity = 1
            };
            return configuration;
        }

        private static Configuration GetConfigurationWithDaily()
        {
            Configuration configuration = new Configuration()
            {
                IsEnabled = true,
                Date = DateTime.Today,
                Periodicity = 2,
                DailyConfiguration = new DailyConfiguration()
                {
                    IsOnce = true,
                    OnceTime = TimeSpan.FromHours(14)
                }
            };
            return configuration;
        }

        private static Configuration GetConfigurationWithWeekly()
        {
            Configuration configuration = new Configuration()
            {
                IsEnabled = true,
                Date = DateTime.Today,
                Periodicity = 2,
                DailyConfiguration = new DailyConfiguration()
                {
                    IsOnce = false,
                    Periodicity = 2,
                    DailyFrecuency = DailyFrecuency.Hours,
                    StartingTime = TimeSpan.FromHours(10),
                    EndingTime = TimeSpan.FromHours(16)
                },
                WeeklyConfiguration = new WeeklyConfiguration()
                {
                    Periodicity = 4,
                    ActiveDays = new DayOfWeek[3] { DayOfWeek.Monday, DayOfWeek.Wednesday , DayOfWeek.Friday}
                }

            };
            return configuration;
        }

        private static Limits GetLimits()
        {
            Limits limits = new Limits()
            {
                StartDate = null,
                EndDate = null
            };
            return limits;
        }
    }
}
