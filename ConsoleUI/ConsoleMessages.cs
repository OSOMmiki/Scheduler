using System;

namespace ConsoleUI
{
    internal static class ConsoleMessages
    {
        public static void PlaceWhiteSpace()
        {
            Console.WriteLine();
        }
        public static bool AskNextDate()
        {
            string reply;
            do
            {
                Console.WriteLine("Do you want to scheduler next Date? Press y/n:");
                reply = Console.ReadLine() ?? String.Empty;
                switch (reply)
                {
                    case "y":
                        return true;
                    case "n":
                        return false;
                }
            } while( reply != "y" && reply != "n" );

            return false;
        }

        public static void Wait()
        {
            Console.ReadLine();
        }

        public static void NoticeAndWait()
        {
            Console.WriteLine("Press any key to Continue:");
            Console.ReadLine();
        }
    }
}
