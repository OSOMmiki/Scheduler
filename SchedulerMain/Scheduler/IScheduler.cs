namespace Domain
{
    public interface IScheduler
    {
        void ValidateConfig();
        string NextScheduledDate();
    }
}