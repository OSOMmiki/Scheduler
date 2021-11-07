using Domain;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class DailyConfigurationTest
    {
        [Theory]
        [InlineData(1, DailyFrecuency.Seconds, 1)]
        [InlineData(1, DailyFrecuency.Minutes, 60)]
        [InlineData(1, DailyFrecuency.Hours, 3600)]
        [InlineData(5, DailyFrecuency.Seconds, 5)]
        [InlineData(5, DailyFrecuency.Minutes, 300)]
        [InlineData(5, DailyFrecuency.Hours, 18000)]

        public void get_total_seconds_periodicity_test(int periodiciy, DailyFrecuency dailyFrecuency, int expectedResult)
        {
            var dailyConfig = new DailyConfiguration()
            {
                Periodicity = periodiciy,
                DailyFrecuency = dailyFrecuency
            };
            int seconds = dailyConfig.GetTotalSecondsPeriocity();
            seconds.Should().Be(expectedResult);
        }
    }
}
