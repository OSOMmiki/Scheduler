
namespace Domain
{
    public class Configuration
    {
        public bool IsEnabled { get; set; }
        public DateTime? Date { get; set; }
        public int? Periodicity { get; set; }
        public WeeklyConfiguration? WeeklyConfiguration { get; set; }    
        public DailyConfiguration? DailyConfiguration { get; set; }
    }
}