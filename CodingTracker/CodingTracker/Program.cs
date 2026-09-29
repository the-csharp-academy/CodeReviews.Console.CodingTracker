using CodingTracker.Controllers;
using CodingTracker.Repository;
using CodingTracker.UI;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace CodingTracker;

internal class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        string connectionString = config.GetConnectionString("DefaultConnection");

        DbContext context = new(connectionString);
        context.InitialTable();

        CodingSessionsRepository repository = new(context);

        var controller = new CodingSessionsController(repository);

        var userInterface = new UserInterface(controller);

        userInterface.MainMenu();
    }
}
