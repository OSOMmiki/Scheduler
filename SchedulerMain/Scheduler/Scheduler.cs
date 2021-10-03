using Domain;

namespace Domain
{
    public class Scheduler : IScheduler
    {
        private Queue<DateTime> datesScheduled;
        private bool isInitiated = false;
        private readonly IInput input;
        private readonly IConfiguration configuration;
        private readonly ILimits limits;
        private IMessager messager;

        public Scheduler(IInput input, IConfiguration configuration, ILimits limits, IMessager messager)
        {
            datesScheduled = new Queue<DateTime>();
            this.input = input ?? throw new ArgumentNullException(nameof(input));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.limits = limits ?? throw new ArgumentNullException(nameof(limits));
            this.messager = messager ?? throw new ArgumentNullException(nameof(messager));
        }

        public void Initiate()
        {
            if (validateConfiguration(input, configuration, limits))
            {
                return;
            }


            isInitiated = true;
        }
        public string NextScheduledDate()
        {
            return string.Empty;
        }

        private bool validateConfiguration(IInput input, IConfiguration configuration, ILimits limits)
        {
            return true;
        }

    }
}