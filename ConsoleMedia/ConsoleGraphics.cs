using Pastel;
using System.Drawing;

namespace ConsoleGraphics
{
    internal class ConsoleGraphics
    {
        /// <summary>
        /// Displays a ConsoleImage
        /// </summary>
        /// <param name="img">Instance of ConsoleImage to display</param>
        /// <param name="xPos">X coordinate of the top left pixel</param>
        /// <param name="yPos">Y coordinate of the top left pixel</param>
        /// <param name="transparentAnim">Set to true to set transparent pixel tiles to the background color (to avoid trails in transparent animations)</param>
        public static void DisplayImage(ConsoleImage img, int xPos = 0, int yPos = 0, bool transparentAnim = false)
        {
            if (img.Pixels != null)
            {
                Console.SetCursorPosition(xPos * 2, yPos);
                for (int y = 0; y < img.Pixels.Length; y++)
                {
                    foreach (Color color in img.Pixels[y])
                    {
                        if (color.A <= 25)
                        {
                            if (transparentAnim)
                            {
                                Console.Write("██".Pastel(Console.BackgroundColor));
                            }
                            else
                            {
                                Console.CursorLeft += 2;
                            }
                        }
                        else
                        {
                            Console.Write("██".Pastel(Color.FromArgb(color.R, color.G, color.B)));
                        }
                    }
                    Console.SetCursorPosition(xPos * 2, yPos + y + 1);
                }
            }
            else
            {
                Console.WriteLine($"Image at {img.FileSrc} is null! Ensure that the directory is correct, then try again!");
            }
        }

        /// <summary>
        /// Displays an animated console image (like a .gif)
        /// </summary>
        /// <param name="imgIn">Instance of ConsoleImageAnimated to display</param>
        /// <param name="xPos">X coordinate of the top left pixel</param>
        /// <param name="yPos">Y coordinate of the top left pixel</param>
        /// <param name="loopCount">The number of times the animation should play</param>
        /// <param name="startFrame">Frame of the animation to start on</param>
        /// <param name="endFrame">Frame of the animation to end on</param>
        /// <param name="transparent">Set to true to set transparent pixel tiles to the background color (to avoid trails in transparent animations)</param>
        public static void DisplayAnimatedImage(ConsoleImageAnimated imgIn, int xPos = 0, int yPos = 0, int loopCount = 1, int startFrame = 0, int endFrame = 999, bool transparent = false)
        {
            if(startFrame < 0) { startFrame = 0; }
            if(startFrame >= imgIn.frameCount) { startFrame = imgIn.frameCount; }
            if(endFrame >= imgIn.frameCount) {  endFrame = imgIn.frameCount; }
            if(endFrame <= startFrame)
            {
                if(startFrame == imgIn.frameCount)
                {
                    startFrame--;
                }
                endFrame = startFrame + 1;
            }

            for (int i = 0; i < loopCount; i++)
            {
                for (int j = startFrame; j < endFrame; j++)
                {
                    DisplayImage(imgIn.frames[j], xPos, yPos, transparent);
                }
            }
        }

        /// <summary>
        /// Display a single frame of a ConsoleImageAnimated
        /// </summary>
        /// <param name="imgIn">Instance of ConsoleImageAnimated to load</param>
        /// <param name="frame">Frame number to display (0 indexed)</param>
        /// <param name="xPos">X coordinate of top left pixel tile in pixel tiles</param>
        /// <param name="yPos">Y coordinate of top left pixel tile in pixel tiles</param>
        /// <param name="opaque">Set to true to set transparent tiles to the background color</param>
        public static void DisplayFrame(ConsoleImageAnimated imgIn, int frame, int xPos = 0, int yPos = 0, bool opaque = false)
        {
            if(frame < 0) { frame = 0; }
            if(frame >= imgIn.frameCount) {  frame = imgIn.frameCount - 1; }

            DisplayImage(imgIn.frames[frame], xPos, yPos, opaque);
        }
    }
}
