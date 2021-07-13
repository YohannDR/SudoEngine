#if DEBUG
using OpenTK;

namespace SudoEngine
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (Window window = new Window(900, 600, "SudoEngine"))
            {
                window.Run(10, 60);
                window.VSync = VSyncMode.On;
            }
        }
    }
}
#endif