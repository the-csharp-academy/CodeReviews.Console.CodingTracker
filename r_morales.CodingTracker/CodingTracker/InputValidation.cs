using System.Globalization;

namespace CodingTracker.Validation 
{
    public class InputValidation
    {
        public static bool ValidateTimeInput(string input, out TimeOnly time)
        {
            return TimeOnly.TryParseExact(input, "HH:mm", out time);
        }

        public static bool ValidateDateInput(string input, out DateOnly date)
        {
            return DateOnly.TryParseExact(input, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }
    }
}
