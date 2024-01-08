using Max = ConsoleGraphics.Maximizer;
using CG = ConsoleGraphics.ConsoleGraphics;
using CS = ConsoleSound.ConsoleSound;
using System.Media;

namespace ConsoleGraphics
{
    public static class ConsoleGraphicsDemo
    {
        static void Main()
        {
            Max.Maximize();
            Console.CursorVisible = false;
            Console.Title = "Console Graphics Demo";
            Console.WriteLine("Loading Data!");

            ConsoleImageAnimated animTest = new(@"nums.gif", 15, 15);
            ConsoleImageAnimated webAnim = new(@"https://media4.giphy.com/media/v1.Y2lkPTc5MGI3NjExbGlvcnVxcW16c3FmMmIxemVseW11YTN3MHltOXk1ZjhhY2JnenV5MiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/Awuqdc4Rj6MbS/giphy.gif", 60, 40, url: true);
            ConsoleImage bkg = new("room.png", 60, 40);
            ConsoleImage dClosed = new("dur-closed.png", 15, 15);
            ConsoleImage dOpen = new("dur-open.png", 15, 15);
            SoundPlayer click = CS.SoundMaker("click.wav");


            Console.Clear();

            Console.SetCursorPosition(121, 15);
            Console.WriteLine("These images are frames of a gif.");
            Console.SetCursorPosition(121, 16);
            Console.WriteLine("The bottom is the animated version!");
            for (int i = 0; i < 5; i++)
            {
                CG.DisplayFrame(animTest, i, i * 10); 
            }
            
            CG.DisplayAnimatedImage(animTest, 0, 10, 30);

            Console.SetCursorPosition(0, 25);
            Console.WriteLine("Press any key to continue.");

            Console.ReadKey(true);
            Console.Clear();

            Console.SetCursorPosition(121, 15);
            Console.WriteLine("This gif was loaded from the internet!");
            CG.DisplayAnimatedImage(webAnim, loopCount: 3);
            Console.SetCursorPosition(0, 41);
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
            Console.Clear();

            CG.DisplayImage(bkg);
            CG.DisplayImage(dClosed, 23, 8);
            Console.SetCursorPosition(121, 15);
            Console.WriteLine("Press Any key to open the door.");
            Console.ReadKey(true);
            click.Play();
            CG.DisplayImage(dOpen, 23, 8);
            Console.SetCursorPosition(121, 15);
            Console.WriteLine("Great job opening that door!!! "); 
            Console.SetCursorPosition(121, 16);
            Console.WriteLine("It even made a noise! :O");


            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
