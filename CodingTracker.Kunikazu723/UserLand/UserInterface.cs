using CodingTracker.Kunikazu723.Dao.Interfaces;
using CodingTracker.Kunikazu723.Models;
using CodingTracker.Kunikazu723.Services;
using Spectre.Console;
using Microsoft.Data.Sqlite;
using static CodingTracker.Kunikazu723.Enums.MenuEnums;
using static CodingTracker.Kunikazu723.Enums.Filters;

namespace CodingTracker.Kunikazu723.UserLand
{
    public class UserInterface
    {
        private readonly UserInput _userInput;
        private readonly TableEngine _tableEngine;
        private readonly IDao<CodingSession> _dao;
        private readonly string _dateFormat = string.Empty;

        public UserInterface(UserInput userInput, TableEngine engine, IDao<CodingSession> dao, string dateFormat)
        {
            _userInput = userInput;
            _tableEngine = engine;
            _dao = dao;
            _dateFormat = dateFormat;
        }
        public void MainMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                WriteBanner(Color.Blue);
                MainMenuOptions option = _userInput.PromptUserEnumOption<MainMenuOptions>("What do you want to do?");
                Console.Clear();
                try
                {
                    ExecuteOption(option);
                }
                catch (SqliteException ex)
                {
                    if (ex.ErrorCode == 19)
                    {
                        DisplayMessage($"ERROR: Date is already exists in database. Insertion aborted", Color.Red);
                    }
                    else
                    {
                        DisplayMessage($"DATABASE ERROR: \n\t{ex.Message}\nERROR CODE: {ex.ErrorCode}", Color.Red);
                    }
                }
                if (option == MainMenuOptions.Exit)
                    isRunning = false;

                AnsiConsole.MarkupLine("[bold grey]Press Any Key To Continue[/]");
                Console.ReadKey();
            }

        }
        private void ExecuteOption(MainMenuOptions option)
        {
            switch (option)
            {
                case MainMenuOptions.ViewSessions:
                    ViewSessions();
                    break;
                case MainMenuOptions.LogSessions:
                    LogSession();
                    break;
                case MainMenuOptions.UpdateSession:
                    UpdateSession();
                    break;
                case MainMenuOptions.DeleteSession:
                    DeleteSession();
                    break;
                case MainMenuOptions.Exit:
                    AnsiConsole.MarkupLine("Exiting [bold cyan]Coding Tracker[/]...");
                    break;
                default:
                    DisplayMessage("Invalid Option", Color.Red);
                    break;
            }
        }

        

        private void DeleteSession()
        {
            WriteBanner(Color.Red, "Delete Session");
            var codingSession = PromptForExistingSession();
            if (codingSession is null) return;

            Table sessionTable = _tableEngine.GenerateCodingSessionsTable(new List<CodingSession> { codingSession }, "To be Deleted");
            AnsiConsole.Write(sessionTable);

            bool confirmDeletion = AnsiConsole.Confirm("Are you sure you wish to delete permanently this session?", false);
            if (confirmDeletion)
            {
                _dao.DeleteItemById(codingSession);
                DisplayMessage("Session Deleted successfully", Color.Green);
            }
            else
            {
                DisplayMessage("Deletion aborted", Color.Green);
            }
        }

        private int PromptForSessionId()
        {
            ViewSessions();
            return _userInput.AskUserForPositiveInteger("Enter the Session's ID");
        }

        private CodingSession? PromptForExistingSession()
        {
            int id = PromptForSessionId();
            var codingSession = _dao.GetItemById(id);
            if (codingSession is null)
            {
                DisplayMessage($"Coding Session With ID {id} not found\nAborting process", Color.Red);
            }
            return codingSession;
        }

        private CodingSession PromptForNewSession()
        {
            bool useStopwatch = AnsiConsole.Confirm("Use stopwatch to track current session in real time?");
            var codingSession = _userInput.GetUserCodingSession(useStopwatch);
            return codingSession;
        }

        private void UpdateSession()
        {
            WriteBanner(Color.Cyan, "Update Session");
            var currentCodingSession = PromptForExistingSession();

            if (currentCodingSession is null) return;

            Table currentItemTable = _tableEngine.GenerateCodingSessionsTable(new List<CodingSession> { currentCodingSession }, "Current Coding Session");
            AnsiConsole.Write(currentItemTable);

            AnsiConsole.MarkupLine("[bold yellow]Enter new Coding Sessino Data[/]");
            var newCodingSession = _userInput.GetUserCodingSession();
            newCodingSession.Id = currentCodingSession.Id;

            Table newItemTable = _tableEngine.GenerateCodingSessionsTable(new List<CodingSession> { newCodingSession }, "Updated Coding Session");
            AnsiConsole.Write(newItemTable);

            bool confirmUpdate = AnsiConsole.Confirm("Do you wish to apply the Update?", false);

            if (confirmUpdate)
            {
                _dao.UpdateItemById(newCodingSession);
                DisplayMessage("Coding session updated successfuly", Color.Green);
            }
            else
            {
                DisplayMessage("Update Aborted. No changes made.", Color.Green);
            }
        }

        private void LogSession()
        {
            WriteBanner(Color.Yellow, "Log Session");

            var codingSession = PromptForNewSession();

            var toBeInsertedDisplay = _tableEngine.GenerateCodingSessionsTable(new List<CodingSession>() { codingSession }, "To Be Inserted");
            AnsiConsole.Write(toBeInsertedDisplay);

            bool confirmInsertion = AnsiConsole.Confirm("[bold yellow]Do you wish to Log the current session?[/]");
            if (confirmInsertion)
            {
                _dao.InsertItem(codingSession);
                DisplayMessage("New coding session Successfuly Logged", Color.Green);
            }
            else
            {
                DisplayMessage("Insertion aborted, no changes made", Color.Green);
            }

        }

        private void ViewSessions()
        {
            var doApplyFilter = AnsiConsole.Confirm("[bold yellow]Apply Filter to view all coding sessions?[/]", false);
            var codingSessions = _dao.GetAllItems().OrderBy(codingSession => codingSession.StartTime).ToList();
            if (doApplyFilter)
            {
                codingSessions = FilterCodingSessions(codingSessions);
            }

            if (codingSessions.Count == 0)
            {
                AnsiConsole.Write(new Panel("[bold red]No Coding Sessions available[/]")
                    .RoundedBorder()
                    .BorderColor(Color.Yellow));
                return;
            }

            var sessionsTable = _tableEngine.GenerateCodingSessionsTable(codingSessions, "Coding Sessions");
            AnsiConsole.Write(sessionsTable);

        }

        private List<CodingSession> FilterCodingSessions(List<CodingSession> codingSessions)
        {
            var filterType = _userInput.PromptUserEnumOption<FilterType>("[bold yellow]Select Paremeter[/]");

            var amount = _userInput.AskUserForPositiveInteger($"[bold yellow]Enter how many {filterType} before today {DateTime.Today.ToString(_dateFormat)}[/]");

            var today = DateTime.Today;
            
            var lowestDate = filterType switch
            {
                FilterType.Days => today.AddDays(-amount),
                FilterType.Weeks => today.AddDays(-amount * 7),
                FilterType.Months => today.AddMonths(-amount),
                FilterType.Years => today.AddYears(-amount),
                _ => throw new NotImplementedException()
            };

            var filteredCodingSessions = codingSessions
                .Where(codingSession => codingSession.StartTime >= lowestDate);

            if (filteredCodingSessions.Any() == false)
                return filteredCodingSessions.ToList();
            
            var filterOrder = _userInput.PromptUserEnumOption<FilterOrder>("[bold yellow]Select Order[/]");

            filteredCodingSessions = filterOrder switch
            {
                FilterOrder.Ascending => filteredCodingSessions.OrderBy(codingSession => codingSession.StartTime),
                FilterOrder.Descending => filteredCodingSessions.OrderByDescending(codingSession => codingSession.StartTime),
                _ => throw new NotImplementedException()
            };

            return filteredCodingSessions.ToList();
        }

        public void WriteBanner(Color color, string bannerText = "CODING TRACKER")
        {
            var fontPath = Path.Combine(AppContext.BaseDirectory, "FigletFonts", "smkeyboard.flf");
            var figletFont = FigletFont.Load(fontPath);

            var appFiglet = new FigletText(figletFont, bannerText)
                .Color(color);

            var ruleStyle = Style.Parse(color.ToMarkup());
            AnsiConsole.Write(new Rule().RuleStyle(ruleStyle));
            AnsiConsole.Write(appFiglet);
            AnsiConsole.Write(new Rule().RuleStyle(ruleStyle));
        }

        void DisplayMessage(string message, Color color)
        {
            AnsiConsole.MarkupLine($"[{color.ToMarkup()}]{Markup.Escape(message)}[/]");
        }
    }
}
