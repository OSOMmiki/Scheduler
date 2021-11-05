using Domain;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class DailyConfigurationTest
    {
        [Theory]
        [InlineData(1, DailyFrecuencyEnum.Seconds, 1)]
        [InlineData(1, DailyFrecuencyEnum.Minutes, 60)]
        [InlineData(1, DailyFrecuencyEnum.Hours, 3600)]
        [InlineData(5, DailyFrecuencyEnum.Seconds, 5)]
        [InlineData(5, DailyFrecuencyEnum.Minutes, 300)]
        [InlineData(5, DailyFrecuencyEnum.Hours, 18000)]

        public void get_total_seconds_periodicity_test(int periodiciy, DailyFrecuencyEnum dailyFrecuency, int expectedResult)
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
