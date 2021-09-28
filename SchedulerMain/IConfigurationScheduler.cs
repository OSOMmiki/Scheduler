
namespace SchedulerMain
{
    public interface IConfigurationScheduler
    {
        DateTime Date { get; set; }
        FrecuencyEnum FrecuencyType { get; set; }
        bool IsEnabled { get; set; }
        int RecurringDelay { get; set; }
        RecurringType RecurringType { get; set; }
    }
}