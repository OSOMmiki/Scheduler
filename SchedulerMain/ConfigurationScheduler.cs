namespace SchedulerMain
{
    public class ConfigurationScheduler : IConfigurationScheduler
    {
        public bool IsEnabled { get; set; }
        public FrecuencyEnum FrecuencyType { get; set; }
        public DateTime Date { get; set; }
        public RecurringType RecurringType { get; set; }
        public int RecurringDelay { get; set; }
    }
}