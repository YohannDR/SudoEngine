using OpenTK;

namespace SudoEngine
{
    class Program
    {
        static void Main()
        {
            using (Window window = new Window(900, 600, "SudoEngine"))
            {
                window.Run(10, 60);
                window.VSync = VSyncMode.On;
            }
        }
    }
}
