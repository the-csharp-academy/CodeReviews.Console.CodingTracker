using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Coding.Tracker
{
    public class CodeSessionService : ICodeSessionService
    {   
        private readonly string table = "sessions";
        private readonly string connectionString;
        public CodeSessionService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool CreateDatabase()
        {
            return Execute(db =>
            {
                var query = $@"CREATE TABLE IF NOT EXISTS {table} 
                            (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                StartTime TEXT,
                                EndTime TEXT,
                                Duration TEXT
                            )";
                return db.Execute(query) > 0;
            });
        }

        public bool Create(CodeSession session)
        {
            return Execute(db =>
            {
                var query = $"INSERT INTO {table}(StartTime, EndTime, Duration) VALUES(@StartTime, @EndTime, @Duration)";
                return db.Execute(query, session) > 0;
            });
        }

        public List<CodeSession> ReadAllData()
        {
            using IDbConnection db = new SqliteConnection(connectionString);
            var query = $"SELECT * FROM {table}";
            return db.Query<CodeSession>(query).ToList();
        }

        public bool Update(CodeSession session)
        {
            return Execute(db =>
            {
                var query = $"UPDATE {table} SET StartTime=@StartTime, EndTime=@EndTime, Duration=@Duration WHERE Id=@Id";
                return db.Execute(query, session) > 0;
            });
        }

        public bool Delete(int id)
        {
            return Execute(db =>
            {
                var query = $"DELETE FROM {table} WHERE Id=@id";
                return db.Execute(query, new { id }) > 0;
            });
        }

        private bool Execute(Func<IDbConnection, bool> action)
        {
            using IDbConnection db = new SqliteConnection(connectionString);
            return action(db);
        }
    }
}
