using System.Globalization;

namespace CodingTracker.Services
{
    public static class ValidationService
    {
        public static bool TryParseDate(string input, out DateTime date)
        {
            return DateTime.TryParseExact(
                input, 
                "MM/dd/yyyy HH:mm:ss", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None,
                out date);
        }

        public static bool StartSmallerThanEnd(DateTime startTime, DateTime EndTime)
        {
            return startTime < EndTime;
        }
    }
}
