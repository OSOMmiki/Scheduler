using Domain;
using Xunit;
using FluentAssertions;
using System;

namespace Tests
{
    public class WeeklyConfigurationTest
    {
        [Theory]
        [ClassData(typeof(DayOfWeeekToSearchInDaysOfWeekData))]
        public void check_contains_day_of_week__test(DayOfWeek day, WeeklyConfiguration weeklyConfiguration, bool expected)
        {
            bool result = weeklyConfiguration.CheckContainsDayOfWeek(day);
            result.Should().Be(expected);
        }
        [Theory]
        [ClassData(typeof(LastDayOfWeeekData))]
        public void get_last_day_of_week_test(DayOfWeek expected, WeeklyConfiguration weeklyConfiguration)
        {
            DayOfWeek result = weeklyConfiguration.GetLastDayOfWeek();
            result.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(FirstDayOfWeeekData))]
        public void get_first_day_of_week_test(DayOfWeek expected, WeeklyConfiguration weeklyConfiguration)
        {
            DayOfWeek result = weeklyConfiguration.GetFirstDayOfWeek();
            result.Should().Be(expected);
        }

        #region AuxiliarClassData
        public class DayOfWeeekToSearchInDaysOfWeekData : TheoryData<DayOfWeek, WeeklyConfiguration,bool>
        {
            public DayOfWeeekToSearchInDaysOfWeekData()
            {
                Add(DayOfWeek.Monday, GetWeeklyConfigWithDays(new int[] { 0, 1, 2, 3, 4, 5, 6 }), true);
                Add(DayOfWeek.Friday, GetWeeklyConfigWithDays(new int[] { 0, 1, 2, 3, 4, 5, 6 }), true);
                Add(DayOfWeek.Monday, GetWeeklyConfigWithDays(new int[] { 0, 2, 3, 4, 5, 6 }), false);
                Add(DayOfWeek.Saturday, GetWeeklyConfigWithDays(new int[] { 0, 1, 2, 3, 4, 5 }), false);
                Add(DayOfWeek.Sunday, GetWeeklyConfigWithDays(new int[] { 0 }), true);
            }
            
            private WeeklyConfiguration GetWeeklyConfigWithDays(int[] days)
            {
                var daysOfWeek = new DayOfWeek[days.Length];
                int index = 0;
                foreach(DayOfWeek day in days)
                {
                    daysOfWeek[index] = day;
                    index++;
                }
                return new WeeklyConfiguration() { ActiveDays = daysOfWeek };
            }
        }

        public class LastDayOfWeeekData : TheoryData<DayOfWeek, WeeklyConfiguration>
        {
            public LastDayOfWeeekData()
            {
                Add(DayOfWeek.Thursday, GetWeeklyConfigWithDays(new int[] { 1, 2, 4}));
                Add(DayOfWeek.Saturday, GetWeeklyConfigWithDays(new int[] { 0, 6}));
                Add(DayOfWeek.Friday, GetWeeklyConfigWithDays(new int[] { 0, 2,  4, 5 }));
                Add(DayOfWeek.Friday, GetWeeklyConfigWithDays(new int[] { 0, 1, 2, 3, 4, 5 }));
                Add(DayOfWeek.Sunday, GetWeeklyConfigWithDays(new int[] { 0 }));
            }

            private WeeklyConfiguration GetWeeklyConfigWithDays(int[] days)
            {
                var daysOfWeek = new DayOfWeek[days.Length];
                int index = 0;
                foreach (DayOfWeek day in days)
                {
                    daysOfWeek[index] = day;
                    index++;
                }
                return new WeeklyConfiguration() { ActiveDays = daysOfWeek };
            }
        }

        public class FirstDayOfWeeekData : TheoryData<DayOfWeek, WeeklyConfiguration>
        {
            public FirstDayOfWeeekData()
            {
                Add(DayOfWeek.Monday, GetWeeklyConfigWithDays(new int[] { 1, 2, 4 }));
                Add(DayOfWeek.Sunday, GetWeeklyConfigWithDays(new int[] { 0, 6 }));
                Add(DayOfWeek.Tuesday, GetWeeklyConfigWithDays(new int[] {  2, 4, 5 }));
                Add(DayOfWeek.Thursday, GetWeeklyConfigWithDays(new int[] { 4, 5 }));
                Add(DayOfWeek.Saturday, GetWeeklyConfigWithDays(new int[] { 6 }));
            }

            private WeeklyConfiguration GetWeeklyConfigWithDays(int[] days)
            {
                var daysOfWeek = new DayOfWeek[days.Length];
                int index = 0;
                foreach (DayOfWeek day in days)
                {
                    daysOfWeek[index] = day;
                    index++;
                }
                return new WeeklyConfiguration() { ActiveDays = daysOfWeek };
            }
        }


        #endregion
    }
}
