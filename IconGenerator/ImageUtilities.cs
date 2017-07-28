using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace IconGenerator
{
    /// <summary>
    /// Provides various image untilities, such as high quality resizing and the ability to save a JPEG.
    /// </summary>
    /// <remarks>
    /// From https://stackoverflow.com/a/353222
    /// </remarks>
    public static class ImageUtilities
    {
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="file">The image to resize.</param>
        /// <param name="target">The output path.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <param name="margin">The margin to apply from each side in percent 0-49.</param>
        public static void ResizeImage(string file, string target, int width, int height, int margin)
        {
            var image = Image.FromFile(file);
            Bitmap result = new Bitmap(width, height);
            try
            {
                //set the resolutions the same to avoid cropping due to resolution differences
                result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                //use a graphics object to draw the resized image into the bitmap
                using (Graphics graphics = Graphics.FromImage(result))
                {
                    //set the resize quality modes to high quality
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //draw the image into the target bitmap
                    var targetWidth = width - (2 * (margin / 100f) * width);
                    var targetHeight = height - (2 * (margin / 100f) * height);

                    var minSize = Math.Min(targetWidth, targetHeight);
                    var sourceAspect = (float)Math.Max(image.Width, image.Height) / Math.Min(image.Width, image.Height);
                    var maxSize = minSize * sourceAspect;
                    var xSize = sourceAspect >= 1 ? maxSize : minSize;
                    var ySize = sourceAspect >= 1 ? minSize : maxSize;
                    var offsetX = (result.Width - xSize) / 2;
                    var offsetY = (result.Height - ySize) / 2;
                    graphics.DrawImage(image, offsetX, offsetY, xSize, ySize);
                }
                var dir = new FileInfo(target).DirectoryName;
                Directory.CreateDirectory(dir);
                result.Save(target, ImageFormat.Png);
            }
            finally
            {
                image.Dispose();
                result.Dispose();
            }
        }
    }
}