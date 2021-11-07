using System;

namespace Domain
{
    public class DailyConfiguration
    {
        public bool IsOnce { get; set; }
        public TimeSpan OnceTime {get; set;}
        public int Periodicity { get; set; }
        public DailyFrecuency DailyFrecuency { get; set; }
        public TimeSpan StartingTime { get; set; }
        public TimeSpan EndingTime { get; set; }
        public int GetTotalSecondsPeriocity()
        {
            switch (DailyFrecuency)
            {
                case DailyFrecuency.Hours:
                    return Periodicity * 3600;
                case DailyFrecuency.Minutes:
                    return Periodicity * 60;
                case DailyFrecuency.Seconds:
                    return Periodicity;
                default:
                    throw new SchedulerException("There is selected a Daily Frecuency not avalible");


            }
        }
    }
}
