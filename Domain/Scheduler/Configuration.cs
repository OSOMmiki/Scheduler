namespace Domain
{
    public class Configuration
    {

        #region Input
        public DateOnly InputDate => DateOnly.FromDateTime(DateTime.Now);

        #endregion

        #region CommonConfiguration
        public bool Enabled { get; set; }
        public DateTime? OnceDate { get; set; }
        public int? Periodicity { get; set; }
        public ConfigurationType ConfigurationType{get; set; }
        public RecurringType RecurringType { get; set; }
        #endregion

        #region Limits
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        #endregion

        #region DailyConfiguration
        public ConfigurationType DailyConfType { get; set; }
        public TimeOnly? DailyConfOnceTime { get; set; }
        public int DailyConfPeriodicity { get; set; }
        public DailyFrecuency DailyConfFrecuency { get; set; }
        public TimeOnly DailyConfStartingTime { get; set; }
        public TimeOnly DailyConfEndingTime { get; set; }
        #endregion

        #region WeeklyConfiguration
        public int WeeklyConfigPeriodicity { get; set; }
        public DayWeek[] WeeklyConfigActiveDays { get; set; } = new DayWeek[7];
        #endregion
    }
}