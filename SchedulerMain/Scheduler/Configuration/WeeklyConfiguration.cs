﻿using System;
using System.Linq;

namespace Domain
{
    public class WeeklyConfiguration
    {
        public int Periodicity { get; set; }
        public DayOfWeek[] ActiveDays { get; set; }

        public bool CheckContainsDayOfWeek(DayOfWeek day)
        {
            if (ActiveDays.Contains(day))
            {
                return true;
            }
            return false;
        }

        public DayOfWeek GetLastDayOfWeek()
        {
            return ActiveDays.OrderByDescending(D => D).First();
        }

        public DayOfWeek GetFirstDayOfWeek()
        {
            return ActiveDays.OrderBy(D => D).First();
        }
    }
}
