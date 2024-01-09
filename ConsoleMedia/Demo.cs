using Max = ConsoleGraphics.Maximizer;
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

            ConsoleImageAnimated animTest = new(@"nums.gif", 15, 15, crop: (150, 150, 200, 200), frameDelay: 1000);
            ConsoleImageAnimated webAnim = new(@"https://media4.giphy.com/media/v1.Y2lkPTc5MGI3NjExbGlvcnVxcW16c3FmMmIxemVseW11YTN3MHltOXk1ZjhhY2JnenV5MiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/Awuqdc4Rj6MbS/giphy.gif", 60, 40, url: true);
            ConsoleImage bkg = new("room.png", 60, 40);
            ConsoleImage dClosed = new("dur-closed.png", 15, 15);
            ConsoleImageAnimated dOpen = new("dur-anim.gif", 15, 15);
            SoundPlayer click = CS.SoundMaker("click.wav");


            Console.Clear();

            Console.SetCursorPosition(121, 20);
            Console.WriteLine("These images are frames of a gif.");
            Console.SetCursorPosition(121, 21);
            Console.WriteLine("The bottom is the animated version!");
            for (int i = 0; i < 5; i++)
            {
                animTest.DisplayFrame(i, i * animTest.XSiz);
            }

            for(int i = 0;i < 10; i++)
            {
                animTest.FrameDelay -= 100;
                animTest.Display(0, animTest.YSiz + 1, i);  
            }



            Console.SetCursorPosition(121, 25);
            Console.WriteLine("Press any key to continue.");

            Console.ReadKey(true);
            Console.Clear();

            Console.SetCursorPosition(121, 15);
            Console.WriteLine("This gif was loaded from the internet!");
            webAnim.Display();
            Console.SetCursorPosition(121, 17);
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
            Console.Clear();

            bkg.Display();
            dClosed.Display(23, 8);
            Console.SetCursorPosition(121, 15);
            Console.WriteLine("Press Any key to open the door.");
            Console.ReadKey(true);
            click.Play();
            dOpen.Display(23, 8);
            Console.SetCursorPosition(121, 15);
            Console.WriteLine("Great job opening that door!!! ");
            Console.SetCursorPosition(121, 16);
            Console.WriteLine("It even made a noise! :O");


            Console.ReadKey(true);
            Console.Clear();

            

        }
    }
}
