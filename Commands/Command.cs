using System.Globalization;

namespace Coding.Tracker.Commands
{
    public abstract class Command
    {
        protected CodeSessionService service;
        protected Command(CodeSessionService service)
        {
            this.service = service;
        }
        public abstract void Execute();

        protected static DateTime ValidateDateTime(string input)
        {
            var valid = DateTime.TryParseExact(
                input,
                "dd-MM-yy HH:mm:ss",
                new CultureInfo("en-US"),
                DateTimeStyles.None, out DateTime result);

            if (valid)
            {
                return result;
            }
            else throw new FormatException($"Wrong format of value: {input}! Should be dd-MM-yy HH:mm:ss");
        }

        protected static int ValidateNumeric(string input)
        {
            var valid = int.TryParse(input, out int result);
            if (valid)
            {
                return result;
            }
            else throw new FormatException($"Wrong format of value: {input}! Should be numeric");
        }
    }
}
