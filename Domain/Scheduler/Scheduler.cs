namespace Domain
{
    public class Scheduler 
    {
        public static string NextOnceDate(Configuration configuration)
        {
            SchedulerValidator.ValidateOnceConfiguration(configuration.OnceDate);

            return $"Scheduled on {configuration.OnceDate:g}";
        }
    }
}
