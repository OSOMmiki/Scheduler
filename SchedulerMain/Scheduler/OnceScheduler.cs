using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class OnceScheduler : IScheduler
    {
        private readonly Configuration configuration;
        private readonly Limits limits;
        private bool isFinished = false;

        public OnceScheduler( Configuration configuration, Limits limits)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.limits = limits ?? throw new ArgumentNullException(nameof(limits));
        }

        public string NextScheduledDate()
        {
            if (isFinished)
            {
                return "There is no more avalible dates to Schedule.";
            }

            return $"Scheduled on {NextDateOnce():g}";
        }

        public void ValidateConfig()
        {
            SchedulerValidator.ValidateConfigurationEnabled(configuration.IsEnabled);
            SchedulerValidator.ValidateLimits(limits.StartDate, limits.EndDate);
            SchedulerValidator.ValidateOnceConfiguration(configuration.Date);
            SchedulerValidator.ValidateDateBetweenLimits(limits.StartDate, limits.EndDate, configuration.Date);
        }

        private DateTime? NextDateOnce()
        {
            isFinished = true;
            return configuration.Date;
        }
    }
}
