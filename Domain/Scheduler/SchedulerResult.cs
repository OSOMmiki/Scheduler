using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SchedulerResult
    {
        public DateTime ScheduledDate { get; }
        public string Description { get;}

        public SchedulerResult(DateTime scheduledDate, string description)
        {
            ScheduledDate = scheduledDate;  
            Description = description;  
        }

    }
}
