namespace ConsoleUI
{
    public class Program
    {
    static void Main(string[] args)
    {
            ISchedulerFactory factory = new SchedulerFactory();

            Application application = new Application(factory.GetScheduler(Domain.RecurringType.Weekly, Domain.Frecuency.Recurring));

            application.Run();
        }
    }
}


