using System.Globalization;

namespace CodingTracker
{
    public static class InputValidation
    {
        /// <summary>
        /// Checks if date is valid
        /// </summary>
        /// <param name="dateInput"></param>
        /// <returns></returns>
        public static bool ValidateDateTime(string dateInput, out DateTime dateValidated)
        {
            if (dateInput == null)
            {
                dateValidated = default;
                return false;
            }
            bool validDate = false;
            if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValidated))
            {
                validDate = true;
            }
            return validDate;
        }

        public static bool ValidateEndTime(DateTime startTime, DateTime endTime)
        {
            return (endTime - startTime).TotalSeconds >= 0;
        }
    }
}
