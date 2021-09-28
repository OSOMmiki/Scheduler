
namespace SchedulerMain
{
    public interface ILimitsScheduler
    {
        DateTime EndDate { get; set; }
        DateTime StartDate { get; set; }
    }
}