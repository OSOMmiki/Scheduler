// See https://aka.ms/new-console-template for more information

using ConsoleUI;

ISchedulerFactory factory = new SchedulerFactory();

Application application = new(factory.GetScheduler(Domain.RecurringType.Weekly, Domain.FrecuencyEnum.Recurring));

application.Run();