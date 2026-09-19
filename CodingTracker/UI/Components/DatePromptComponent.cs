using Spectre.Console;
using CodingTracker.Services;

namespace CodingTracker.UI.Components
{
    public static class DatePromptComponent
    {
        public static DateTime GetDate()
        {
            DateTime date = default;

            AnsiConsole.Prompt(new TextPrompt<string>("Please enter the days date:" +
                "\nExpected date format: [blue]MM/dd/yyyy HH:mm:ss[/]")
                .Validate(x =>
                {
                    if (ValidationService.TryParseDate(x, out date))
                    {
                        return ValidationResult.Success();
                    }

                    return ValidationResult.Error("[red]Invalid date. Use MM/dd/yyyy HH:mm:ss[/]");
                }));

            return date;
        }
    }
}
