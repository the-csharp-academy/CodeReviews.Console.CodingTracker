using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace Coding.Tracker
{
    [TestFixture]
    public class CodeTracker_should
    {
        private const string DatabaseFile = "coding_tracker_tests.db";
        private CodeSessionService database = null!;

        [Test]
        public void CreateDatabaseTest()
        {
            var session = new CodeSession
            {
                StartTime = "2026-09-21 10:00",
                EndTime = "2026-09-21 11:00",
                Duration = "01:00:00"
            };

            database.Create(session);
            database.CreateDatabase();

            Assert.That(database.ReadAllData().Count, Is.EqualTo(1));
        }

        [TestCase("2026-09-21 10:00", "2026-09-21 11:00", "01:00:00")]
        public void CreateRecordTest(string startTime, string endTime, string duration)
        {
            var session = new CodeSession
            {
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration
            };

            var result = database.Create(session);
            var added = database.ReadAllData().Single();

            Assert.That(result, Is.True);
            Assert.That(added.Id, Is.GreaterThan(0));
            Assert.That(added.StartTime, Is.EqualTo(session.StartTime));
            Assert.That(added.EndTime, Is.EqualTo(session.EndTime));
            Assert.That(added.Duration, Is.EqualTo(session.Duration));
        }

        [Test]
        public void ReadRecordsTest()
        {
            database.Create(new CodeSession
            {
                StartTime = "2026-09-21 10:00",
                EndTime = "2026-09-21 11:00",
                Duration = "01:00:00"
            });

            database.Create(new CodeSession
            {
                StartTime = "2026-09-22 10:00",
                EndTime = "2026-09-22 12:00",
                Duration = "02:00:00"
            });

            var records = database.ReadAllData();

            Assert.That(records.Count, Is.EqualTo(2));
            Assert.That(
                records.Select(r => r.Duration),
                Is.EquivalentTo(new[] { "01:00:00", "02:00:00" }));
        }

        [Test]
        public void DeleteRecordTest()
        {
            database.Create(new CodeSession
            {
                StartTime = "2026-09-21 10:00",
                EndTime = "2026-09-21 11:00",
                Duration = "01:00:00"
            });

            database.Create(new CodeSession
            {
                StartTime = "2026-09-22 10:00",
                EndTime = "2026-09-22 12:00",
                Duration = "02:00:00"
            });

            var records = database.ReadAllData();
            var toDelete = records.Single(r => r.Duration == "01:00:00");
            var toKeep = records.Single(r => r.Duration == "02:00:00");

            var result = database.Delete(toDelete.Id);
            var remaining = database.ReadAllData().Single();

            Assert.That(result, Is.True);
            Assert.That(remaining.Id, Is.EqualTo(toKeep.Id));
            Assert.That(remaining.Duration, Is.EqualTo(toKeep.Duration));
        }

        [Test]
        public void UpdateRecordTest()
        {
            database.Create(new CodeSession
            {
                StartTime = "2026-09-21 10:00",
                EndTime = "2026-09-21 11:00",
                Duration = "01:00:00"
            });

            var original = database.ReadAllData().Single();

            var updatedSession = new CodeSession
            {
                Id = original.Id,
                StartTime = "2026-09-22 12:00",
                EndTime = "2026-09-22 14:30",
                Duration = "02:30:00"
            };

            var result = database.Update(updatedSession);
            var updated = database.ReadAllData().Single();

            Assert.That(result, Is.True);
            Assert.That(updated.Id, Is.EqualTo(original.Id));
            Assert.That(updated.StartTime, Is.EqualTo(updatedSession.StartTime));
            Assert.That(updated.EndTime, Is.EqualTo(updatedSession.EndTime));
            Assert.That(updated.Duration, Is.EqualTo(updatedSession.Duration));
        }

        [SetUp]
        public void Setup()
        {
            DeleteDatabaseFile();
            database = new CodeSessionService($"Data Source={DatabaseFile}");
            database.CreateDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteDatabaseFile();
        }

        private static void DeleteDatabaseFile()
        {
            SqliteConnection.ClearAllPools();
            File.Delete(DatabaseFile);
        }
    }
}