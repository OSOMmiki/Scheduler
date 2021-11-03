using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class DailyConfiguration
    {
        public bool IsOnce { get; set; }
        public TimeSpan OnceTime {get; set;}
        public int Periodicity { get; set; }
        public DailyFrecuencyEnum DailyFrecuencyEnum { get; set; }
        public TimeSpan StartingTime { get; set; }
        public TimeSpan EndingTime { get; set; }
        public int GetTotalSecondsPeriocity()
        {
            switch (DailyFrecuencyEnum)
            {
                case DailyFrecuencyEnum.Hours:
                    return Periodicity * 3600;
                case DailyFrecuencyEnum.Minutes:
                    return Periodicity * 60;
                case DailyFrecuencyEnum.Seconds:
                    return Periodicity;
                default:
                    throw new Exception("There is selected a Daily Frecuency not avalible");


            }
        }
    }
}
