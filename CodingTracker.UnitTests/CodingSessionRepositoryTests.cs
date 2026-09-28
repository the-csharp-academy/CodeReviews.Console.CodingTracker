using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace CodingTracker.UnitTests
{
    public class CodingSessionRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            DatabaseManager.SetConfiguration(config);
            DatabaseManager.Start();
            DatabaseManager.EmptyDb();
        }

        [TestCase("Coding Tracker", "C#", 2026, 9, 21, 18, 30, 0, 2026, 9, 21, 20, 15, 0)]
        [TestCase("Videogame", "C++", 2026, 9, 22, 10, 0, 0, 2026, 9, 22, 12, 45, 0)]
        public void CRUD_CodingSessions(string project, string language, int year1, int month1, int day1, int hours1, int minutes1, int seconds1, int year2, int month2, int day2, int hours2, int minutes2, int seconds2)
        {
            List<CodingSession> newSessions = new()
            {
                new() { Project = project, Language = language,
                    StartTime = new DateTime(year1, month1, day1, hours1, minutes1, seconds1),
                    EndTime   = new DateTime(year2, month2, day2, hours2, minutes2, seconds2) },
            };

            CodingSessionRepository.InsertSessions(newSessions);
            var sessions = CodingSessionRepository.GetSessions();
            Assert.That(sessions, Is.Not.Empty);

            foreach(var session in sessions)
            {
                int rowsDeleted = CodingSessionRepository.DeleteSession(session.Id);
                Assert.That(rowsDeleted, Is.EqualTo(1));
            }

            var empty = CodingSessionRepository.GetSessions();
            Assert.That(empty, Is.Empty);
        }
    }
}
