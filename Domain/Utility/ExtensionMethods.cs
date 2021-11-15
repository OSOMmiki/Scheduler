
namespace Domain
{
    public static class ExtensionMethods
    {

        public static int TotalSeconds(this TimeSpan time)
        {
            int seconds = time.Hours * 3600;
            seconds += time.Minutes * 60;
            seconds += time.Seconds;
            return seconds;
        }
    }
}
