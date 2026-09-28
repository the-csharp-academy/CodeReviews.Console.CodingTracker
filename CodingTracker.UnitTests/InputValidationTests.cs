using System.Globalization;

namespace CodingTracker.UnitTests
{
    public class InputValidationTests
    {
        private static readonly object[] ValidDateTimeCases =
        {
            new TestCaseData("2026-12-12 10:05:13", true),
            new TestCaseData("2025-13-12 20:12:26", false),
            new TestCaseData("2021-10-44 01:02:03", false),
            new TestCaseData("2021-10-12 60:70:80", false),

            new TestCaseData("2021-10-12", false),
            new TestCaseData("", false),
            new TestCaseData("asdf", false),
            new TestCaseData(null, false)
        };

        [TestCaseSource(nameof(ValidDateTimeCases))]
        public void ValidDateTime_ReturnsTrue(string inputDate, bool expectedResult)
        {
            bool isValidDate = InputValidation.ValidateDateTime(inputDate, out DateTime dateValidated);
            Assert.That(isValidDate, Is.EqualTo(expectedResult));
        }

        [TestCase("2026-12-12 10:05:13", "2026-12-12 10:05:14", true)]
        [TestCase("2026-12-12 10:05:13", "2026-12-12 10:05:13", true)]
        [TestCase("2026-12-12 10:05:13", "2026-12-12 10:05:12", false)]
        public void ValidEndTime_ReturnsTrue(string startDate, string endDate, bool expectedResult)
        {
            DateTime start = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime end = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
            bool isValidDate = InputValidation.ValidateEndTime(start, end);
            Assert.That(isValidDate, Is.EqualTo(expectedResult));
        }
    }
}
