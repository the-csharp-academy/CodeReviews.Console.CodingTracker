using CodingTracker;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        DatabaseManager.SetConfiguration(config);
        DatabaseManager.Start();
        DatabaseManager.Initialize();

        UIController ui = new(config);
        ui.MainMenu();
    }
}