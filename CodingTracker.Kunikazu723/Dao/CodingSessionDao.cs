using CodingTracker.Kunikazu723.Dao.Interfaces;
using CodingTracker.Kunikazu723.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Kunikazu723.Dao
{
    public class CodingSessionDao : IDao<CodingSession>
    {
        private readonly string _connectionString;

        public CodingSessionDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void DeleteItemById(CodingSession codingSession)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = """
                DELETE FROM CodingSessions WHERE Id = @Id
                """;
            connection.Execute(sql, codingSession);
        }

        public List<CodingSession> GetAllItems()
        {
            using var connection = new SqliteConnection(_connectionString);
            string sql = """
                SELECT * FROM CodingSessions
                """;
            return connection.Query<CodingSession>(sql).ToList();
        }

        public CodingSession? GetItemById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = """
                SELECT * FROM CodingSessions WHERE Id = @itemId
                """;
            return connection.QuerySingleOrDefault<CodingSession>(sql, new { itemId = id });
        }

        public void InsertItem(CodingSession item)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = """
                INSERT INTO CodingSessions (StartTime, EndTime) VALUES (@StartTime, @EndTime)
                """;
            connection.Execute(sql, item);
        }

        public void InsertMany(List<CodingSession> collection)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            connection.Execute("""
                INSERT INTO CodingSessions (StartTime, EndTime)
                VALUES (@StartTime, @EndTime)
                """, collection, transaction);
            transaction.Commit();
        }

        public void UpdateItemById(CodingSession newItem)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = """
                UPDATE CodingSessions
                SET StartTime = @StartTime, EndTime = @EndTime
                WHERE Id = @Id
                """;
            connection.Execute(sql, newItem);
        }
    }
}
