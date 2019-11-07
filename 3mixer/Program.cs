using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace _3mixer
{
    class Program
    {
        static float opacity = 1;
        static string imagePathFormat = @"{0}\{1}.{2}";
        static string extension = "png";
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    opacity = float.Parse(args[0].Replace("-", string.Empty), CultureInfo.InvariantCulture);
                }
                catch
                {
                    opacity = 1;
                }
            }
            if (CheckImages(extension))
                StackImages(extension);
            else
            {
                System.Console.WriteLine("--------------");
                System.Console.WriteLine(string.Empty);
                System.Console.WriteLine("Requirements: To convert images into a 3 mixed image, ");
                System.Console.WriteLine("add a file named left.png, a file named right.png, ");
                System.Console.WriteLine("and a file named back.png to the current directory. ");
                System.Console.WriteLine("If the background is not a png, just rename the file ");
                System.Console.WriteLine("extension to a png. The left and right files must be ");
                System.Console.WriteLine("true png files to make the image transparency display properly.");
                System.Console.WriteLine(string.Empty);
                System.Console.WriteLine("You can add opacity to the background image by providing a ");
                System.Console.WriteLine("decimal argument. The values should range from 0 to 1.");
                System.Console.WriteLine("Example: 3mixer -0.9");
            }
        }

        static void StackImages(string arg)
        {
            int outputImageWidth = 800;
            int outputImageHeight = 600;
            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var firstImage = Image.FromFile(string.Format(imagePathFormat, Directory.GetCurrentDirectory(), "back", arg));
            var secondImage = Image.FromFile(string.Format(imagePathFormat, Directory.GetCurrentDirectory(), "left", arg));
            var thirdImage = Image.FromFile(string.Format(imagePathFormat, Directory.GetCurrentDirectory(), "right", arg));
            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                var backResized = ScaleImageMaxWidth(firstImage, 720);
                var leftResized = ScaleImageMaxHeight(secondImage, 352);
                var rightSized = ScaleImageMaxWidth(thirdImage, 400);

                graphics.DrawImage(SetImageOpacity(backResized, opacity), 40, 0);
                graphics.DrawImage(leftResized, 6, 240);
                graphics.DrawImage(rightSized, 400, 594 - rightSized.Height);
                System.Console.WriteLine("Saving file 'output.png'");
                outputImage.Save("output.png", ImageFormat.Png);
                System.Console.WriteLine(string.Empty);
                System.Console.WriteLine("Complete!");
            }
        }

        static Image SetImageOpacity(Image image, float opacity)
        {
            var bmp = new Bitmap(image.Width, image.Height);
            using (var graphics = Graphics.FromImage(bmp))
            {
                var matrix = new ColorMatrix();
                matrix.Matrix33 = opacity;
                var attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }

        static Image ScaleImageMaxWidth(Image image, int maxWidth)
        {
            var ratio = (double)maxWidth / image.Width;
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        static Image ScaleImageMaxHeight(Image image, int maxHeight)
        {
            var ratio = (double)maxHeight / image.Height;
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        static bool CheckImages(string arg)
        {
            System.Console.WriteLine("Checking images...");
            System.Console.WriteLine(string.Empty);
            var left = string.Format(imagePathFormat, Directory.GetCurrentDirectory(), "left", arg);
            if (!File.Exists(left))
            {
                System.Console.WriteLine(string.Format("Left image missing! Must have a file named left.{0}.", extension));
                System.Console.WriteLine("This will be the system game box front.");
                System.Console.WriteLine(string.Empty);
            }
            var right = string.Format(imagePathFormat, Directory.GetCurrentDirectory(), "right", arg);
            if (!File.Exists(right))
            {
                System.Console.WriteLine(string.Format("Right image missing! Must have a file named right.{0}.", extension));
                System.Console.WriteLine("This will be the game header.");
                System.Console.WriteLine(string.Empty);
            }

            var background = string.Format(imagePathFormat, Directory.GetCurrentDirectory(), "back", arg);
            if (!File.Exists(background))
            {
                System.Console.WriteLine(string.Format("Back image missing! Must have a file named back.{0}.", extension));
                System.Console.WriteLine("This will be your screenshot background image.");
                System.Console.WriteLine(string.Empty);
            }
            return File.Exists(left) && File.Exists(right) && File.Exists(background);
        }

        static float Scale(int height, int width)
        {
            var scaleHeight = (float)600 / (float)height;
            var scaleWidth = (float)800 / (float)width;
            return Math.Min(scaleHeight, scaleWidth);
        }
    }
}
