using Domain;
using Xunit;
using FluentAssertions;
namespace Tests
{
    public class SchedulerValidatorTest
    {
        [Fact]
        public void validate_configuration_enabled_false_test()
        {
            Action action = () => ConfigurationValidator.ValidateConfigurationEnabled(false);
            action.Should().Throw<ValidatorException>();
        }

        [Fact]
        public void validate_configuration_enabled_true_test()
        {
            Action action = () => ConfigurationValidator.ValidateConfigurationEnabled(true);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void validate_recurring_configuration_correct_test(int? periodicity)
        {
            Action action = () => ConfigurationValidator.ValidateRecurringConfiguration(periodicity);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
 
        [InlineData(0)]
        [InlineData(null)]
        [InlineData(-1)]

        public void validate_recurring_configuration_error_test( int? periodicity)
        {
            Action action = () => ConfigurationValidator.ValidateRecurringConfiguration(periodicity);
            action.Should().Throw<ValidatorException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-100)]
        public void validate_once_configuration_correct_test(int days)
        {
            Action action = () => ConfigurationValidator.ValidateOnceConfiguration(DateTime.Today.AddDays(days));
            action.Should().NotThrow<ValidatorException>();
        }

        [Fact]
        public void validate_once_configuration_error_test()
        {
            Action action = () => ConfigurationValidator.ValidateOnceConfiguration(null);
            action.Should().Throw<ValidatorException>();
        }
        [Fact]
        public void validate_limit_end_date_null_correct_test()
        {
            Action action = () => ConfigurationValidator.ValidateLimits(DateOnly.MinValue, null);
            action.Should().NotThrow<ValidatorException>();
        }
        [Theory]
        [InlineData(0,0)]
        [InlineData(0,10)]
        public void validate_limit_correct_test(int startDays, int endDays)
        {
            var startDate = DateOnly.FromDateTime(DateTime.Today).AddDays(startDays);
            var endDate = DateOnly.FromDateTime(DateTime.Today).AddDays(endDays);  
            Action action = () => ConfigurationValidator.ValidateLimits(startDate, endDate);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(0, -10)]
        public void validate_limit_error_test(int startDays, int endDays)
        {
            var startDate = DateOnly.FromDateTime(DateTime.Today).AddDays(startDays);
            var endDate = DateOnly.FromDateTime(DateTime.Today).AddDays(endDays);
            Action action = () => ConfigurationValidator.ValidateLimits(startDate, endDate);
            action.Should().Throw<ValidatorException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-100)]
        public void validate_date_between_nulls_test(int days)
        {
            var date = DateTime.Today.AddDays(days);
            Action action = () => ConfigurationValidator.ValidateDateBetweenLimits(DateOnly.MinValue, null, date);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(0,0,0)]
        [InlineData(-5, 5, 0)]
        [InlineData(-50, 0, -25)]
        [InlineData(0, 50, 25)]
        public void validate_date_between_limits_correct_test(int startDays, int endDays, int days)
        {
            var startDate = DateOnly.FromDateTime(DateTime.Today).AddDays(startDays);
            var endDate = DateOnly.FromDateTime(DateTime.Today).AddDays(endDays);
            var date = DateTime.Today.AddDays(days);
            Action action = () => ConfigurationValidator.ValidateDateBetweenLimits(startDate, endDate, date);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(0, 0, 5)]
        [InlineData(0, 0, -5)]
        [InlineData(-5, 50, -25)]
        [InlineData(-50, 5, 25)]
        public void validate_date_between_limits_error_test(int startDays, int endDays, int days)
        {
            var startDate = DateOnly.FromDateTime(DateTime.Today).AddDays(startDays);
            var endDate = DateOnly.FromDateTime(DateTime.Today).AddDays(endDays);
            var date = DateTime.Today.AddDays(days);
            Action action = () => ConfigurationValidator.ValidateDateBetweenLimits(startDate, endDate, date);
            action.Should().Throw<ValidatorException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-100)]
        public void validate_once_time_not_null_correct_test(int seconds)
        {
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now.AddSeconds(seconds));
            Action action = () => ConfigurationValidator.ValidateOnceTimeNotNull(time);
            action.Should().NotThrow<ValidatorException>();
        }

        [Fact]
        public void validate_once_time_not_null_error_test()
        {
            Action action = () => ConfigurationValidator.ValidateOnceTimeNotNull(null);
            action.Should().Throw<ValidatorException>();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 10)]
        public void validate_starting_ending_daily_correct_test(int startOffset, int endOffset)
        {
            var startTime = TimeOnly.FromDateTime(DateTime.Now.AddSeconds(startOffset));
            var endTime = TimeOnly.FromDateTime(DateTime.Now.AddSeconds(endOffset));
            Action action = () => ConfigurationValidator.ValidateStartingEndingDaily(startTime, endTime);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(0, -10)]
        public void validate_starting_ending_daily_error_test(int startOffset, int endOffset)
        {
            var startTime = TimeOnly.FromDateTime(DateTime.Now.AddSeconds(startOffset));
            var endTime = TimeOnly.FromDateTime(DateTime.Now.AddSeconds(endOffset));
            Action action = () => ConfigurationValidator.ValidateStartingEndingDaily(startTime, endTime);
            action.Should().Throw<ValidatorException>();
        }

        [Fact]
        public void validate_weekly_configuration_correct_test()
        {
            var listOfDays = new DayWeek[1]{ DayWeek.Friday};
            Action action = () => ConfigurationValidator.ValidateWeeklyConfiguration(listOfDays);
            action.Should().NotThrow<ValidatorException>();
        }

        [Fact]
        public void validate_weekly_configuration_error_test()
        {
            var listODays = Array.Empty<DayWeek>();
            Action action = () => ConfigurationValidator.ValidateWeeklyConfiguration(listODays);
            action.Should().Throw<ValidatorException>();
        }
    }
}