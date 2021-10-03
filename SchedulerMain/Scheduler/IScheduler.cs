namespace Domain
{
    public interface IScheduler
    {
        void Initiate();
        string NextScheduledDate();
    }
}