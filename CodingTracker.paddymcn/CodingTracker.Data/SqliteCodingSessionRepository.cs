using CodingTracker.Core;
using Dapper;
using Microsoft.Data.Sqlite;
using CodingTracker.Core.DataModel;

namespace CodingTracker.Data;

public class SqliteCodingSessionRepository(string stringIn) : ICodingSessionRepository
{
    private readonly string connectionString = stringIn;
    public void EnsureCreated()
    {
        using var connection = new SqliteConnection(connectionString);
        const string sql = @"
        CREATE TABLE IF NOT EXISTS CodingSessions (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Description TEXT NOT NULL,
            StartTime TEXT NOT NULL,
            EndTime TEXT NOT NULL
        );";
        connection.Execute(sql);
    }
    public CodingSession Add(CodingSession codingSession)
    {
        using var connection = new SqliteConnection(connectionString);

        const string sql = @"
        INSERT INTO CodingSessions (Description, StartTime, EndTime)
        VALUES (@Description, @StartTime, @EndTime);
        SELECT last_insert_rowid();";

        codingSession.Id = connection.ExecuteScalar<int>(sql, codingSession);
        return codingSession;
    }
    public bool Delete(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        const string sql = @"
        DELETE FROM CodingSessions WHERE Id = @Id;";
        return connection.Execute(sql, new { Id = id }) > 0;
    }
    public List<CodingSession> GetAll()
    {
        using var connection = new SqliteConnection(connectionString);

        const string sql = @"
        SELECT Id, Description, StartTime, EndTime
        FROM CodingSessions
        ORDER BY StartTime;";

        List<CodingSession> sessions = connection.Query<CodingSession>(sql).ToList();
        return sessions;
    }
    public CodingSession? GetById(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        const string sql = @"
        SELECT Id, Description, StartTime, EndTime
        FROM CodingSessions WHERE Id = @Id;";
        CodingSession? session = connection.QuerySingleOrDefault<CodingSession>(sql, new { Id = id });
        return session;
    }
    public bool Update(CodingSession codingSession)
    {
        using var connection = new SqliteConnection(connectionString);
        const string sql = @"
        UPDATE CodingSessions 
        SET Description = @Description, StartTime = @StartTime, EndTime=@EndTime WHERE Id = @Id";
        return connection.Execute(sql, codingSession) > 0;
    }
}