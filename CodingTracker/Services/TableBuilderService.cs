using CodingTracker.Entities;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Services
{
    public static class TableBuilderService
    {
        public static Table BuildSessionsTable(IEnumerable<CodingSession> sessions)
        {
            var table = new Table()
                .AddColumn("Id")
                .AddColumn("Project")
                .AddColumn("Start Time")
                .AddColumn("End Time")
                .AddColumn("Duration");

            foreach(var s in sessions)
            {
                table.AddRow(
                    s.Id.ToString(), 
                    s.Project, 
                    s.StartTime.ToString(), 
                    s.EndTime.ToString(), 
                    s.Duration.ToString());
            }

            return table;
        }
    }
}
