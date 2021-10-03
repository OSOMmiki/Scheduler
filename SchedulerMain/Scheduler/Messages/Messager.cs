namespace Domain
{
    public class Messager : IMessager
    {
        public string ErrorMessage => "The configuration of the Scheduler is not correct";

        public string GetScheduledDateMessage(DateTime scheduledDate)
        {
            return $"Schedule will be used on {DateTime.Parse("dd/mm/yyyy")}";
        }
    }
}