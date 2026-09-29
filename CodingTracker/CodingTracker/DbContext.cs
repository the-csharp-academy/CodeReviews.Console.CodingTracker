using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CodingTracker
{
    internal class DbContext
    {
        private readonly string _connectionString;

        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        
        public void InitialTable()
        {
            
            using(var connection = CreateConnection())
            {
                var tableCmd = """
                        CREATE TABLE IF NOT EXISTS CodingSession (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            StartTime TEXT NOT NULL,
                            EndTime TEXT NOT NULL);

                    """;

                connection.Execute(tableCmd);
            }
        }
    }
}
