using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace ConsoleGraphics
{
    public class ConsoleImageAnimated
    {
        public readonly int xSiz;
        public readonly int ySiz;
        public readonly int frameCount;
        public readonly ConsoleImage[]? frames;
        public readonly string? fileSrc;

        /// <summary>
        /// Creates a ConsoleImageAnimated instance from a specified file directory
        /// </summary>
        /// <param name="sourceIn">File name, full directory location, or URL of the source image</param>
        /// <param name="xSizIn">Width of the ConsoleImageAnimated in pixel tiles</param>
        /// <param name="ySizIn">Height of the ConsoleImageAnimated in pixel tiles</param>
        /// <param name="fullDir">Set to true if the source image isn't in the "images" folder of the working directory</param>
        /// <param name="url">Set to true to indicate that sourceIn is a URL</param>
        public ConsoleImageAnimated(String sourceIn, int xSizIn, int ySizIn, bool fullDir = false, bool url = false)
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

            xSiz = xSizIn;
            ySiz = ySizIn;
            frames = GetFrames(image);
            frameCount = frames.Length;
        }

        /// <summary>
        /// Creates a ConsoleImageAnimated instance from a specified Image object
        /// </summary>
        /// <param name="imgIn">Image object of source image</param>
        /// <param name="xSizIn">Width of the ConsoleImageAnimated in pixel tiles</param>
        /// <param name="ySizIn">Height of the ConsoleImageAnimated in pixel tiles</param>
        public ConsoleImageAnimated(Image imgIn, int xSizIn, int ySizIn)
        {
            xSiz = xSizIn;
            ySiz = ySizIn;
            frames = GetFrames(imgIn);
            frameCount = frames.Length;
        }

        /// <summary>
        /// Creates instances of ConsoleImage for each frame of imgIn
        /// </summary>
        /// <param name="imgIn">Instance of Image to parse into frames</param>
        /// <returns>Array of ConsoleImage instances to be displayed for the animation</returns>
        private ConsoleImage[] GetFrames(Image imgIn)
        {
            FrameDimension dimension = new(imgIn.FrameDimensionsList[0]);
            int frameCount = imgIn.GetFrameCount(dimension);

            ConsoleImage[] frames = new ConsoleImage[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                imgIn.SelectActiveFrame(dimension, i);
                frames[i] = new ConsoleImage(imgIn, xSiz, ySiz);
            }
            return frames;
        }

    }
}
