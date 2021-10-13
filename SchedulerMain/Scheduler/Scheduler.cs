namespace Domain
{
    public class Scheduler : IScheduler
    {
        private DateTime? lastDateScheduled;
        private bool isFinished = false;
        private readonly IInput input;
        private readonly IConfiguration configuration;
        private readonly ILimits limits;

        public Scheduler(IInput input, IConfiguration configuration, ILimits limits)
        {
            this.input = input ?? throw new ArgumentNullException(nameof(input));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.limits = limits ?? throw new ArgumentNullException(nameof(limits));
        }

        public string NextScheduledDate()
        {
            if (isFinished)
            {
                return "There is no more avalible dates to Schedule.";
            }
            DateTime? dateScheduled = null;
            switch (configuration.FrecuencyType)
            {
                case FrecuencyEnum.Once:
                    dateScheduled = NextDateOnce();
                    break;
                case FrecuencyEnum.Recurring:
                    dateScheduled = NextDateRecurring();
                    break;
            }
            return $"Scheduled on {dateScheduled:g}";
        }
        private DateTime? NextDateOnce()
        {
            isFinished = true;
            return configuration.Date;
        }
        private DateTime? NextDateRecurring()
        {
            DateTime? output = null;
            switch (this.configuration.RecurringType)
            {
                case RecurringType.Daily:
                    output = CalculateDaily();
                    break;
            }
            return output;
        }
        private DateTime? CalculateNextDaily()
        {
            DateTime? output = null;
            if (!lastDateScheduled.HasValue)
            {
                lastDateScheduled = input.InitialDate;
            }
            else
            {
                lastDateScheduled = lastDateScheduled.Value.AddDays(configuration.RecurringDelay ?? 0);
            }
            output = lastDateScheduled;
            CheckFinished(output);
            return output.Value;
        }
        private void CheckFinished(DateTime? output)
        {
            if (limits.EndDate != null && output.Value.AddDays(configuration.RecurringDelay ?? 0) > limits.EndDate)
            {
                isFinished = true;
            }
        }

        public void ValidateConfig()
        {
            SchedulerValidator.ValidateConfigurationEnabled(configuration.IsEnabled);
            SchedulerValidator.ValidateLimits(limits.StartDate, limits.EndDate);

            switch (configuration.FrecuencyType)
            {
                case FrecuencyEnum.Once:
                    SchedulerValidator.ValidateOnceConfiguration(configuration.Date);
                    SchedulerValidator.ValidateDateBetweenLimits(limits.StartDate, limits.EndDate, configuration.Date);
                    break;
                case FrecuencyEnum.Recurring:
                    SchedulerValidator.ValidateRecurringConfiguration(configuration.RecurringType,configuration.RecurringDelay);
                    SchedulerValidator.ValidateDateBetweenLimits(limits.StartDate, limits.EndDate, input.InitialDate);
                    break;
            }

        }

    }
}