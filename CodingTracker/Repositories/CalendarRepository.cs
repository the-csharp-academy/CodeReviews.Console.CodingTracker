using CodingTracker.Entities;
using Spectre.Console;
using System.Data.SQLite;
using Dapper;
using CodingTracker.Configuration;




namespace CodingTracker.Repositories
{
    public static class CalendarRepository
    {
        private static readonly string _connectionString = AppConfig.Settings.ConnectionString;

        public static IEnumerable<CodingSession> GetCodingSessions(DateTime start, DateTime end, string sortOrder)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                var query = "SELECT * FROM CodingSessions WHERE StartTime >= @Start and StartTime < @End";

                var result = connection.Query<CodingSession>(query, new { Start = start, End = end }).OrderByDescending(x => x.StartTime).ToList();

                if(sortOrder.ToLower() == "ascending")
                {
                    return result.OrderBy(x => x.StartTime).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error getting coding sessions: {ex.Message}[/]");

                return new List<CodingSession>();
            }
            
        }

        public static void CreateCodingSessions(CodingSession c)
        {
            using var connection = new SQLiteConnection(_connectionString);

            connection.Open();

            var query = "INSERT INTO CodingSessions (Project ,StartTime, EndTime, Duration )" +
                "VALUES (@Project, @StartTime, @EndTime, @Duration)";

            connection.Execute(query, new { c.Project, c.StartTime, c.EndTime, c.Duration });
        }

        public static void UpateCodingSession(CodingSession c)
        {
            using var connection = new SQLiteConnection(_connectionString);

            connection.Open();

            var query = "UPDATE CodingSessions SET Project = @Project, StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";

            connection.Execute(query, new {  c.Project, c.StartTime, c.EndTime, c.Duration, c.Id });
        }

        public static void DeleteCodingSession(CodingSession c)
        {
            using var connection = new SQLiteConnection(_connectionString);

            connection.Open();

            var query = "DELETE FROM CodingSessions WHERE Id = @Id";

            connection.Execute(query, new { c.Id });
        }
    }
}
