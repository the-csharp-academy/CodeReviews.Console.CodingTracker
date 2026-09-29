using CodingTracker.Controllers;
using CodingTracker.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Repository
{
    internal class CodingSessionsRepository : BaseController, ICodingSessionsRepository
    {
        private readonly DbContext _context;

        public CodingSessionsRepository(DbContext context)
        {
            _context = context;
        }

        public List<CodingSession> GetAll()
        {
            using var connection = _context.CreateConnection();
            string sql = "SELECT * FROM CodingSession";
            return connection.Query<CodingSession>(sql).ToList();
        }

        public CodingSession? GetById(int id)
        {
            using var connection = _context.CreateConnection();
            string sql = "SELECT * FROM CodingSession WHERE Id = @Id";
            return connection.QueryFirstOrDefault<CodingSession>(sql, new {Id = id});

        }

        public int Create(CodingSession session)
        {
            using var connection = _context.CreateConnection();
            string sql = "INSERT INTO CodingSession (StartTime, EndTime) VALUES (@StartTime, @EndTime);" +
                "SELECT last_insert_rowid();";

            return connection.ExecuteScalar<int>(sql, session);
        }

        public bool Delete(int id) 
        {
            using var connection = _context.CreateConnection();
            string sql = "DELETE FROM CodingSession WHERE Id = @Id";

            int rows = connection.Execute(sql, new { Id = id });
            return rows > 0;
        }

        public bool Update(CodingSession session) 
        {
            using var connection = _context.CreateConnection();
            string sql = "UPDATE CodingSession SET StartTime = @StartTime, EndTime = @EndTime WHERE Id = @Id";

            int rows = connection.Execute(sql, session);

            return rows > 0;
        }
    }
}
