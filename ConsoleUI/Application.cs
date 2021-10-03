using Domain;

namespace ConsoleUI
{
    internal class Application
    {
        private readonly IScheduler scheduler;

        public Application(Scheduler scheduler)
        {
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        public void Run()
        {
            scheduler.Initiate();

            while (ConsoleMessages.AskNextDate())
            {
                scheduler.NextScheduledDate();
            }   
        }
    }
}
