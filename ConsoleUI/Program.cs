using ConsoleUI;
using Domain;

var configuration = new Configuration()
{
    OnceDate = DateTime.Now
};


var application = new Application(configuration);

application.Run();