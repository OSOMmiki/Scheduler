using Domain;

namespace ConsoleUI
{
    internal class Application
    {
        private readonly IScheduler scheduler;

        public Application(IScheduler scheduler)
        {
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        public void Run()
        {
            try
            {
                scheduler.ValidateConfig();
                while (ConsoleMessages.AskNextDate())
                {
                     Console.WriteLine(scheduler.NextScheduledDate());
                } 

            }
            catch (ValidatorException exc)
            {
                Console.Error.WriteLine(exc.Message);
            }

              
        }
    }
}
