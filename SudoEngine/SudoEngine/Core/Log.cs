using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace SudoEngine.Core
{
    public static class Log
    {
        private static Stopwatch SW = new Stopwatch();
        /// <summary> <param>Écrit un message dans la console (police bleue)</param> </summary>
        public static void Info(object message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"[LOG] - {message}");
        }

        /// <summary> <param>Écrit un message dans la console (police jaune)</param> </summary>
        public static void Warning(object message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[LOG] - {message}");
        }

        /// <summary> <param>Écrit un message dans la console (police rouge)</param> </summary>
        public static void Error(object message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[LOG] - {message}");
        }
        /// <summary> <param>Écrit l'erreur OpenGL la plus récente dans la console (police rouge)</param> </summary>
        public static void GLError() => Error(GL.GetError());
        /// <summary> <param>Lance une StopWatch</param> </summary>
        public static void StartTimer() => SW.Start();
        /// <summary> <param>Stoppe la Stopwatch et écrit le temps écoulé avec un log info</param> </summary>
        public static void StopTimer()
        {
            SW.Stop();
            Info($"Ticks : {SW.ElapsedTicks} ; MS : {SW.ElapsedMilliseconds}");
            SW.Reset();
        }
    }
}
