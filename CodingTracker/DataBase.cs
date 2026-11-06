using System.Globalization;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace CodingTracker;

class DataBase
{
    public static IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    public static string? connectionString = config.GetConnectionString("DefaultConnection");
    
    public static void CreateDataBase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding_session (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration INTEGER
                    )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public static void GetAllRecords()
    {
        var sql = $"SELECT * FROM coding_session";
        using var connection = new SqliteConnection(connectionString);
        var codingSessions = connection.Query<CodingSession>(sql).ToList();
        foreach (var session in codingSessions)
        {
           AnsiConsole.MarkupLine($"Session {session.Id} started at: {session.StartTime}, ended at: {session.EndTime}, duration: {session.FormattedDuration}");
        }
    }

    public static void AddNewRecord()
    {
        string? StartTime = UserInput.GetTimeInput("Start time");
        if (StartTime is null) return;

        string? EndTime = UserInput.GetTimeInput("End time");
        if (EndTime is null) return;

        double Duration = DurationCalculator.CalculateDurationInMinutes(StartTime, EndTime);

        using (var connection = new SqliteConnection(connectionString))
        {
            string sql = @"INSERT INTO coding_session (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration);";

            // Create the data object to insert
            var newSession = new {StartTime, EndTime, Duration};
            // Execute returns the number of rows inserted (should be 1)
            int rowsAffected = connection.Execute(sql, newSession);

            Console.WriteLine($"{rowsAffected} record added successfully.");
        }
    }

    public static void UpdateRecord()
    {
        var id = AnsiConsole.Ask<int>("Enter Record [red]Id[/]");

        string? StartTime = UserInput.GetTimeInput("Start time");
        if (StartTime is null) return;

        string? EndTime = UserInput.GetTimeInput("End time");
        if (EndTime is null) return;

        double Duration = DurationCalculator.CalculateDurationInMinutes(StartTime, EndTime);

        using (var connection = new SqliteConnection(connectionString))
        {
            string sql = "UPDATE coding_session SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @id;";

            var updateSession = new {id, StartTime, EndTime, Duration};

            int rowsAffected = connection.Execute(sql, updateSession);

            Console.WriteLine($"{rowsAffected} record updated.");
        }
    }

    public static void DeleteRecord()
    {
        var Id = AnsiConsole.Ask<int>("Enter Record [red]Id[/]");
        
        using (var connection = new SqliteConnection(connectionString))
        {
            string sql = "DELETE FROM coding_session WHERE Id = @Id;";

            int rowsAffected = connection.Execute(sql, new { Id });

            if (rowsAffected > 0)
            {
                Console.WriteLine("Record deleted successfully.");
            }
            else
            {
                Console.WriteLine("No record found with that ID.");
            }
        }
    }

}

public static class DurationCalculator
{
    public static int CalculateDurationInMinutes(string startTimeStr, string endTimeStr)
    {
        // Parse the inputs (assuming today's date for simple calculations)
        DateTime today = DateTime.Today;
        DateTime startTime = DateTime.ParseExact($"{today:yyyy-MM-dd} {startTimeStr}", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        DateTime endTime = DateTime.ParseExact($"{today:yyyy-MM-dd} {endTimeStr}", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        // Midnight crossover check: if end time is before start time, it means next day
        if (endTime < startTime)
        {
            endTime = endTime.AddDays(1);
        }

        TimeSpan difference = endTime - startTime;
        
        // Return rounded total minutes as an integer
        return (int)Math.Round(difference.TotalMinutes);
    }
}
