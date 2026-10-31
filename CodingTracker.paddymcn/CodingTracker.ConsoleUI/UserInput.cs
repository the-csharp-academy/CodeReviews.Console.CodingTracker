using Spectre.Console;
namespace CodingTracker.Console;
public class UserInput
{
    public DateTime GetDateTime(string message)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>($"{Markup.Escape(message)} [grey](format: {Validator.DateTimeFormat})[/]")
                .Validate(text => Validator.TryParseDateTime(text, out _)
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"[red]Invalid date and time. Use {Validator.DateTimeFormat}.[/]")));

        Validator.TryParseDateTime(input, out var dateTime);
        return dateTime;
    }
    public int GetPositiveInt(string message)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>(Markup.Escape(message))
                .Validate(text => Validator.IsValidPositiveInt(text, out _)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Please enter a whole number greater than 0.[/]")));

        Validator.IsValidPositiveInt(input, out var number);
        return number;
    }
    public string GetDescription(string message, int maxLength = 250)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>(Markup.Escape(message))
                .Validate(text => Validator.IsValidDescription(text, maxLength)
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"[red]Name can't be empty or longer than {maxLength} characters.[/]")));

        return input.Trim();
    }
}   