using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class ConfigurationOperations
    {

        public static int GetTotalSecondsPeriocity(int periodicity, DailyFrecuency dailyFrecuency)
        {
            return dailyFrecuency switch
            {
                DailyFrecuency.Hours => periodicity * 3600,
                DailyFrecuency.Minutes => periodicity * 60,
                DailyFrecuency.Seconds => periodicity,
                _ => throw new SchedulerException("There is selected a Daily Frecuency not avalible"),
            };
        }

        public static bool CheckContainsDayOfWeek(DayWeek day, DayWeek[] daysOfWeek)
        {
            if (daysOfWeek.Contains(day))
            {
                return true;
            }
            return false;
        }
        
        public static DayWeek GetFirstDayOfWeek(DayWeek[] daysOfWeek)
        {
            return daysOfWeek.OrderBy(D => D).First();
        }

        public static DayWeek GetLastDayOfWeek(DayWeek[] daysOfWeek)
        {
            return daysOfWeek.OrderByDescending(D => D).First();
        }

        
    }
}
