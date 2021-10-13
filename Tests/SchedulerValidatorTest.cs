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
            Action action = () => SchedulerValidator.ValidateConfigurationEnabled(false);
            action.Should().Throw<ValidatorException>();
        }

        [Fact]
        public void validate_configuration_enabled_true_test()
        {
            Action action = () => SchedulerValidator.ValidateConfigurationEnabled(true);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(RecurringType.Daily,1)]
        [InlineData(RecurringType.Daily, 10)]
        public void validate_recurring_configuration_correct_test(RecurringType? recurringType, int? recurringDelay)
        {
            Action action = () => SchedulerValidator.ValidateRecurringConfiguration(recurringType,recurringDelay);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(null, 10)]
        [InlineData(RecurringType.Daily, 0)]
        [InlineData(RecurringType.Daily, null)]
        [InlineData(null, null)]
        public void validate_recurring_configuration_error_test(RecurringType? recurringType, int? recurringDelay)
        {
            Action action = () => SchedulerValidator.ValidateRecurringConfiguration(recurringType, recurringDelay);
            action.Should().Throw<ValidatorException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-100)]
        public void validate_once_configuration_correct_test(int days)
        {
            Action action = () => SchedulerValidator.ValidateOnceConfiguration(DateTime.Today.AddDays(days));
            action.Should().NotThrow<ValidatorException>();
        }

        [Fact]
        public void validate_once_configuration_error_test()
        {
            Action action = () => SchedulerValidator.ValidateOnceConfiguration(null);
            action.Should().Throw<ValidatorException>();
        }
        [Fact]
        public void validate_limit_start_date_null_correct_test()
        {
            Action action = () => SchedulerValidator.ValidateLimits(null,DateTime.Today);
            action.Should().NotThrow<ValidatorException>();
        }
        [Fact]
        public void validate_limit_end_date_null_correct_test()
        {
            Action action = () => SchedulerValidator.ValidateLimits( DateTime.Today, null);
            action.Should().NotThrow<ValidatorException>();
        }
        [Fact]
        public void validate_limit_star_end_date_null_correct_test()
        {
            Action action = () => SchedulerValidator.ValidateLimits(null, null);
            action.Should().NotThrow<ValidatorException>();
        }
        [Theory]
        [InlineData(0,0)]
        [InlineData(0,10)]
        public void validate_limit_correct_test(int startDays, int endDays)
        {
            var startDate = DateTime.Today.AddDays(startDays);
            var endDate = DateTime.Today.AddDays(endDays);  
            Action action = () => SchedulerValidator.ValidateLimits(startDate, endDate);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(0, -10)]
        public void validate_limit_error_test(int startDays, int endDays)
        {
            var startDate = DateTime.Today.AddDays(startDays);
            var endDate = DateTime.Today.AddDays(endDays);
            Action action = () => SchedulerValidator.ValidateLimits(startDate, endDate);
            action.Should().Throw<ValidatorException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-100)]
        public void validate_date_between_nulls_test(int days)
        {
            var date = DateTime.Today.AddDays(days);
            Action action = () => SchedulerValidator.ValidateDateBetweenLimits(null, null, date);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(0,0,0)]
        [InlineData(-5, 5, 0)]
        [InlineData(-50, 0, -25)]
        [InlineData(0, 50, 25)]
        public void validate_date_between_limits_correct_test(int startDays, int endDays, int days)
        {
            var startDate = DateTime.Today.AddDays(startDays);
            var endDate = DateTime.Today.AddDays(endDays);
            var date = DateTime.Today.AddDays(days);
            Action action = () => SchedulerValidator.ValidateDateBetweenLimits(startDate, endDate, date);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(0, 0, 5)]
        [InlineData(0, 0, -5)]
        [InlineData(-5, 50, -25)]
        [InlineData(-50, 5, 25)]
        public void validate_date_between_limits_error_test(int startDays, int endDays, int days)
        {
            var startDate = DateTime.Today.AddDays(startDays);
            var endDate = DateTime.Today.AddDays(endDays);
            var date = DateTime.Today.AddDays(days);
            Action action = () => SchedulerValidator.ValidateDateBetweenLimits(startDate, endDate, date);
            action.Should().NotThrow<ValidatorException>();
        }
    }
}