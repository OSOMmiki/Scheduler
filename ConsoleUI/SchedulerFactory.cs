using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    //TODO cambiar la creacion de las distintas dependencias del scheduler para obtenerlas por consola.
    internal class SchedulerFactory : ISchedulerFactory
    {
        public IScheduler GetScheduler()
        {
            IInput input = this.GetInput();
            IConfiguration configuration = this.GetConfiguration();
            ILimits limits = this.GetLimits();
            return new Scheduler(input,configuration,limits);
        }

        private IInput GetInput()
        {
            IInput input = new Input();
            return input;
        }
        
        private IConfiguration GetConfiguration()
        {
            IConfiguration configuration = new Configuration()
            {
                FrecuencyType = FrecuencyEnum.Recurring,
                IsEnabled = true,
                Date = DateTime.Today,
                RecurringType = RecurringType.Daily,
                RecurringDelay = 1
            };
            return configuration;
        }

        private ILimits GetLimits()
        {
            ILimits limits = new Limits()
            {
                StartDate = null,
                EndDate = DateTime.Today.AddDays(5)
            };
            return limits;
        }
    }
}
