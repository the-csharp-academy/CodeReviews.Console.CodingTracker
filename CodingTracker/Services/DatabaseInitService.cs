using Dapper;
using Spectre.Console;
using System.Data.SQLite;

namespace CodingTracker.Services
{
    internal class DatabaseInitService
    {
        public void CreateDirectory()
        {
            string path = @"./db";

            if (!Directory.Exists(path))
            {
                AnsiConsole.MarkupLine($"[yellow]Database Directory does not exist, creating...[/]");
                try
                {
                    Directory.CreateDirectory("./db");
                }catch(Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error initializing the database: {ex.Message}[/]");
                }
                finally
                {
                    AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
                    Console.ReadKey();
                }
            }
        }

        public void InitTable(string connectionSting)
        {
            try
            {
                using var connection = new SQLiteConnection(connectionSting);
                connection.Open();

                string query = @"CREATE TABLE IF NOT EXISTS CodingSessions (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Project TEXT NOT NULL,
                                StartTime TEXT NOT NULL,
                                EndTime TEXT,
                                Duration INTEGER)";

                connection.Execute(query);

            }catch(Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error initializing the database: {ex.Message}[/]");
            }
        }
    }
}
