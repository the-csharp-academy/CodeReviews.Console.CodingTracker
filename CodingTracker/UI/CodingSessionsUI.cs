using Spectre.Console;
using CodingTracker.Repositories;
using CodingTracker.Entities;
using CodingTracker.Controllers;
using CodingTracker.UI.Components;
using CodingTracker.Services;

namespace CodingTracker.UI
{
    public static class CodingSessionsUI
    {
        public static void Show()
        {
            while (true)
            {
                AnsiConsole.Clear();
                TitleComponent.Show();

                var sessions = CalendarRepository.GetCodingSessions(DateTime.Today.AddYears(-1), DateTime.Now, string.Empty);
                var table = TableBuilderService.BuildSessionsTable(sessions);

                AnsiConsole.Write(table);

                var Choice = AnsiConsole.Prompt(new SelectionPrompt<SessionsUIOptions>()
                    .Title("What do you want to do?")
                    .UseConverter(o => o switch
                    {
                        SessionsUIOptions.ViewFiltered => "View filtered sessions",
                        SessionsUIOptions.EditSession => "Edit session",
                        SessionsUIOptions.DeleteSession => "Delete session",
                        SessionsUIOptions.Back => "back"
                    })
                    .AddChoices(Enum.GetValues<SessionsUIOptions>()));

                switch (Choice)
                {
                    case SessionsUIOptions.ViewFiltered:
                        FilterSessionsMenu();
                        break;
                    case SessionsUIOptions.EditSession:
                        EditMenu();
                        break;
                    case SessionsUIOptions.DeleteSession:
                        DeleteMenu();
                        break;
                    case SessionsUIOptions.Back:
                        return;
                }

            }
        }

        public static void EditMenu()
        {
            var codingSessions = CalendarRepository.GetCodingSessions(DateTime.MinValue, DateTime.Now, string.Empty);

            var choices = codingSessions.Select(x => new CodingSessionChoice(x)).ToList();

            choices.Add(new CodingSessionChoice());

            var choice = AnsiConsole.Prompt(new SelectionPrompt<CodingSessionChoice>()
                .Title("Please select A Coding Session to update")
                .UseConverter(x =>
                {
                    if (x.IsCancel)
                    {
                        return "[bold]Cancel[/]";
                    }

                    return $"{x.Session!.Id} - {x.Session.Project} - {x.Session.StartTime} - {x.Session.EndTime} - {x.Session.Duration}";
                }).AddChoices(choices));

            if (choice.IsCancel)
            {
                return;
            }

            var project = AnsiConsole.Prompt(new TextPrompt<string>
                ("Enter a new [bold green]Project name[/] or leave empty to keep same value\nThen press enter to continue:")
                .AllowEmpty());

            var startTime = AnsiConsole.Prompt(new TextPrompt<string>("Enter a new Start time or leave empty to keep previous value." +
                "\nExpected format is: [bold yellow]'MM/DD/YYYY hh:mm:ss:'[/]")
                .AllowEmpty());

            var endTime = AnsiConsole.Prompt(new TextPrompt<string>("Enter a new End time or leave empty to keep previous value." +
                "\nExpected format is: [bold yellow]'MM/DD/YYYY hh:mm:ss:'[/]")
                .AllowEmpty());

            var updatedProject = string.IsNullOrWhiteSpace(project) ? choice.Session!.Project : project;
            var updatedStartTime = string.IsNullOrWhiteSpace(startTime) ? choice.Session!.StartTime.ToString() : startTime;
            var updatedEndTime = string.IsNullOrWhiteSpace(endTime) ? choice.Session!.EndTime.ToString() : endTime;


            AnsiConsole.Clear();
            TitleComponent.Show();

            var table = new Table()
                .AddColumn("")
                .AddColumn("Current")
                .AddColumn("")
                .AddColumn("Upated")
                .AddRow("Project", $"[red]{choice.Session!.Project}[/]", "[darkorange]->[/]", $"[green]{updatedProject}[/]")
                .AddRow("Start Time", $"[red]{choice.Session.StartTime}[/]", "[darkorange]->[/]", $"[green]{updatedStartTime}[/]")
                .AddRow("End Time", $"[red]{choice.Session.EndTime}[/]", "[darkorange]->[/]", $"[green]{updatedEndTime}[/]");


            AnsiConsole.Write(table);

            var confirm = AnsiConsole.Confirm("you are about to update your session, please confirm:");

            if (confirm)
            {
                var result = CodingController.EditCodingSession(choice.Session!, project, startTime, endTime);
                
                if(result.ToLower() != "success")
                {
                    AnsiConsole.MarkupLine($"[bold red]Failed to update coding session: {result}[/]" +
                        $"\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }
            }

            AnsiConsole.MarkupLine($"[bold green]Coding Session Id: {choice.Session!.Id} was successfully updated[/]\nPress any key to continue..");
            Console.ReadKey();
        }

        public static void DeleteMenu()
        {
            var codingSessions = CalendarRepository.GetCodingSessions(DateTime.MinValue, DateTime.Now, string.Empty);

            var choices = codingSessions.Select(x => new CodingSessionChoice(x)).ToList();
            choices.Add(new CodingSessionChoice());

            var choice = AnsiConsole.Prompt(new SelectionPrompt<CodingSessionChoice>()
                .Title("Select a session to delete:")
                .UseConverter(x => {

                    if (x.IsCancel)
                    {
                        return "Canel";
                    }
                    return $"{x.Session!.Id} - {x.Session.Project} - {x.Session.StartTime} - {x.Session.EndTime} - {x.Session.Duration}";
                    })
                .AddChoices(choices));

            if (choice.IsCancel)
            {
                return;
            }

            var Conifirm = AnsiConsole.Confirm("Are you sure you want to delete session: \n" +
                $"[red]Id: {choice.Session!.Id} - " +
                $"Project: {choice.Session.Project} - " +
                $"StartTime: {choice.Session.StartTime} - " +
                $"EndTime: {choice.Session.EndTime} ?[/]\n");

            if (Conifirm)
            {
                CalendarRepository.DeleteCodingSession(choice.Session!);
                AnsiConsole.MarkupLine($"[red]Coding session Id: {choice.Session!.Id} deleted![/]\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        public static void FilterSessionsMenu()
        {
            var filterChoice = AnsiConsole.Prompt(new SelectionPrompt<FilterSessionsOptions>()
                .Title("How would you like to filter your sessions?")
                .UseConverter(x => x switch
                {
                    FilterSessionsOptions.Day => "Filter by [bold]day[/]",
                    FilterSessionsOptions.Week => "Filter by [bold]week[/]",
                    FilterSessionsOptions.Month => "Filter by [bold]Month[/]",
                    FilterSessionsOptions.Year => "Filter by [bold]Year[/]",
                    FilterSessionsOptions.Cancel => "[bold]Cancel[/]"
                })
                .AddChoices(Enum.GetValues<FilterSessionsOptions>()));

            if(filterChoice == FilterSessionsOptions.Cancel)
            {
                return;
            }

            var orderChoice = AnsiConsole.Prompt(new SelectionPrompt<SortOrder>()
                .Title("How would you like to order the results>")
                .AddChoices(Enum.GetValues<SortOrder>()));


            switch (filterChoice)
            {
                case FilterSessionsOptions.Day:
                    {
                        var date = DatePromptComponent.GetDate();
                        var sessions = CodingController.GetSessionsByDay(date, orderChoice.ToString());

                        FilteredResultsComponent.RenderFilteredResults(sessions);
                    }
                    break;
                case FilterSessionsOptions.Week:
                    {
                        var date = DatePromptComponent.GetDate();
                        var sessions = CodingController.GetSessionsByWeek(date, orderChoice.ToString());

                        FilteredResultsComponent.RenderFilteredResults(sessions);
                    }
                    break;
                case FilterSessionsOptions.Month:
                    {
                        var date = DatePromptComponent.GetDate();
                        var sessions = CodingController.GetSessionsByMonth(date, orderChoice.ToString());

                        FilteredResultsComponent.RenderFilteredResults(sessions);
                    }
                    break;
                case FilterSessionsOptions.Year:
                    {
                        var date = DatePromptComponent.GetDate();
                        var sessions = CodingController.GetSessionsByYear(date, orderChoice.ToString());

                        FilteredResultsComponent.RenderFilteredResults(sessions);
                    }
                    break;
            }
        }
    }

    enum SessionsUIOptions
    {
        ViewFiltered,
        EditSession,
        DeleteSession,
        Back
    }

    enum FilterSessionsOptions
    {
        Day,
        Week,
        Month,
        Year,
        Cancel
    }

    enum SortOrder
    {
        Ascending,
        Descendings
    }
}
