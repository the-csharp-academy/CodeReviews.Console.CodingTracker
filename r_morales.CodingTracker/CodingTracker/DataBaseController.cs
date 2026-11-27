using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

public class DataBaseController
{
    const string tableName = "CodingSessions";

    public static void CreateTableIfNotExists(string? connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        string sql = @$"CREATE TABLE IF NOT EXISTS {tableName} (
                        SessionID INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT NOT NULL,
                        EndTime TEXT NOT NULL,
                        Date TEXT NOT NULL
                    );";
        try
        {
            connection.Execute(sql);
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        finally
        {
            connection.Dispose();
        }
    }

    public static void InsertCodingSession(string? connectionString, CodingSession session)
    {
        using var connection = new SqliteConnection(connectionString);
        string sql = @$"INSERT INTO {tableName} (StartTime, EndTime, Date) 
                        VALUES (@StartTime, @EndTime, @Date);";

        try
        {
            connection.Execute(sql, new { session.StartTime, session.EndTime, session.Date });
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        finally
        {
            connection.Dispose();
        }
    }

    public static List<CodingSession> GetAllCodingSessions(string? connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        string sql = $"SELECT * FROM {tableName};";

        try
        {
           return connection.Query<CodingSession>(sql).ToList();
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        finally
        {
            connection.Dispose();
        }

        return null;
    }

    public static void UpdateCodingSession(string? connectionString, CodingSession session)
    {
        using var connection = new SqliteConnection(connectionString);
        string sql = $"UPDATE {tableName} SET StartTime = @StartTime, EndTime = @EndTime, Date = @Date WHERE SessionID = @SessionID";
        try
        {
            connection.Execute(sql, new { session.StartTime, session.EndTime, session.Date, session.SessionID });
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        finally
        {
            connection.Dispose();
        }
    }

    public static void DeleteCodingSessionByID(string? connectionString, int id)
    {
        using var connection = new SqliteConnection(connectionString);
        string sql = $"DELETE FROM {tableName} WHERE SessionID = @ID";

        try
        {
            connection.Execute(sql, new { ID = id });
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        finally
        {
            connection.Dispose();
        }
    }
}
