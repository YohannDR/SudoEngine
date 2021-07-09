using System;
using OpenTK.Graphics.OpenGL;

namespace SudoEngine.Core
{
    public static class Log
    {
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
        /// <summary> <param>Écrit l'erreur OpenGL La plus récente dans la console (police rouge)</param> </summary>
        public static void GLError() => Error(GL.GetError());
    }
}
