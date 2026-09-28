using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Globalization;
using System.Runtime.Serialization;

namespace CodingTracker
{
    internal class UIController
    {
        internal enum MenuOption
        {
            TrackSession,
            ViewSessions,
            AddSession,
            DeleteSession,
            ExitApplication
        }

        internal enum SortOrder
        {
            Ascending,
            Descending
        }

        private Dictionary<string, MenuOption> options = new()
        {
            ["Track Session"] = MenuOption.TrackSession,
            ["View Sessions"] = MenuOption.ViewSessions,
            ["Add Sessions"] = MenuOption.AddSession,
            ["Delete Sessions"] = MenuOption.DeleteSession,
            ["Exit Application"] = MenuOption.ExitApplication
        };

        private static IConfiguration _config = null!;
        private static string DateFormat = null!;

        private static CodingSession? sessionInProgress = null;

        private static DateTime Now() =>
            DateTime.Now.AddTicks(-(DateTime.Now.Ticks % TimeSpan.TicksPerSecond));

        public UIController(IConfiguration config)
        {
            _config = config;
            DateFormat = _config["DateTimeFormat"];
        }

        internal void MainMenu()
        {
            bool exitApp = false;
            while (!exitApp)
            {
                AnsiConsole.Clear();
                MenuOption option = SelectMenuOption();
                switch (option)
                {
                    case MenuOption.TrackSession:
                        TrackSession();
                        break;
                    case MenuOption.ViewSessions:
                        ViewSessions();
                        break;
                    case MenuOption.AddSession:
                        AddSession();
                        break;
                    case MenuOption.DeleteSession:
                        DeleteSession();
                        break;
                    case MenuOption.ExitApplication:
                        exitApp = true;
                        AnsiConsole.Clear();
                        break;
                    default:
                        break;
                }
            }
        }

        private MenuOption SelectMenuOption()
        {
            bool optionSelected = false;
            MenuOption option = default;
            while (!optionSelected)
            {
                string selected = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .HighlightStyle(Style.Parse("cyan"))
                    .Title("Main Menu")
                    .AddChoices(options.Keys)
                );
                optionSelected = options.TryGetValue(selected, out option);
            }
            return option;
        }

        private void TrackSession()
        {
            AnsiConsole.WriteLine("Stopwatch for tracking session in progress.");

            if (sessionInProgress == null)
            {
                string project = AnsiConsole.Ask<string>("Write the name of the project you are coding:");
                string language = AnsiConsole.Ask<string>("Write the language you are coding in:");
                string startAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .HighlightStyle(Style.Parse("cyan"))
                    .AddChoices("Start", "Cancel")
                );
                if (startAction != "Start") return;
                sessionInProgress = new(project, language, Now(), null);
            }

            AnsiConsole.Clear();
            AnsiConsole.Write(TableVisualizationEngine.CreateTable([sessionInProgress]));
            string stopAction = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .AddChoices("Go Back", "Stop time")
            );

            if(stopAction == "Stop time")
            {
                AnsiConsole.Clear();
                sessionInProgress.EndTime = Now();
                InsertSession(sessionInProgress);
            }
        }

        private void ViewSessions()
        {
            var sessions = CodingSessionRepository.GetSessions();
            if(sessionInProgress != null)
            {
                sessions.Add(sessionInProgress);
            }

            string viewChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .AddChoices("View all", "View filtered")
            );
            if(viewChoice == "View filtered")
            {
                sessions = FilterSessions(sessions);
            }
            sessions = SortSessions(sessions);

            Table table = TableVisualizationEngine.CreateTable(sessions);
            Panel menu = new Panel(table).Header("Coding Sessions").Expand();
            menu.Height = Console.WindowHeight - 1;
            AnsiConsole.Write(menu);
            string action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .AddChoices("Go back")
            );
        }

        private List<CodingSession> FilterSessions(List<CodingSession> sessions)
        {
            List<string> filterChoices = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .Title("Filter by (space to select, enter to confirm)")
                .AddChoices("Time period", "Project", "Language", "Min duration")
            );

            foreach(string choice in filterChoices)
            {
                switch(choice)
                {
                    case "Time period":
                        sessions = FilterByPeriod(sessions);
                        break;
                    case "Language":
                        sessions = FilterByLanguage(sessions);
                        break;
                    case "Project":
                        sessions = FilterByProject(sessions);
                        break;
                    case "Min duration":
                        sessions = FilterByMinDuration(sessions);
                        break;
                    default:
                        break;
                }
            }
            return sessions;
        }

        private List<CodingSession> SortSessions(List<CodingSession> sessions)
        {
            string sortChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .Title("Sort by")
                .AddChoices("Id", "Project", "Language", "Start time", "End time", "Duration")
            );
            switch (sortChoice)
            {
                case "Id":
                    sessions.Sort((a, b) => a.Id.CompareTo(b.Id));
                    break;
                case "Language":
                    sessions.Sort((a, b) => a.Language.CompareTo(b.Language));
                    break;
                case "Project":
                    sessions.Sort((a, b) => a.Project.CompareTo(b.Project));
                    break;
                case "Start time":
                    sessions.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
                    break;
                case "End time":
                    sessions.Sort((a, b) =>
                    {
                        if (a.EndTime == null && b.EndTime == null) return 0;
                        if (a.EndTime == null) return 1;
                        if (b.EndTime == null) return -1;
                        return a.EndTime.Value.CompareTo(b.EndTime.Value);
                    });
                    break;
                case "Duration":
                    sessions.Sort((a, b) => a.Duration.CompareTo(b.Duration));
                    break;
            }

            SortOrder sortOrder = AnsiConsole.Prompt(
                new SelectionPrompt<SortOrder>()
                .HighlightStyle(Style.Parse("cyan"))
                .Title("Sort order")
                .AddChoices(SortOrder.Ascending, SortOrder.Descending)
            );
            if (sortOrder == SortOrder.Descending) sessions.Reverse();

            return sessions;
        }

        private List<CodingSession> FilterByPeriod(List<CodingSession> sessions)
        {
            List<CodingSession> result = new();
            var (start, end) = PromptPeriod();
            foreach (CodingSession session in sessions)
            {
                if (session.StartTime >= start && session.StartTime < end)
                {
                    result.Add(session);
                }
            }
            return result;
        }

        private (DateTime, DateTime) PromptPeriod()
        {
            string period = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .Title("Period")
                .AddChoices("Today", "This week", "This month", "This year", "Custom range")
            );
            DateTime today = DateTime.Today;
            (DateTime, DateTime) datePeriod = new();

            switch(period)
            {
                case "Today":
                    datePeriod = (today, today.AddDays(1));
                    break;
                case "This week":
                    datePeriod = (today.AddDays(-(int)today.DayOfWeek), today.AddDays(7 - (int)today.DayOfWeek));
                    break;
                case "This month":
                    datePeriod = (new DateTime(today.Year, today.Month, 1), new DateTime(today.Year, today.Month, 1).AddMonths(1));
                    break;
                case "This year":
                    datePeriod = (new DateTime(today.Year, 1, 1), new DateTime(today.Year + 1, 1, 1));
                    break;
                case "Custom range":
                    datePeriod = PromptCustomRange();
                    break;
            }
            return datePeriod;
        }

        private (DateTime, DateTime) PromptCustomRange()
        {
            DateTime startTime = default;
            DateTime endTime = default;
            bool validDate = false;
            while (!validDate)
            {
                string startTimeInput = AnsiConsole.Ask<string>("Write the start date in (yyyy-MM-dd) format:");
                validDate = InputValidation.ValidateDateTime(startTimeInput, out startTime);
            }
            validDate = false;
            while (!validDate)
            {
                string endTimeInput = AnsiConsole.Ask<string>("Write the end date in (yyyy-MM-dd) format:");
                validDate = InputValidation.ValidateDateTime(endTimeInput, out endTime);
                validDate = InputValidation.ValidateEndTime(startTime, endTime);
                endTime.AddDays(1);
            }
            return (startTime, endTime);
        }

        private List<CodingSession> FilterByLanguage(List<CodingSession> sessions)
        {
            List<CodingSession> result = new();
            string language = AnsiConsole.Ask<string>("Language:");
            foreach (CodingSession session in sessions)
            {
                if(session.Language.Equals(language, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(session);
                }
            }
            return result;
        }

        private List<CodingSession> FilterByProject(List<CodingSession> sessions)
        {
            List<CodingSession> result = new();
            string project = AnsiConsole.Ask<string>("Project:");
            foreach (CodingSession session in sessions)
            {
                if (session.Project.Equals(project, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(session);
                }
            }
            return result;
        }

        private List<CodingSession> FilterByMinDuration(List<CodingSession> sessions)
        {
            List<CodingSession> result = new();
            int minutes = AnsiConsole.Ask<int>("Minimum duration (minutes):");
            foreach (CodingSession session in sessions)
            {
                if(session.Duration.TotalMinutes >= minutes)
                {
                    result.Add(session);
                }
            }
            return result;
        }

        private void AddSession()
        {
            string project = AnsiConsole.Ask<string>("Write the name of the project you are coding:");
            string language = AnsiConsole.Ask<string>("Write the language you are coding in:");
            DateTime startTime = default;
            DateTime endTime = default;
            bool validDate = false;
            while (!validDate)
            {
                string startTimeInput = AnsiConsole.Ask<string>("Write the start time in (yyyy-MM-dd HH:mm:ss) format:");
                validDate = InputValidation.ValidateDateTime(startTimeInput, out startTime);
            }
            validDate = false;
            while (!validDate)
            {
                string endTimeInput = AnsiConsole.Ask<string>("Write the end time in (yyyy-MM-dd HH:mm:ss) format:");
                validDate = InputValidation.ValidateDateTime(endTimeInput, out endTime);
                validDate = InputValidation.ValidateEndTime(startTime, endTime);
            }
            CodingSession newSession = new(project, language, startTime, endTime);
            InsertSession(newSession);
        }

        private void InsertSession(CodingSession newSession)
        {
            AnsiConsole.Write(TableVisualizationEngine.CreateTable([newSession]));
            bool insertNew = AnsiConsole.Confirm($"Do you want to add this new session?");
            if (insertNew)
            {
                int rowsInserted = CodingSessionRepository.InsertSessions([newSession]);
                AnsiConsole.MarkupLine($"[{(rowsInserted > 0 ? "lime" : "red")}]{rowsInserted} new sessions added[/]");
                string action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .HighlightStyle(Style.Parse("cyan"))
                    .AddChoices("Go back")
                );
            }
        }

        private void DeleteSession()
        {
            var sessions = CodingSessionRepository.GetSessions();
            bool keepDeleting = true;
            while (keepDeleting)
            {
                if (sessions.Count() == 0)
                {
                    AnsiConsole.WriteLine("There are no sessions to delete");
                    string action = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .HighlightStyle(Style.Parse("cyan"))
                        .AddChoices("Go back")
                    );
                    return;
                }

                AnsiConsole.WriteLine("Select a session to delete\n");

                CodingSession? selected = PromptSelectSession(sessions);
                if (selected == null) return;

                AnsiConsole.Write(TableVisualizationEngine.CreateTable([selected]));
                bool delete = AnsiConsole.Confirm("Are you sure you want to delete this session entry?");
                if (!delete)
                {
                    AnsiConsole.Clear();
                    continue;
                }
                sessions.Remove(selected);
                int rowsDeleted = CodingSessionRepository.DeleteSession(selected.Id);
                AnsiConsole.WriteLine($"{rowsDeleted} row deleted: {selected.Id} ");
                keepDeleting = AnsiConsole.Confirm("Do you want to delete more sessions?");
            }
        }

        private static CodingSession? PromptSelectSession(List<CodingSession> sessions)
        {
            CodingSession cancelOption = new(){ Id = -1 };
            if (!sessions.Exists(s => s.Id == cancelOption.Id))
            {
                sessions.Add(cancelOption);
            }
            CodingSession selected = AnsiConsole.Prompt(
                new SelectionPrompt<CodingSession>()
                .HighlightStyle(Style.Parse("cyan"))
                .Title($"  {"Id",-4} {"Project",-15} {"Language",-10} {"Start",-20} {"End",-20} {"Duration",-11}")
                .UseConverter(
                    s => s.Id == -1 
                    ? "[grey]Cancel[/]" 
                    : $"{s.Id,-4} {s.Project,-15} {s.Language,-10} {s.StartTime.ToString(DateFormat).PadRight(19)}  {s.EndTime?.ToString(DateFormat).PadRight(19) ?? "In progress".PadRight(19)}  {(int)s.Duration.TotalHours,2}h {s.Duration.Minutes:00}m {s.Duration.Seconds:00}s")
                .AddChoices(sessions)
            );
            return selected.Id == -1 ? null : selected;
        }
    }
}
