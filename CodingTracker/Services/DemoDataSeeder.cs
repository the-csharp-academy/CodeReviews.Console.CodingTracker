using System.Data.SQLite;
using Dapper;

namespace CodingTracker.Services;

internal static class DemoDataSeeder
{
    public static void Seed(string connectionString)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        // Prevent duplicate demo data if the method is accidentally run twice.
        connection.Execute("DELETE FROM CodingSessions");

        var random = new Random(42);

        DateTime firstDay = new DateTime(2026, 1, 1);
        DateTime lastDay = new DateTime(2026, 9, 18);

        for (DateTime day = firstDay; day <= lastDay; day = day.AddDays(1))
        {
            // Weekends have a lower chance of coding activity.
            double activityChance = day.DayOfWeek switch
            {
                DayOfWeek.Saturday => 0.25,
                DayOfWeek.Sunday => 0.15,
                _ => 0.60
            };

            if (random.NextDouble() > activityChance)
                continue;

            string project = GetProjectForDate(day, random);

            // Most days have one session, occasionally two.
            int sessionCount = random.NextDouble() < 0.15 ? 2 : 1;

            for (int i = 0; i < sessionCount; i++)
            {
                int duration = GenerateDuration(random);

                int startHour = random.Next(9, 16);
                int startMinute = random.Next(0, 4) * 15;

                DateTime startTime = day
                    .AddHours(startHour)
                    .AddMinutes(startMinute);

                DateTime endTime = startTime.AddMinutes(duration);

                connection.Execute(
                    """
                    INSERT INTO CodingSessions
                        (Project, StartTime, EndTime, Duration)
                    VALUES
                        (@Project, @StartTime, @EndTime, @Duration)
                    """,
                    new
                    {
                        Project = project,
                        StartTime = startTime,
                        EndTime = endTime,
                        Duration = duration
                    });
            }
        }
    }

    private static string GetProjectForDate(
        DateTime date,
        Random random)
    {
        // Different projects become more prominent throughout the year.
        if (date < new DateTime(2026, 3, 1))
        {
            return random.Next(100) switch
            {
                < 40 => "C# Learning",
                < 65 => "LINQ Practice",
                < 80 => "HabitTracker",
                < 90 => "AI Experiments",
                _ => "Blauwbyte Website"
            };
        }

        if (date < new DateTime(2026, 5, 1))
        {
            return random.Next(100) switch
            {
                < 45 => "HabitTracker",
                < 70 => "C# Learning",
                < 85 => "Cute & Creative Crafts",
                < 95 => "LINQ Practice",
                _ => "AI Experiments"
            };
        }

        if (date < new DateTime(2026, 7, 1))
        {
            return random.Next(100) switch
            {
                < 40 => "Cute & Creative Crafts",
                < 70 => "Blauwbyte Website",
                < 90 => "Accriva",
                _ => "C# Learning"
            };
        }

        if (date < new DateTime(2026, 9, 1))
        {
            return random.Next(100) switch
            {
                < 45 => "Accriva",
                < 75 => "Blauwbyte Website",
                < 90 => "CodingTracker",
                _ => "AI Experiments"
            };
        }

        // September: CodingTracker becomes the dominant project.
        return random.Next(100) switch
        {
            < 70 => "CodingTracker",
            < 85 => "Accriva",
            < 95 => "C# Learning",
            _ => "AI Experiments"
        };
    }

    private static int GenerateDuration(Random random)
    {
        double roll = random.NextDouble();

        return roll switch
        {
            // 1–30 minutes
            < 0.20 => random.Next(15, 31),

            // 31–60 minutes
            < 0.45 => random.Next(31, 61),

            // 61–90 minutes
            < 0.70 => random.Next(61, 91),

            // 91–120 minutes
            < 0.85 => random.Next(91, 121),

            // 121–150 minutes
            < 0.95 => random.Next(121, 151),

            // 151–240 minutes
            < 0.99 => random.Next(151, 241),

            // Occasionally a long coding session
            _ => random.Next(241, 361)
        };
    }
}