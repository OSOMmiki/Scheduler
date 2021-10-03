namespace Domain
{
    public interface IMessager
    {
        string ErrorMessage { get; }

        string GetScheduledDateMessage(DateTime scheduledDate);
    }
}