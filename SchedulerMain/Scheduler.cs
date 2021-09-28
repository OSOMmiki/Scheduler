namespace SchedulerMain
{
    public class Scheduler
    {
        private Queue<DateTime> datesScheduled;
        private bool isInitiated = false;
        private readonly IInputScheduler input;
        private readonly IConfigurationScheduler configuration;
        private readonly ILimitsScheduler limits;
        private readonly IMessagerScheduler messager;

        public Scheduler(IInputScheduler input, IConfigurationScheduler configuration, ILimitsScheduler limits, IMessagerScheduler messager)
        {
            datesScheduled = new Queue<DateTime>();
            this.input = input;
            this.configuration = configuration;
            this.limits = limits;
            this.messager = messager;
        }
        public void Initiate()
        {
            if (validateConfiguration(input, configuration, limits))
            {
                return;
            }


            isInitiated = true;
        }
        public string GetNextDateScheduled()
        {
            return string.Empty;
        }

        private bool validateConfiguration(IInputScheduler input,IConfigurationScheduler configuration,ILimitsScheduler limits)
        {
            return true;
        }

    }
}