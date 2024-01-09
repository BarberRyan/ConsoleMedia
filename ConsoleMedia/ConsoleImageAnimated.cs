using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace ConsoleGraphics
{
    public class ConsoleImageAnimated
    {
        public readonly int XSiz;
        public readonly int YSiz;
        public readonly int FrameCount;
        public int FrameDelay;
        public readonly ConsoleImage[]? Frames;
        public readonly string? FileSrc;

        /// <summary>
        /// Creates a ConsoleImageAnimated instance from a specified file directory
        /// </summary>
        /// <param name="sourceIn">File name, full directory location, or URL of the source image</param>
        /// <param name="xSizIn">Width of the ConsoleImageAnimated in pixel tiles</param>
        /// <param name="ySizIn">Height of the ConsoleImageAnimated in pixel tiles</param>
        /// <param name="fullDir">Set to true if the source image isn't in the "images" folder of the working directory</param>
        /// <param name="url">Set to true to indicate that sourceIn is a URL</param>
        /// <param name="frameDelay">Delay before displaying the next frame in milliseconds</param>
        /// <param name="crop">Crops the source frames based on (Starting X, Starting Y, Width, Height) in pixels</param>
        public ConsoleImageAnimated(String sourceIn, int xSizIn, int ySizIn, bool fullDir = false, bool url = false, int frameDelay = 0, (int, int, int, int)? crop = null)
        {
            String fileSrc = "";
            Image image = new Bitmap(21, 21, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            if (fullDir)
            {
                fileSrc = sourceIn;
            }
            else if (url)
            {
                WebClient w = new();
                try
                {
                    byte[] imgBtye = w.DownloadData(sourceIn);
                    MemoryStream stream = new (imgBtye);
                    image = Image.FromStream(stream);
                }
                catch (Exception e)
                {
                    Console.WriteLine("NOPE - " + e.Message);
                    return;
                }
            }
            else
            {
                fileSrc = System.IO.Directory.GetCurrentDirectory() + @"\images\" + sourceIn;
            }
            try
            {
                if (!url)
                {
                    image = (Bitmap)Bitmap.FromFile(fileSrc);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Image at {fileSrc} gave error of {e.Message}");
            }

            XSiz = xSizIn;
            YSiz = ySizIn;
            FrameDelay = frameDelay;
            Frames = GetFrames(image, crop);
            FrameCount = Frames.Length;
        }

        /// <summary>
        /// Creates a ConsoleImageAnimated instance from a specified Image object
        /// </summary>
        /// <param name="imgIn">Image object of source image</param>
        /// <param name="xSizIn">Width of the ConsoleImageAnimated in pixel tiles</param>
        /// <param name="ySizIn">Height of the ConsoleImageAnimated in pixel tiles</param>
        /// <param name="frameDelay">Delay before displaying the next frame in milliseconds</param>
        /// <param name="crop">Crops the source frames based on (Starting X, Starting Y, Width, Height) in pixels</param>
        public ConsoleImageAnimated(Image imgIn, int xSizIn, int ySizIn, int frameDelay  = 0, (int, int, int, int)? crop = null)
        {
            XSiz = xSizIn;
            YSiz = ySizIn;
            FrameDelay = frameDelay;
            Frames = GetFrames(imgIn, crop);
            FrameCount = Frames.Length;
        }

        /// <summary>
        /// Creates instances of ConsoleImage for each frame of imgIn
        /// </summary>
        /// <param name="imgIn">Instance of Image to parse into frames</param>
        /// <param name="crop">Crops the source frames based on (Starting X, Starting Y, Width, Height) in pixels</param>
        /// <returns>Array of ConsoleImage instances to be displayed for the animation</returns>
        private ConsoleImage[] GetFrames(Image imgIn, (int, int, int, int)? crop = null)
        {
            FrameDimension dimension = new(imgIn.FrameDimensionsList[0]);
            int frameCount = imgIn.GetFrameCount(dimension);

            ConsoleImage[] frames = new ConsoleImage[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                imgIn.SelectActiveFrame(dimension, i);
                frames[i] = new ConsoleImage(imgIn, XSiz, YSiz, crop);
            }
            return frames;
        }

        /// <summary>
        /// Displays the animated image
        /// </summary>
        /// <param name="xPos">X coordinate of the top left pixel</param>
        /// <param name="yPos">Y coordinate of the top left pixel</param>
        /// <param name="loopCount">The number of times the animation should play</param>
        /// <param name="startFrame">Frame of the animation to start on</param>
        /// <param name="endFrame">Frame of the animation to end on</param>
        /// <param name="transparent">Set to true to set transparent pixel tiles to the background color (to avoid trails in transparent animations)</param>
        public void Display(int xPos = 0, int yPos = 0, int loopCount = 1, int startFrame = 0, int endFrame = 999, bool transparent = false)
        {
            ConsoleGraphics.DisplayAnimatedImage(this, xPos, yPos, loopCount, startFrame, endFrame, FrameDelay, transparent);
        }

        /// <summary>
        /// Displays the ConsoleImage of a specified frame of a ConsoleImageAnimated
        /// </summary>
        /// <param name="frame">Frame of the animation to display</param>
        /// <param name="xPos">X coordinate of the top left pixel</param>
        /// <param name="yPos">Y coordinate of the top left pixel</param>
        /// <param name="opaque">Set true to make transparent pixels match the background color.</param>
        public void DisplayFrame(int frame, int xPos = 0, int yPos = 0, bool opaque = false)
        {
            ConsoleGraphics.DisplayFrame(this, frame, xPos, yPos, opaque);
        }

    }
}
