using System.Drawing;
using System.Net;

namespace ConsoleGraphics
{
    public class ConsoleImage
    {
        public readonly int XSiz;
        public readonly int YSiz;
        public readonly string FileSrc = "";
        public readonly Bitmap SrcImage = new(21, 21, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        public readonly Color[][] Pixels = new Color[1][];

        /// <summary>
        /// Creates a ConsoleImage instance from a specified file directory
        /// </summary>
        /// <param name="sourceIn">File name, full directory location, or URL of the source image</param>
        /// <param name="xSizIn">Width of the ConsoleImage in pixel tiles</param>
        /// <param name="ySizIn">Height of the ConsoleImage in pixel tiles</param>
        /// <param name="fullDir">Set to true if the source image isn't in the "images" folder of the working directory and is not a URL</param>
        /// <param name="url">Set to true if sourceIn is a URL</param>
        /// <param name="crop">Crops the source image based on (Starting X, Starting Y, Width, Height) in pixels</param>
        public ConsoleImage(string sourceIn, int xSizIn, int ySizIn, bool fullDir = false, bool url = false, (int, int, int, int)? crop = null)
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
                if (!url) { SrcImage = (Bitmap)Bitmap.FromFile(FileSrc); }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Image at {FileSrc} gave error of {e.Message}");
                return;
            }
            XSiz = xSizIn;
            YSiz = ySizIn;

            if(crop != null) { SrcImage = Crop(((int, int, int, int))crop); }


            Pixels = GeneratePixels();
        }

        /// <summary>
        /// Creates a ConsoleImage instance from a specified Image object
        /// </summary>
        /// <param name="imgIn">Image object of source image</param>
        /// <param name="xSizIn">Width of the ConsoleImage in pixel tiles</param>
        /// <param name="ySizIn">Height of the ConsoleImage in pixel tiles</param>
        /// <param name="crop">Crops the source image based on (Starting X, Starting Y, Width, Height) in pixels</param>
        public ConsoleImage(Image imgIn, int xSizIn, int ySizIn, (int, int, int, int)? crop = null)
        {
            XSiz = xSizIn;
            YSiz = ySizIn;
            FileSrc = "TEMP";
            SrcImage = new Bitmap(imgIn);
            if(crop != null) { SrcImage = Crop(((int, int, int, int))crop); }

            Pixels = GeneratePixels();
        }

        /// <summary>
        /// Creates pixel tile data for displaying the ConsoleImage
        /// </summary>
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

        /// <summary>
        /// Displays the image
        /// </summary>
        /// <param name="xPos">X coordinate of top left pixel tile (in pixel tiles)</param>
        /// <param name="yPos">Y coordinate of top left pixel tile (in pixel tiles)</param>
        public void Display(int xPos = 0, int yPos = 0)
        {
            ConsoleGraphics.DisplayImage(this, xPos, yPos);
        }


        /// <summary>
        /// Ensures that crop values are not outside the range of the source image
        /// </summary>
        /// <param name="crop">crop parameters as (Starting X, Starting Y, Width, Height) in pixels</param>
        /// <returns>crop parameters as (Starting X, Starting Y, Width, Height) in pixels</returns>
        private (int, int, int, int) CropCoordHandler((int, int, int, int) crop)
        {
            int xStart = crop.Item1;
            int yStart = crop.Item2;
            int width = crop.Item3;
            int height = crop.Item4;

            //X CROP
            if (xStart < 0)
            {
               xStart = 0;
            }
            if (xStart + width >= SrcImage.Width)
            {
                width = SrcImage.Width - xStart;
            }

            //Y CROP
            if (yStart < 0)
            {
                yStart = 0;
            }
            if (yStart + height >= SrcImage.Height)
            {
                height = SrcImage.Height - yStart;
            }

            return (xStart, yStart, width, height);
        }


        /// <summary>
        /// Returns a copy of SrcImage cropped to the specified parameters
        /// </summary>
        /// <param name="crop">crop parameters as (Starting X, Starting Y, Width, Height) in pixels</param>
        /// <returns>Cropped Bitmap</returns>
        private Bitmap Crop((int, int, int, int) crop)
        {
            crop = CropCoordHandler(crop);
            Rectangle cropRect = new Rectangle(crop.Item1, crop.Item2, crop.Item3, crop.Item4);

            using (Bitmap croppedImage = SrcImage.Clone(cropRect, SrcImage.PixelFormat))
            {
                return new Bitmap(croppedImage);
            }
        }

    }
}
