using Xunit;
using FluentAssertions;
using Domain;

namespace Tests
{
    public class SchedulerTest
    {
        [Fact]
        public void once_scheduler_test()
        {
            var configuration = new Configuration()
            {
                ConfigurationType = ConfigurationType.Once,
                OnceDate = new DateTime(2021, 10, 10, 17, 30, 0),
                StartDate = new DateOnly(2021, 10, 10),
                Enabled = true
            };

            var result = Scheduler.NextScheduleResult(configuration);

            result.ScheduledDate.Should().Be(configuration.OnceDate.Value);
            result.Description.Should().Be($"Occurs once. Scheduler will be used on 10/10/2021 at 17:30:00 starting on 10/10/2021");
        }

        [Fact]
        public void daily_recurring_scheduler_with_once_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                ConfigurationType = ConfigurationType.Recurring,
                CurrentDate = new DateTime(2021, 10, 10,0,0,0),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                DailyConfType = ConfigurationType.Once,
                DailyConfOnceTime = new TimeOnly(12,0,0),
                RecurringType = RecurringType.Daily,
                Periodicity = 1   
            };

            var results = Scheduler.ScheduleMulitple(configuration, 10);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 12, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 12, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 12, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 12, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 12, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 12, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 16, 12, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 17, 12, 0, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 18, 12, 0, 0));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 19, 12, 0, 0));

        }
    }
}
