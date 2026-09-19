using CodingTracker.Services;

namespace CodingTracker.Tests
{
    public class ValidationServiceTests
    {

        [Theory]
        [InlineData(10, 11, true)]
        [InlineData(10, 10, false)]
        [InlineData(10, 09, false)]
        public void StartSmallerThanEnd_ReturnsExpectedResult(int startHour, int endHour, bool expected)
        {
            var start = new DateTime(2026, 9, 18, startHour, 0, 0);
            var end = new DateTime(2026, 9, 18, endHour, 0, 0);

            var result = ValidationService.StartSmallerThanEnd(start, end);

            Assert.Equal(expected, result);
        }
    }
}
