using Domain;

namespace ConsoleUI
{
    public interface ISchedulerFactory
    {
        public IScheduler GetScheduler(RecurringType recurringType, FrecuencyEnum frecuencyEnum);
    }
}