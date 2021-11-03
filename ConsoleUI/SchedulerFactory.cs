using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    //TODO cambiar la creacion de las distintas dependencias del scheduler para obtenerlas por consola.
    internal class SchedulerFactory : ISchedulerFactory
    {
        public IScheduler GetScheduler(RecurringType recurringType, FrecuencyEnum frecuencyEnum)
        { 
           
            if(frecuencyEnum == FrecuencyEnum.Once)
            { 
                Input input = this.GetInput();
                Configuration configuration = this.GetBasicConfiguration();
                Limits limits = this.GetLimits();
                return new OnceScheduler(configuration, limits);
            }
            else
            {
                if(recurringType == RecurringType.Daily)
                {
                    Input input = this.GetInput();
                    Configuration configuration = this.GetConfigurationWithDaily();
                    Limits limits = this.GetLimits();
                    return new DailyScheduler(input, configuration, limits);
                }
                else
                {
                    Input input = this.GetInput();
                    Configuration configuration = this.GetConfigurationWithWeekly();
                    Limits limits = this.GetLimits();
                    return new WeeklyScheduler(input, configuration, limits);
                }
            }
        }

        private Input GetInput()
        {
            Input input = new Input();
            return input;
        }
        
        private Configuration GetBasicConfiguration()
        {
            Configuration configuration = new Configuration()
            {
                IsEnabled = true,
                Date = DateTime.Today,
                Periodicity = 1
            };
            return configuration;
        }

        private Configuration GetConfigurationWithDaily()
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

        private Configuration GetConfigurationWithWeekly()
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
                    DailyFrecuencyEnum = DailyFrecuencyEnum.Hours,
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

        private Limits GetLimits()
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
