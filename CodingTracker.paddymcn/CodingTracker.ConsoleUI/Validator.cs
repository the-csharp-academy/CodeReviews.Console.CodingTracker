using System.Globalization;
namespace CodingTracker.Console;

public class Validator
{
    public const string DateFormat = "dd-MM-yy"; 
    public const string TimeFormat = "HH:mm"; 
    public const string DateTimeFormat = "dd/MM/yyyy HH:mm";

    internal static bool IsValidTime(string? input, string format, out TimeOnly time)
    {
        return TimeOnly.TryParseExact(
            input?.Trim(),
            format,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out time);
    }
    
    public static bool TryParseDateTime(string? input, out DateTime result)
    {
        return DateTime.TryParseExact(
            input,
            DateTimeFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out result);
    }
    public static bool IsValidDate(string? input, string format, out DateTime date)
    {
        return DateTime.TryParseExact(
            input?.Trim(),
            format,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out date);
    }
    
    public static bool IsValidPositiveInt(string? input, out int number)
    {
        return int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out number)
               && number > 0;
    }

    public static bool IsValidDescription(string? input, int maxLength = 50)
    {
        return !string.IsNullOrWhiteSpace(input)
               && input.Trim().Length <= maxLength;
    }
}