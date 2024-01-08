using Pastel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGraphics
{
    public class ConsoleImage
    {
        public int XSiz;
        public int YSiz;
        public string FileSrc = "";
        public  Bitmap SrcImage = new(21, 21, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        public Color[][] Pixels = new Color[1][];

        /// <summary>
        /// Creates a ConsoleImage instance from a specified file directory
        /// </summary>
        /// <param name="sourceIn">File name, full directory location, or URL of the source image</param>
        /// <param name="xSizIn">Width of the ConsoleImage in pixel tiles</param>
        /// <param name="ySizIn">Height of the ConsoleImage in pixel tiles</param>
        /// <param name="fullDir">Set to true if the source image isn't in the "images" folder of the working directory and is not a URL</param>
        /// <param name="url">Set to true if sourceIn is a URL</param>
        public ConsoleImage(string sourceIn, int xSizIn, int ySizIn, bool fullDir = false, bool url = false)
        {
            if (fullDir)
            {
                FileSrc = sourceIn;
            }
            else if (url)
            {
                WebClient w = new();
                try
                {
                    Console.WriteLine("PING!");
                    byte[] imgBtye = w.DownloadData(sourceIn);
                    MemoryStream stream = new (imgBtye);
                    SrcImage = (Bitmap)Bitmap.FromStream(stream);
                }
                catch (Exception e)
                {
                    Console.WriteLine("NOPE - " + e.Message);
                    FileSrc = "";
                    return;
                }
            }
            else
            {
                FileSrc = System.IO.Directory.GetCurrentDirectory() + @"\images\" + sourceIn;
            }
            try
            {
                if (!url)
                {
                    SrcImage = (Bitmap)Bitmap.FromFile(FileSrc);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Image at {FileSrc} gave error of {e.Message}");
                return;
            }
            XSiz = xSizIn;
            YSiz = ySizIn;
            Pixels = GeneratePixels();
        }

        /// <summary>
        /// Creates a ConsoleImage instance from a specified Image object
        /// </summary>
        /// <param name="imgIn">Image object of source image</param>
        /// <param name="xSizIn">Width of the ConsoleImage in pixel tiles</param>
        /// <param name="ySizIn">Height of the ConsoleImage in pixel tiles</param>
        public ConsoleImage(Image imgIn, int xSizIn, int ySizIn)
        {
            XSiz = xSizIn;
            YSiz = ySizIn;
            FileSrc = "TEMP";
            SrcImage = new Bitmap(imgIn);
            Pixels = GeneratePixels();
        }

        /// <summary>
        /// Creates pixel tile data for displaying the ConsoleImage
        /// </summary>
        /// <param name="xSizIn">Width of the parent ConsoleImage in pixel tiles</param>
        /// <param name="ySizIn">Height of the parent ConsoleImage in pixel tiles</param>
        /// <returns>2 Dimensional array of Color objects representing the color of each pixel tile</returns>
        private Color[][] GeneratePixels()
        {
            int xSam = SrcImage.Width / XSiz;
            int ySam = SrcImage.Height / YSiz;

            Color[] row = new Color[XSiz];
            Color[][] output = new Color[YSiz][];

            for (int y = 0; y < YSiz; y++)
            {
                for (int x = 0; x < XSiz; x++)
                {
                    row[x] = GetTileColor((x * xSam, y * ySam), xSam, ySam);
                }
                output[y] = row;
                row = new Color[XSiz];
            }
            return output;
        }

        /// <summary>
        /// Gets the average color of a sample of pixels in SrcImage to determie the color of a pixel tile
        /// </summary>
        /// <param name="startPos">Tuple of coordinates of the top left pixel of the sample</param>
        /// <param name="samX">Width of the sample in pixels</param>
        /// <param name="samY">Height of the sample in pixels</param>
        /// <returns>Instance of Color representing the average ARGB value of the specified sample</returns>
        private Color GetTileColor((int, int) startPos, int samX, int samY)
        {
            int ATotal = 0;
            int RTotal = 0;
            int GTotal = 0;
            int BTotal = 0;
            int res = samX * samY;
            int imgW = SrcImage.Width;
            int imgH = SrcImage.Height;

            Color pixelColor;

            for (int x = startPos.Item1; x < startPos.Item1 + samX; x++)
                for (int y = startPos.Item2; y < startPos.Item2 + samY; y++)
                {
                    if (x > imgW - 1) x = imgW - 1;

                    if (y > imgH - 1) y = imgH - 1;

                    pixelColor = SrcImage.GetPixel(x, y);
                    ATotal += pixelColor.A;
                    RTotal += pixelColor.R;
                    GTotal += pixelColor.G;
                    BTotal += pixelColor.B;
                }

            return Color.FromArgb(ATotal / res, RTotal / res, GTotal / res, BTotal / res);
        }

    }
}
