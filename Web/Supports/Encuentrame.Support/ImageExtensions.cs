using System;
using System.Drawing;
using System.IO;

namespace Encuentrame.Support
{
    public static class ImageExtensions
    {
        public static Image Base64ToImage(this string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static MemoryStream Base64ToImageMemoryStreamMemory(this string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            return new MemoryStream(imageBytes, 0, imageBytes.Length);

        }

        public static MemoryStream ImageMemoryStreamFromContentFolder(string pathImage)
        {
            var root = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = Path.Combine(root, "images/" + pathImage);

            var imageBytes = File.ReadAllBytes(path);
            return new MemoryStream(imageBytes, 0, imageBytes.Length);

        }

        public static string RemoveBase64Prefix(this string image)
        {
            return image.Remove("data:image/png;base64,").Remove("data:image/jpeg;base64,");
        }

        public static string ImageToBase64(this Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to base 64 string
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
    }
}