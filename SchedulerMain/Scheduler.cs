namespace SchedulerMain
{
    public class Scheduler
    {
        private DateOnly inputDate;
        private FrecuencyEnum frecuency;
        public void Launch()
        {
            inputDate = DateOnly.FromDateTime(DateTime.Now);
        }
        public void Configurate()
        {
            frecuency = new FrecuencyEnumSelector().GetFrecuencyEnum();
        }
        public void Run()
        {

        }

    }
}