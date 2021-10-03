// See https://aka.ms/new-console-template for more information

using ConsoleUI;

ISchedulerFactory factory = new SchedulerFactory();

Application application = new(factory.GetScheduler());

application.Run();