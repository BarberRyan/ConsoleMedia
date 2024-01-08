using System.Runtime.InteropServices;

namespace ConsoleGraphics
{
    internal class Maximizer
    {
        /// <summary>
        /// Maximizes the console window
        /// </summary>
        public static void Maximize()
        {
            [DllImport("kernel32.dll", ExactSpelling = true)]
            static extern IntPtr GetConsoleWindow();
            IntPtr ThisConsole = GetConsoleWindow();
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
            const int MAXIMIZE = 3;
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
        }
    }
}
