using System;

namespace SudoEngine.Core
{
    public static class Log
    {
        /// <summary> <param>Écrit un message dans la console (police bleue)</param> </summary>
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"[LOG] - {message}");
        }
        /// <summary> <param>Écrit un message dans la console (police bleue)</param> </summary>
        public static void Info(int value)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"[LOG] - {value}");
        }

        /// <summary> <param>Écrit un message dans la console (police jaune)</param> </summary>
        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[LOG] - {message}");
        }       
        /// <summary> <param>Écrit un message dans la console (police jaune)</param> </summary>
        public static void Warning(int value)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[LOG] - {value}");
        }

        /// <summary> <param>Écrit un message dans la console (police rouge)</param> </summary>
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[LOG] - {message}");
        }      
        /// <summary> <param>Écrit un message dans la console (police rouge)</param> </summary>
        public static void Error(int value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[LOG] - {value}");
        }
    }
}
