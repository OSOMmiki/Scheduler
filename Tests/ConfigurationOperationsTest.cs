using Xunit;
using FluentAssertions;
using Domain;

namespace Tests
{
    public class ConfigurationOperationsTest
    {
        [Theory]
        [InlineData(1,DailyFrecuency.Seconds,1)]
        [InlineData(1, DailyFrecuency.Minutes, 60)]
        [InlineData(1, DailyFrecuency.Hours, 3600)]
        [InlineData(60, DailyFrecuency.Seconds, 60)]
        [InlineData(60, DailyFrecuency.Minutes, 3600)]
        [InlineData(60, DailyFrecuency.Hours, 216000)]
        public void get_total_seconds_periodicity_test(int periodicity, DailyFrecuency dailyFrecuency, int expected)
        {
            int seconds = ConfigurationOperations.GetTotalSecondsPeriocity(periodicity, dailyFrecuency);
            seconds.Should().Be(expected);  
        }



        [Theory]
        [InlineData(DayWeek.Monday,true)]
        [InlineData(DayWeek.Tuesday, true)]
        [InlineData(DayWeek.Wednesday, true)]
        [InlineData(DayWeek.Thursday, true)]
        [InlineData(DayWeek.Friday, false)]
        [InlineData(DayWeek.Saturday, false)]
        [InlineData(DayWeek.Sunday, true)]
        
        public void check_contains_day_of_week__test(DayWeek day, bool expected)
        {
            var listOfDays = new DayWeek[5]
            {
                DayWeek.Monday,
                DayWeek.Tuesday,
                DayWeek.Wednesday,
                DayWeek.Thursday,
                DayWeek.Sunday
            };
            bool result = ConfigurationOperations.CheckContainsDayOfWeek(day,listOfDays);
            result.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(LastDayOfWeeekData))]
        public void get_last_day_of_week_test(DayWeek expected, DayWeek[] listOfDays)
        {
            DayWeek result = ConfigurationOperations.GetLastDayOfWeek(listOfDays);
            result.Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(FirstDayOfWeeekData))]
        public void get_first_day_of_week_test(DayWeek expected, DayWeek[] listOfDays)
        {
            DayWeek result = ConfigurationOperations.GetFirstDayOfWeek(listOfDays);
            result.Should().Be(expected);
        }

        #region AuxiliarClassData
        public class LastDayOfWeeekData : TheoryData<DayWeek, DayWeek[]>
        {
            public LastDayOfWeeekData()
            {
                Add(DayWeek.Thursday, new DayWeek[3]{ (DayWeek)1, (DayWeek)2, (DayWeek)4 });
                Add(DayWeek.Saturday, new DayWeek[2] {(DayWeek)1, (DayWeek)6 });
                Add(DayWeek.Friday, new DayWeek[4] { (DayWeek)1, (DayWeek)2, (DayWeek)4, (DayWeek)5 });
                Add(DayWeek.Saturday, new DayWeek[6] { (DayWeek)1, (DayWeek)2, (DayWeek)3, (DayWeek)4, (DayWeek)5, (DayWeek)6 });
                Add(DayWeek.Monday, new DayWeek[1]  { (DayWeek)1 });
            }
        }
        public class FirstDayOfWeeekData : TheoryData<DayWeek, DayWeek[]>
        {
            public FirstDayOfWeeekData()
            {
                Add(DayWeek.Monday, new DayWeek[3] { (DayWeek)1, (DayWeek)2, (DayWeek)4 });
                Add(DayWeek.Wednesday, new DayWeek[2] { (DayWeek)3, (DayWeek)6 });
                Add(DayWeek.Tuesday, new DayWeek[3] { (DayWeek)2, (DayWeek)4, (DayWeek)5 });
                Add(DayWeek.Monday, new DayWeek[6] { (DayWeek)1, (DayWeek)2, (DayWeek)3, (DayWeek)4, (DayWeek)5, (DayWeek)6 });
                Add(DayWeek.Sunday, new DayWeek[1] { (DayWeek)7 });
            }
        }
        #endregion
    }
}
