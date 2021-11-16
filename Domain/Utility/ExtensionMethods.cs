namespace Domain
{
    public static class ExtensionMethods
    {
        public static int TotalCompleteSeconds(this TimeSpan time)
        {
            int seconds = time.Hours * 3600;
            seconds += time.Minutes * 60;
            seconds += time.Seconds;
            return seconds;
        }

        public static DayWeek GetDayWeek(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:  
                    return DayWeek.Sunday;
                case DayOfWeek.Monday:
                    return DayWeek.Monday;  
                case DayOfWeek.Tuesday: 
                    return DayWeek.Tuesday;
                case DayOfWeek.Wednesday:
                    return DayWeek.Wednesday;
                case DayOfWeek.Thursday:
                    return DayWeek.Thursday;
                case DayOfWeek.Friday:
                    return DayWeek.Friday;
                case DayOfWeek.Saturday:
                    return DayWeek.Saturday;
                default:
                    throw new SchedulerException("Missing day of Week");
            }
        }
    }
}
