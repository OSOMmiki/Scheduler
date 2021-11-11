using Domain;

namespace ConsoleUI
{
    internal class Application
    {
        private readonly Configuration configuration;
        public Application(Configuration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Run()
        {
            try
            {
               
                while (ConsoleMessages.AskNextDate())
                { 
                    string output = Scheduler.NextScheduleResult(configuration).Description;
                     Console.WriteLine(output);
                } 

            }
            catch (ValidatorException exc)
            {
                Console.Error.WriteLine(exc.Message);
            }

              
        }
    }
}
