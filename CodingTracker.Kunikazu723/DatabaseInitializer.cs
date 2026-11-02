using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Kunikazu723
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;
        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Initialize()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Execute("""
                CREATE TABLE IF NOT EXISTS CodingSessions (
                  Id INTEGER PRIMARY KEY AUTOINCREMENT,
                  StartTime TEXT UNIQUE NOT NULL,
                  EndTime TEXT UNIQUE NOT NULL
                )
                """);
        }
    }
}
