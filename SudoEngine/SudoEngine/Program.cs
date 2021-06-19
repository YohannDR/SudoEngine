using OpenTK;

namespace SudoEngine
{
    class Program
    {
        static void Main()
        {
            using (Window window = new Window(900, 600, "SudoEngine"))
            {
                window.Run(60.0d, 60.0d);
                window.VSync = VSyncMode.On;
            }
        }
    }
}
