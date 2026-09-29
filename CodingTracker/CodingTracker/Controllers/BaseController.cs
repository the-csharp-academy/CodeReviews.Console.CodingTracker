using CodingTracker.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Controllers
{
    internal abstract class BaseController
    {
        protected internal void DisplayMessage(string message, string color = "yellow")
        {
            AnsiConsole.MarkupLine($"[{color}]{message}[/]");
            Console.WriteLine();
        }

        protected internal void DisplayError(string message, string color = "red")
        {
            AnsiConsole.MarkupLine($"[{color}]Error: {message}[/]");
            Console.WriteLine();
        }

        protected internal void DisplayTable(List<CodingSession> sessions, string title)
        {
            Table table = new Table()
                .Border(TableBorder.Rounded)
                .Title($"{title}");

            table.AddColumn("Id");
            table.AddColumn("Start Time");
            table.AddColumn("End Time");
            table.AddColumn("Duration");

            foreach(var session in sessions)
            {
                table.AddRow(
                    session.Id.ToString(),
                    session.StartTime.ToString("yyyy-MM-dd HH:mm"),
                    session.EndTime.ToString("yyyy-MM-dd HH:mm"),
                    session.Duration.ToString(@"hh\:mm"));
            }

            AnsiConsole.Write(table);
        }
        protected internal DateTime GetDateTimeInput(string text, DateTime? defaultValue = null)
        {
            string fullText = defaultValue.HasValue
                    ? $"{text} (Current: {defaultValue.Value.ToString("yyyy-MM-dd HH:mm")}, Press Enter to keep) " : text;

            string userInput = AnsiConsole.Prompt(
                new TextPrompt<string>($"{fullText}")
                    .AllowEmpty()
                    .Validate(input =>
                    {
                        if (string.IsNullOrWhiteSpace(input) && defaultValue.HasValue)
                        {
                            return ValidationResult.Success();
                        }

                        if (DateTime.TryParse(input, out _))
                        {
                            return ValidationResult.Success();
                        }

                        return ValidationResult.Error("[red]Invalid date format. Example: 14:00 or 2026-09-27 14:00[/]");
                    })
                    );

            if (string.IsNullOrWhiteSpace(userInput) && defaultValue.HasValue)
            {
                return defaultValue.Value;
            }

            return DateTime.Parse(userInput);
        }

        protected internal (DateTime startTime, DateTime endTime) DateTimeInput()
        {
            while (true)
            {
                DateTime startTime = GetDateTimeInput("Enter the start time (like 14:00 or 2026-09-27 14:00): ");
                DateTime endTime = GetDateTimeInput("Enter the end time (like 15:00 or 2026-09-27 15:00): ");

                if (endTime >= startTime)
                {
                    DisplayMessage("The input was saved.");
                    return (startTime, endTime);
                }

                DisplayError("End time cannot be earlier than start time. Please enter both again.");
            }
        }

        protected internal CodingSession DateTimeUpdateInput(CodingSession session)
        {
            while (true)
            {
                DateTime newStartTime = GetDateTimeInput("Enter the start time", session.StartTime);
                DateTime newEndTime = GetDateTimeInput("Enter the end time", session.EndTime);

                if (newEndTime >= newStartTime)
                {
                    DisplayMessage("The input was saved.");
                    return new CodingSession
                    {
                        Id = session.Id,
                        StartTime = newStartTime,
                        EndTime = newEndTime
                    };
                }

                DisplayError("End time cannot be earlier than start time. Please re-enter.");
            }
            
        }

        protected internal int InputId()
        {
            int inputNum = AnsiConsole.Ask<int>("Enter the ID to proceed: ");
            return inputNum;
        }

    }
}
