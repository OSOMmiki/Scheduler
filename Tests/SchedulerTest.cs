using Xunit;
using FluentAssertions;
using Domain;
using Xunit.Abstractions;

namespace Tests
{
    public class SchedulerTest
    {
        private readonly ITestOutputHelper output;
        public SchedulerTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void scheduler_not_enabled_test()
        {
            var configuration = new Configuration()
            {
                Enabled = false
            };

            Action action = () => Scheduler.NextScheduleResult(configuration);

            action.Should()
                .ThrowExactly<ValidatorException>()
                .WithMessage("Scheduler Is Disabled");

            this.output.WriteLine("Scheduler Is Disabled");
        }

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

            this.output.WriteLine($"Occurs once. Scheduler will be used on 10/10/2021 at 17:30:00 starting on 10/10/2021");
        }

        #region DailyRecurringOnceTime
        [Fact]
        public void daily_recurring_scheduler_with_once_time_after_current_time_test()
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
            results[9].Description.Should().Be("Occurs every day at 12:00:00. Scheduler will be used on 19/10/2021 at 12:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every day at 12:00:00. Scheduler will be used on 19/10/2021 starting on 10/10/2021 ending on 20/10/2021");
        }

        [Fact]
        public void daily_recurring_scheduler_with_once_time_before_current_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                ConfigurationType = ConfigurationType.Recurring,
                CurrentDate = new DateTime(2021, 10, 10, 12, 0, 0),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                DailyConfType = ConfigurationType.Once,
                DailyConfOnceTime = new TimeOnly(12, 0, 0),
                RecurringType = RecurringType.Daily,
                Periodicity = 1
            };

            var results = Scheduler.ScheduleMulitple(configuration, 10);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 12, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 12, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 12, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 12, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 12, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 16, 12, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 17, 12, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 18, 12, 0, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 19, 12, 0, 0));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 12, 0, 0));
            results[9].Description.Should().Be("Occurs every day at 12:00:00. Scheduler will be used on 20/10/2021 at 12:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every day at 12:00:00. Scheduler will be used on 20/10/2021 starting on 10/10/2021 ending on 20/10/2021");
        }
        #endregion

        #region WeeklyRecurringOnceTime
        [Fact]
        public void weekly_recurring_scheduler_with_once_time_after_current_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                ConfigurationType = ConfigurationType.Recurring,
                CurrentDate = new DateTime(2021, 10, 11, 0, 0, 0),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 11, 20),
                DailyConfType = ConfigurationType.Once,
                DailyConfOnceTime = new TimeOnly(12, 0, 0),
                RecurringType = RecurringType.Weekly,
                WeeklyConfigActiveDays = new DayWeek[7]
                {
                    DayWeek.Monday,
                    DayWeek.Tuesday,
                    DayWeek.Wednesday,
                    DayWeek.Thursday,
                    DayWeek.Friday,
                    DayWeek.Saturday,
                    DayWeek.Sunday
                },
                WeeklyConfigPeriodicity = 2,
                Periodicity = 1
            };

            var results = Scheduler.ScheduleMulitple(configuration, 14);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 12, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 12, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 12, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 12, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 12, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 16, 12, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 17, 12, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 25, 12, 0, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 26, 12, 0, 0));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 27, 12, 0, 0));
            results[10].ScheduledDate.Should().Be(new DateTime(2021, 10, 28, 12,0,0));
            results[11].ScheduledDate.Should().Be(new DateTime(2021, 10, 29, 12, 0, 0));
            results[12].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 12, 0, 0));
            results[13].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 12, 0, 0));
            results[13].Description.Should().Be("Occurs every 2 weeks on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday and Sunday at 12:00:00. Scheduler will be used on 31/10/2021 at 12:00:00 starting on 10/10/2021 ending on 20/11/2021");

            this.output.WriteLine("Occurs every 2 weeks on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday and Sunday at 12:00:00. Scheduler will be used on 31/10/2021 at 12:00:00 starting on 10/10/2021 ending on 20/11/2021");
        }

        [Fact]
        public void weekly_recurring_scheduler_with_once_time_before_current_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                ConfigurationType = ConfigurationType.Recurring,
                CurrentDate = new DateTime(2021, 10, 11, 12, 0, 0),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 11, 20),
                DailyConfType = ConfigurationType.Once,
                DailyConfOnceTime = new TimeOnly(12, 0, 0),
                RecurringType = RecurringType.Weekly,
                WeeklyConfigActiveDays = new DayWeek[7]
                {
                    DayWeek.Monday,
                    DayWeek.Tuesday,
                    DayWeek.Wednesday,
                    DayWeek.Thursday,
                    DayWeek.Friday,
                    DayWeek.Saturday,
                    DayWeek.Sunday
                },
                WeeklyConfigPeriodicity = 2,
                Periodicity = 1
            };

            var results = Scheduler.ScheduleMulitple(configuration, 14);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 12, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 12, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 12, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 12, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 16, 12, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 17, 12, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 25, 12, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 26, 12, 0, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 27, 12, 0, 0));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 28, 12, 0, 0));
            results[10].ScheduledDate.Should().Be(new DateTime(2021, 10, 29, 12, 0, 0));
            results[11].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 12, 0, 0));
            results[12].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 12, 0, 0));
            results[13].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 12, 0, 0));
            results[13].Description.Should().Be("Occurs every 2 weeks on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday and Sunday at 12:00:00. Scheduler will be used on 08/11/2021 at 12:00:00 starting on 10/10/2021 ending on 20/11/2021");

            this.output.WriteLine("Occurs every 2 weeks on Monday, Tuesday, Wednesday, Thursday, Friday, Saturday and Sunday at 12:00:00. Scheduler will be used on 08/11/2021 at 12:00:00 starting on 10/10/2021 ending on 20/11/2021");
        }

        #endregion

        #region DailyRecurringHoursRecurring
        [Fact]
        public void daily_recurring_scheduler_every_2_hours_current_time_before_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,

                CurrentDate = new DateTime(2021, 10, 10, 0, 0, 0),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Daily,
                Periodicity = 2,
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Hours,
                DailyConfPeriodicity = 2,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(22, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 9);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 20, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 22, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 18, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 20, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 22, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 20, 0, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 22, 0, 0));
            results[8].Description.Should().Be("Occurs every 2 days every 2 hours between 18:00:00 and 22:00:00. Scheduler will be used on 14/10/2021 at 22:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every 2 days every 2 hours between 18:00:00 and 22:00:00. Scheduler will be used on 14/10/2021 at 22:00:00 starting on 10/10/2021 ending on 20/10/2021");
        }

        [Fact]
        public void daily_recurring_scheduler_every_2_hours_current_time_after_ending_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 19, 12, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Daily,
                Periodicity = 2,
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Hours,
                DailyConfPeriodicity = 2,
                DailyConfStartingTime = new TimeOnly(12, 0, 0),
                DailyConfEndingTime = new TimeOnly(16, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 9);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 12, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 14, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 16, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 12, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 14, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 16, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 16, 12, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 16, 14, 0, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 16, 16, 0, 0));
            results[8].Description.Should().Be("Occurs every 2 days every 2 hours between 12:00:00 and 16:00:00. Scheduler will be used on 16/10/2021 at 16:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every 2 days every 2 hours between 12:00:00 and 16:00:00. Scheduler will be used on 16/10/2021 at 16:00:00 starting on 10/10/2021 ending on 20/10/2021");
        }

        [Fact]
        public void daily_recurring_scheduler_every_2_hours_current_time_after_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 19, 12, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Daily,
                Periodicity = 2,
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Hours,
                DailyConfPeriodicity = 2,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(22, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 9);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 20, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 22, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 18, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 20, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 12, 22, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 20, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 22, 0, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 16, 18, 0, 0));
            results[8].Description.Should().Be("Occurs every 2 days every 2 hours between 18:00:00 and 22:00:00. Scheduler will be used on 16/10/2021 at 18:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every 2 days every 2 hours between 18:00:00 and 22:00:00. Scheduler will be used on 16/10/2021 at 18:00:00 starting on 10/10/2021 ending on 20/10/2021");
        }
        #endregion

        #region DailyRecurringMinutesRecurring
        [Fact]
        public void daily_recurring_scheduler_every_20_minutes_current_time_before_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 0, 0, 0),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Daily,
                Periodicity = 10,
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Minutes,
                DailyConfPeriodicity = 20,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(19, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 8);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 20, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 40, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 19, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 18, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 18, 20, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 18, 40, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 19, 0, 0));
            results[7].Description.Should().Be("Occurs every 10 days every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 20/10/2021 at 19:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every 10 days every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 20/10/2021 at 19:00:00 starting on 10/10/2021 ending on 20/10/2021");
        }

        [Fact]
        public void daily_recurring_scheduler_every_20_minutes_current_time_after_ending_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 19, 12, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Daily,
                Periodicity = 10,
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Minutes,
                DailyConfPeriodicity = 20,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(19, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 4);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 18, 20, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 18, 40, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 19, 0, 0));
            results[3].Description.Should().Be("Occurs every 10 days every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 20/10/2021 at 19:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every 10 days every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 20/10/2021 at 19:00:00 starting on 10/10/2021 ending on 20/10/2021");
        }

        [Fact]
        public void daily_recurring_scheduler_every_20_minutes_current_time_after_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 18, 12, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Daily,
                Periodicity = 10,
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Minutes,
                DailyConfPeriodicity = 20,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(19, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 7);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 20, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 40, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 19, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 18, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 18, 20, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 18, 40, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 19, 0, 0));
            results[6].Description.Should().Be("Occurs every 10 days every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 20/10/2021 at 19:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every 10 days every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 20/10/2021 at 19:00:00 starting on 10/10/2021 ending on 20/10/2021");
        }
        #endregion

        #region DailyRecurringSecondsRecurring
        [Fact]
        public void daily_recurring_scheduler_every_90_seconds_current_time_before_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 0, 0, 0),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Daily,
                Periodicity = 4,
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Seconds,
                DailyConfPeriodicity = 90,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(18, 6, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 10);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 1, 30));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 3, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 4, 30));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 6, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 1, 30));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 3, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 4, 30));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 6, 0));
            results[9].Description.Should().Be("Occurs every 4 days every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 14/10/2021 at 18:06:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every 4 days every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 14/10/2021 at 18:06:00 starting on 10/10/2021 ending on 20/10/2021");
        }

        [Fact]
        public void daily_recurring_scheduler_every_90_seconds_current_time_after_ending_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 18, 12, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Daily,
                Periodicity = 4,
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Seconds,
                DailyConfPeriodicity = 90,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(18, 6, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 5);


            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 1, 30));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 3, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 4, 30));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 6, 0));
            results[4].Description.Should().Be("Occurs every 4 days every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 14/10/2021 at 18:06:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every 4 days every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 14/10/2021 at 18:06:00 starting on 10/10/2021 ending on 20/10/2021");
        }

        [Fact]
        public void daily_recurring_scheduler_every_90_seconds_current_time_after_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 18, 2, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Daily,
                Periodicity = 4,
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Seconds,
                DailyConfPeriodicity = 90,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(18, 6, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 8);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 3, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 4, 30));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 6, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 1, 30));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 3, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 4, 30));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 14, 18, 6, 0));
            results[7].Description.Should().Be("Occurs every 4 days every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 14/10/2021 at 18:06:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every 4 days every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 14/10/2021 at 18:06:00 starting on 10/10/2021 ending on 20/10/2021");
        }
        #endregion

        #region WeeklyRecurringHoursRecurring
        [Fact]
        public void weekly_recurring_scheduler_every_2_hours_current_time_before_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,

                CurrentDate = new DateTime(2021, 10, 10, 0, 0, 0),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Weekly,
                WeeklyConfigPeriodicity = 1,
                WeeklyConfigActiveDays = new DayWeek[3]
                {
                    DayWeek.Monday,
                    DayWeek.Wednesday,
                    DayWeek.Friday
                },
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Hours,
                DailyConfPeriodicity = 2,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(22, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 10);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 20, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 22, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 18, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 20, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 22, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 18, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 20, 0, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 22, 0, 0));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 18, 18, 0, 0));
            results[9].Description.Should().Be("Occurs every week on Monday, Wednesday and Friday every 2 hours between 18:00:00 and 22:00:00. Scheduler will be used on 18/10/2021 at 18:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every week on Monday, Wednesday and Friday every 2 hours between 18:00:00 and 22:00:00. Scheduler will be used on 18/10/2021 at 18:00:00 starting on 10/10/2021 ending on 20/10/2021");
        }

        [Fact]
        public void weekly_recurring_scheduler_every_2_hours_current_time_after_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 11, 19, 12, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Weekly,
                WeeklyConfigPeriodicity = 1,
                WeeklyConfigActiveDays = new DayWeek[3]
                {
                    DayWeek.Monday,
                    DayWeek.Wednesday,
                    DayWeek.Friday
                },
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Hours,
                DailyConfPeriodicity = 2,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(22, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 10);

            
            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 20, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 22, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 18, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 20, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 22, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 18, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 20, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 22, 0, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 18, 18, 0, 0));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 18, 20, 0, 0));
            results[9].Description.Should().Be("Occurs every week on Monday, Wednesday and Friday every 2 hours between 18:00:00 and 22:00:00. Scheduler will be used on 18/10/2021 at 20:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every week on Monday, Wednesday and Friday every 2 hours between 18:00:00 and 22:00:00. Scheduler will be used on 18/10/2021 at 20:00:00 starting on 10/10/2021 ending on 20/10/2021");
        }

        [Fact]
        public void weekly_recurring_scheduler_every_2_hours_current_time_after_ending_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 11, 23, 12, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 10, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Weekly,
                WeeklyConfigPeriodicity = 1,
                WeeklyConfigActiveDays = new DayWeek[3]
                {
                    DayWeek.Monday,
                    DayWeek.Wednesday,
                    DayWeek.Friday
                },
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Hours,
                DailyConfPeriodicity = 2,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(22, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 10);
            
            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 20, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 13, 22, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 18, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 20, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 15, 22, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 18, 18, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 18, 20, 0, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 18, 22, 0, 0));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 20, 18, 0, 0));
            results[9].Description.Should().Be("Occurs every week on Monday, Wednesday and Friday every 2 hours between 18:00:00 and 22:00:00. Scheduler will be used on 20/10/2021 at 18:00:00 starting on 10/10/2021 ending on 20/10/2021");

            this.output.WriteLine("Occurs every week on Monday, Wednesday and Friday every 2 hours between 18:00:00 and 22:00:00. Scheduler will be used on 20/10/2021 at 18:00:00 starting on 10/10/2021 ending on 20/10/2021");
        }
        #endregion

        #region WeeklyRecurringMinutesRecurring
        [Fact]
        public void weekly_recurring_scheduler_every_20_minutes_current_time_before_starting_time_test()
        {
            var configuration = new Configuration()
            {

                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 0, 0, 0),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 12, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Weekly,
                WeeklyConfigPeriodicity = 4,
                WeeklyConfigActiveDays = new DayWeek[1]
                {
                    DayWeek.Monday
                },
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Minutes,
                DailyConfPeriodicity = 20,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(19, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 8);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 18, 20, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 18, 40, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 19, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 18, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 18, 20, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 18, 40, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 19, 0, 0));
            results[7].Description.Should().Be("Occurs every 4 weeks on Monday every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 08/11/2021 at 19:00:00 starting on 10/10/2021 ending on 20/12/2021");

            this.output.WriteLine("Occurs every 4 weeks on Monday every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 08/11/2021 at 19:00:00 starting on 10/10/2021 ending on 20/12/2021");
        }

        [Fact]
        public void weekly_recurring_scheduler_every_20_minutes_current_time_after_ending_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 11, 19, 12, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 12, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Weekly,
                WeeklyConfigPeriodicity = 4,
                WeeklyConfigActiveDays = new DayWeek[1]
                {
                    DayWeek.Monday
                },
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Minutes,
                DailyConfPeriodicity = 20,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(19, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 8);

            
            results[0].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 18, 20, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 18, 40, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 19, 0, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 12, 06, 18, 0, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 12, 06, 18, 20, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 12, 06, 18, 40, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 12, 06, 19, 0, 0));
            results[7].Description.Should().Be("Occurs every 4 weeks on Monday every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 06/12/2021 at 19:00:00 starting on 10/10/2021 ending on 20/12/2021");

            this.output.WriteLine("Occurs every 4 weeks on Monday every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 06/12/2021 at 19:00:00 starting on 10/10/2021 ending on 20/12/2021");
        }

        [Fact]
        public void weekly_recurring_scheduler_every_20_minutes_current_time_after_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 11, 18, 32, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 12, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Weekly,
                WeeklyConfigPeriodicity = 4,
                WeeklyConfigActiveDays = new DayWeek[1]
                {
                    DayWeek.Monday
                },
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Minutes,
                DailyConfPeriodicity = 20,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(19, 0, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 8);

            
            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 18, 40, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 11, 19, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 18, 0, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 18, 20, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 18, 40, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 11, 8, 19, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 12, 06, 18, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 12, 06, 18, 20, 0));
            results[7].Description.Should().Be("Occurs every 4 weeks on Monday every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 06/12/2021 at 18:20:00 starting on 10/10/2021 ending on 20/12/2021");

            this.output.WriteLine("Occurs every 4 weeks on Monday every 20 minutes between 18:00:00 and 19:00:00. Scheduler will be used on 06/12/2021 at 18:20:00 starting on 10/10/2021 ending on 20/12/2021");
        }
        #endregion

        #region WeeklyRecurringSecondsRecurring
        [Fact]
        public void weekly_recurring_scheduler_every_90_seconds_current_time_before_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 0, 0, 0),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 12, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Weekly,
                WeeklyConfigPeriodicity = 3,
                WeeklyConfigActiveDays = new DayWeek[2]
                {
                    DayWeek.Saturday,
                    DayWeek.Sunday
                },
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Seconds,
                DailyConfPeriodicity = 90,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(18, 6, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 13);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 1, 30));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 3, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 4, 30));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 6, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 1, 30));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 3, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 4, 30));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 6, 0));
            results[10].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 0, 0));
            results[11].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 1, 30));
            results[12].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 3, 0));
            results[12].Description.Should().Be("Occurs every 3 weeks on Saturday and Sunday every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 31/10/2021 at 18:03:00 starting on 10/10/2021 ending on 20/12/2021");

            this.output.WriteLine("Occurs every 3 weeks on Saturday and Sunday every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 14/10/2021 at 18:06:00 starting on 10/10/2021 ending on 20/12/2021");
        }

        [Fact]
        public void weekly_recurring_scheduler_every_90_seconds_current_time_after_ending_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 18, 12, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 12, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Weekly,
                WeeklyConfigPeriodicity = 3,
                WeeklyConfigActiveDays = new DayWeek[2]
                {
                    DayWeek.Saturday,
                    DayWeek.Sunday
                },
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Seconds,
                DailyConfPeriodicity = 90,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(18, 6, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 13);

            
            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 0, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 1, 30));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 3, 0));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 4, 30));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 6, 0));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 0, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 1, 30));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 3, 0));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 4, 30));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 6, 0));
            results[10].ScheduledDate.Should().Be(new DateTime(2021, 11, 20, 18, 0, 0));
            results[11].ScheduledDate.Should().Be(new DateTime(2021, 11, 20, 18, 1, 30));
            results[12].ScheduledDate.Should().Be(new DateTime(2021, 11, 20, 18, 3, 0));
            results[12].Description.Should().Be("Occurs every 3 weeks on Saturday and Sunday every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 20/11/2021 at 18:03:00 starting on 10/10/2021 ending on 20/12/2021");

            this.output.WriteLine("Occurs every 3 weeks on Saturday and Sunday every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 20/11/2021 at 18:03:00 starting on 10/10/2021 ending on 20/12/2021");
        }

        [Fact]
        public void weekly_recurring_scheduler_every_90_seconds_current_time_after_starting_time_test()
        {
            var configuration = new Configuration()
            {
                Enabled = true,
                CurrentDate = new DateTime(2021, 10, 10, 18, 5, 43),
                StartDate = new DateOnly(2021, 10, 10),
                EndDate = new DateOnly(2021, 12, 20),
                ConfigurationType = ConfigurationType.Recurring,
                RecurringType = RecurringType.Weekly,
                WeeklyConfigPeriodicity = 3,
                WeeklyConfigActiveDays = new DayWeek[2]
                {
                    DayWeek.Saturday,
                    DayWeek.Sunday
                },
                DailyConfType = ConfigurationType.Recurring,
                DailyConfFrecuency = DailyFrecuency.Seconds,
                DailyConfPeriodicity = 90,
                DailyConfStartingTime = new TimeOnly(18, 0, 0),
                DailyConfEndingTime = new TimeOnly(18, 6, 0)
            };

            var results = Scheduler.ScheduleMulitple(configuration, 13);

            results[0].ScheduledDate.Should().Be(new DateTime(2021, 10, 10, 18, 6, 0));
            results[1].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 0, 0));
            results[2].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 1, 30));
            results[3].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 3, 0));
            results[4].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 4, 30));
            results[5].ScheduledDate.Should().Be(new DateTime(2021, 10, 30, 18, 6, 0));
            results[6].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 0, 0));
            results[7].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 1, 30));
            results[8].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 3, 0));
            results[9].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 4, 30));
            results[10].ScheduledDate.Should().Be(new DateTime(2021, 10, 31, 18, 6, 0));
            results[11].ScheduledDate.Should().Be(new DateTime(2021, 11, 20, 18, 0, 0));
            results[12].ScheduledDate.Should().Be(new DateTime(2021, 11, 20, 18, 1, 30));
            results[12].Description.Should().Be("Occurs every 3 weeks on Saturday and Sunday every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 20/11/2021 at 18:01:30 starting on 10/10/2021 ending on 20/12/2021");

            this.output.WriteLine("Occurs every 3 weeks on Saturday and Sunday days every 90 seconds between 18:00:00 and 18:06:00. Scheduler will be used on 20/11/2021 at 18:01:30 starting on 10/10/2021 ending on 20/12/2021");
        }
        #endregion
    }
}
