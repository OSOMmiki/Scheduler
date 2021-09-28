
namespace SchedulerMain
{
    public interface IMessagerScheduler
    {
        string ErrorMessage { get; }

        string GetScheduledDateMessage(DateTime scheduledDate);
    }
}