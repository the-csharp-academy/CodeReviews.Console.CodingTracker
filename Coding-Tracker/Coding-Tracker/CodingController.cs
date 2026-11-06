
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Dapper;
using Spectre.Console;

class CodingController
{

    static string _connectionString = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build()
    .GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");


    public static void CreateTable()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Execute(@"CREATE TABLE IF NOT EXISTS coding_tracker (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            StartTime TEXT,
            EndTime TEXT,
            Duration TEXT
        )");
        }
    }

    public static void ViewRecords()
    {
        GetAllRecords();
        Console.WriteLine();
        Console.ReadKey();
    }

    public static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(_connectionString))
        {
            List<Codingtracker> tabledata = connection.Query<Codingtracker>("SELECT * FROM coding_tracker").ToList();

            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Start Time");
            table.AddColumn("End Time");
            table.AddColumn("Duration");

            foreach (var session in tabledata)
            {
                table.AddRow(
                    session.Id.ToString(),
                    session.StartTime.ToString(),
                    session.EndTime.ToString(),
                    session.Duration
                );
            }

            AnsiConsole.Write(table);
        }

    }

    public static void Insert(DateTime startTime, DateTime endTime)
    {

        string duration = (endTime - startTime).ToString();

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Execute("INSERT INTO coding_tracker (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)",
           new { StartTime = startTime, EndTime = endTime, Duration = duration });

            Console.WriteLine("Record Inserted.\nPress any key to return to main menu.");
            Console.ReadKey();
        }
    }

    public static void Delete(int id)
    {

        using (var connection = new SqliteConnection(_connectionString))
        {
            int rowsAffected = connection.Execute("DELETE FROM coding_tracker WHERE Id = @Id", new { Id = id });

            if (rowsAffected == 0)
                Console.WriteLine("No record found with that ID.");
            else
                Console.WriteLine("Record Deleted.\nPress any key to return to main menu.");

            Console.ReadKey();
        }
    }

    public static void Update(int id, DateTime startTime, DateTime endTime, string duration)
    {


        using (var connection = new SqliteConnection(_connectionString))
        {

            int rowsAffected = connection.Execute("UPDATE coding_tracker SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE ID = @Id",
               new { Id = id, StartTime = startTime, EndTime = endTime, Duration = duration });

            if (rowsAffected == 0)
            {
                Console.WriteLine("No record found with that ID.");
            }
            else
                Console.WriteLine("Record Updated.\nPress any key to return to main menu.");

            Console.ReadKey();

        }
    }
}






