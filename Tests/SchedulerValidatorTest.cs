using Domain;
using Xunit;
using FluentAssertions;
using System;

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
        [InlineData(1)]
        [InlineData(10)]
        public void validate_recurring_configuration_correct_test(int? periodicity)
        {
            Action action = () => SchedulerValidator.ValidateRecurringConfiguration(periodicity);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
 
        [InlineData(0)]
        [InlineData(null)]
        [InlineData(-1)]

        public void validate_recurring_configuration_error_test( int? periodicity)
        {
            Action action = () => SchedulerValidator.ValidateRecurringConfiguration(periodicity);
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
            action.Should().Throw<ValidatorException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-100)]
        public void validate_once_time_not_null_correct_test(int seconds)
        {
            TimeSpan time = DateTime.Now.TimeOfDay.Add(TimeSpan.FromSeconds(seconds));
            Action action = () => SchedulerValidator.ValidateOnceTimeNotNull(time);
            action.Should().NotThrow<ValidatorException>();
        }

        [Fact]
        public void validate_once_time_not_null_error_test()
        {
            Action action = () => SchedulerValidator.ValidateOnceTimeNotNull(null);
            action.Should().Throw<ValidatorException>();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 10)]
        public void validate_starting_ending_daily_correct_test(int startOffset, int endOffset)
        {
            var startTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromSeconds(startOffset)); 
            var endTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromSeconds(endOffset));
            Action action = () => SchedulerValidator.ValidateStartingEndingDaily(startTime, endTime);
            action.Should().NotThrow<ValidatorException>();
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(0, -10)]
        public void validate_starting_ending_daily_error_test(int startOffset, int endOffset)
        {
            var startTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromSeconds(startOffset));
            var endTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromSeconds(endOffset));
            Action action = () => SchedulerValidator.ValidateStartingEndingDaily(startTime, endTime);
            action.Should().Throw<ValidatorException>();
        }

        [Fact]
        public void validate_weekly_configuration_correct_test()
        {
            
            var listOfDays = new DayOfWeek[1]{ DayOfWeek.Friday};
            var weeklyConfig = new WeeklyConfiguration()
            {
                ActiveDays = listOfDays
            };
            Action action = () => SchedulerValidator.ValidateWeeklyConfiguration(weeklyConfig);
            action.Should().NotThrow<ValidatorException>();
        }

        [Fact]
        public void validate_weekly_configuration_error_test()
        {
            var weeklyConfig = new WeeklyConfiguration();
            Action action = () => SchedulerValidator.ValidateWeeklyConfiguration(weeklyConfig);
            action.Should().Throw<ValidatorException>();
        }
    }
}