using CodingTracker.Entities;
using CodingTracker.Repositories;
using CodingTracker.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Controllers
{
    public static class CodingController
    {
        public static void HandleNewSession(string project, DateTime startTime, DateTime endTime)
        {
            var codingSession = new CodingSession()
            {
                Project = project,
                StartTime = startTime,
                EndTime = endTime,
                Duration = (int)endTime.Subtract(startTime).TotalMinutes
            };

            CalendarRepository.CreateCodingSessions(codingSession);
        }

        public static string EditCodingSession(CodingSession session, string project, string startTime, string endTime)
        {
            string result = "Success";

            session.Project = project == string.Empty ? session.Project : project;

            if(startTime != string.Empty)
            {

                if (ValidationService.TryParseDate(startTime, out DateTime newStartTime))
                {
                    session.StartTime = newStartTime;
                }
                else
                {
                    return result = "Invalid Start Time provided";
                }
            }

            if (endTime != string.Empty)
            {
                if(ValidationService.TryParseDate(endTime, out DateTime newEndTime))
                {
                    session.EndTime = newEndTime;
                }
                else
                {
                    return result = "Invalid End Time Provided";
                }
            }

            if(!ValidationService.StartSmallerThanEnd(session.StartTime, session.EndTime))
            {
                return result = "End time cannot be bofore start Time";
            }

            session.Duration = (int)session.EndTime.Subtract(session.StartTime).TotalMinutes;

            CalendarRepository.UpateCodingSession(session);

            return result;
        }

        public static IEnumerable<CodingSession> GetSessionsByYear(DateTime year, string sortOrder)
        {
            var start = new DateTime(year.Year, 1, 1);
            var end = start.AddYears(1);

            return CalendarRepository.GetCodingSessions(start, end, sortOrder);
        }

        private static DateTime StartOfWeek(DateTime date)
        {
            int difference = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;

            return date.Date.AddDays(-difference);
        }

        public static IEnumerable<CodingSession> GetSessionsByWeek(DateTime date, string sortOrder)
        {
            var start = StartOfWeek(date);
            var end = date.Date.AddDays(7);

            return CalendarRepository.GetCodingSessions(start, end, sortOrder);
        }

        public static IEnumerable<CodingSession> GetSessionsByMonth(DateTime date, string sortOrder)
        {
            var start = new DateTime(date.Year, date.Month, 1);
            var end = start.AddMonths(1);

            return CalendarRepository.GetCodingSessions(start, end, sortOrder);
        }

        public static IEnumerable<CodingSession> GetSessionsByDay(DateTime date, string sortOrder)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return CalendarRepository.GetCodingSessions(start, end, sortOrder);
        }
    }
}
