
using CodingTracker.Entities;
using CodingTracker.Services;

namespace CodingTracker.Tests
{
    public class CalendarServiceTests
    {
        [Theory]
        [InlineData(1, "░ ", "darkgreen")]
        [InlineData(30, "░ ", "darkgreen")]
        [InlineData(31, "▒ ", "darkgreen")]
        [InlineData(60, "▒ ", "darkgreen")]
        [InlineData(61, "▓ ", "darkgreen")]
        [InlineData(90, "▓ ", "darkgreen")]
        [InlineData(91, "█ ", "darkgreen")]
        [InlineData(120, "█ ", "darkgreen")]
        [InlineData(121, "▒ ", "green")]
        [InlineData(150, "▒ ", "green")]
        [InlineData(151, "█ ", "green")]
        [InlineData(160, "█ ", "green")]
        public void AssignSymbolAndColorAccordingToDuration_ExpectedResult(int duration, string symbol, string color)
        {
            var d = new CalendarDay
            {
                CodingMinutes = duration
            };

            CalendarServiceBuilder.AssignSymbolAndColor(d);

            Assert.Equal(symbol, d.ActivitySymbol);
            Assert.Equal(color, d.ActivityColor);
        }

        [Fact]
        public void AssignSymbolAndColorAccordingToDuration_DurationIsZero_ExpectedResult()
        {
            var d = new CalendarDay
            {
                CodingMinutes = 0
            };

            CalendarServiceBuilder.AssignSymbolAndColor(d);

            Assert.Equal("gray", d.ActivityColor);
        }
    }
}
