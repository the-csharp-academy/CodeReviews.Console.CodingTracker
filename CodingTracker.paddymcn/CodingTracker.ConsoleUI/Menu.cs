using Spectre.Console;
using CodingTracker.Core;
using CodingTracker.Core.DataModel;
using CodingTracker.Data;
using Microsoft.Extensions.Configuration;

namespace CodingTracker.Console;
public class Menu
{
    private readonly UserInput _userInput = new();
    static IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();
    static string connectionString = config.GetConnectionString("CodingTracker")
                              ?? throw new InvalidOperationException("Connection string 'CodingTracker' not found in appsettings.json.");
    ICodingSessionRepository repo = new SqliteCodingSessionRepository( connectionString);
    public void Run()
    {
        var isAppRunning = true;
        while (isAppRunning)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                    .Title("What would you like to do?")
                    .UseConverter(ToDisplayText)
                    .AddChoices(Enum.GetValues<MenuOptions>())); 

            switch (option)
            {
                case MenuOptions.AddCodingSession:
                    repo.EnsureCreated();
                    var sessionNew = new CodingSession
                    {
                        StartTime = _userInput.GetDateTime("Enter start time"),
                        EndTime = _userInput.GetDateTime("Enter end time"),
                        Description = _userInput.GetDescription("Enter description for the coding session")
                    };
                   repo.Add(sessionNew);
                    break;
                case MenuOptions.DeleteCodingSession:
                    repo.EnsureCreated();
                    repo.EnsureCreated();
                    int idDelete = _userInput.GetPositiveInt("Enter session Id");
                    var sessionDelete = repo.GetById(idDelete);
                    if (sessionDelete is null)
                    {
                        AnsiConsole.MarkupLine($"[red]No session found with Id {idDelete}.[/]");
                        break;   
                    }
                    repo.Delete(idDelete);
                    break;
                case MenuOptions.UpdateCodingSession:
                {
                    int idUpdate = _userInput.GetPositiveInt("Enter id of the coding session");
                    var sessionUpdate = repo.GetById(idUpdate);
                    if (sessionUpdate is null)
                    {
                        AnsiConsole.MarkupLine($"[red]No session found with Id {idUpdate}.[/]");
                        break;
                    }
                    sessionUpdate.StartTime = _userInput.GetDateTime("Enter start time");
                    sessionUpdate.EndTime = _userInput.GetDateTime("Enter end time");
                    sessionUpdate.Description = _userInput.GetDescription("Enter description for the coding session");
                    repo.Update(sessionUpdate);
                    AnsiConsole.MarkupLine("[green]Session updated.[/]");
                    break;
                }
                    break;
                case MenuOptions.ViewCodingSession:
                    repo.EnsureCreated();
                    int idSession = _userInput.GetPositiveInt("Enter session Id");
                    var sessionById = repo.GetById(idSession);
                    if (sessionById is null)
                    {
                        AnsiConsole.MarkupLine($"[red]No session found with Id {idSession}.[/]");
                        break;   
                    }
                    System.Console.WriteLine($"{sessionById.Id} - {sessionById.Duration} -  {sessionById.Description}");
                break;
                case MenuOptions.ViewAllCodingSessions:
                    repo.EnsureCreated();
                    List<CodingSession> allSessions= repo.GetAll();
                    foreach (var codingSessions in allSessions)
                    {
                        System.Console.WriteLine($"{codingSessions.Id} - {codingSessions.Duration} -  {codingSessions.Description}");
                    }
                    break;
                case MenuOptions.Quit:
                    isAppRunning = false;
                    break;
                default:
               throw new ArgumentOutOfRangeException(nameof(option), option, "Unhandled menu option.");
            }
        }
    }
    private static string ToDisplayText(MenuOptions option) => option switch
    {
        MenuOptions.AddCodingSession => "Add Coding Session",
        MenuOptions.DeleteCodingSession => "Delete Coding Session",
        MenuOptions.UpdateCodingSession => "Update Coding Session",
        MenuOptions.ViewCodingSession => "View Coding Session",
        MenuOptions.ViewAllCodingSessions => "View All Coding Session",
        MenuOptions.Quit => "Quit",
        _ => option.ToString()
    };
}