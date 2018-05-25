using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Encuentrame.Support
{
    public static class FileExtensions
    {
        public static void SaveToFile(this Stream stream, string fileFullPath)
        {
            if (stream.Length == 0) return;

            // Create a FileStream object to write a stream to a file
            using (var fileStream = File.Create(fileFullPath, (int)stream.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }

        public static Image ResizeImage(this Image image, int width, int height,PixelFormat pixelFormat)
        {
            float sourceWidth = image.Width;
            float sourceHeight = image.Height;
            float destHeight = 0;
            float destWidth = 0;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            // force resize, might distort image
            if (width != 0 && height != 0)
            {
                destWidth = width;
                destHeight = height;
            }
            // change size proportially depending on width or height
            else if (height != 0)
            {
                destWidth = (float)(height * sourceWidth) / sourceHeight;
                destHeight = height;
            }
            else
            {
                destWidth = width;
                destHeight = (float)(sourceHeight * width / sourceWidth);
            }

            Bitmap bmPhoto = new Bitmap((int)destWidth, (int)destHeight,
                                        pixelFormat);
            bmPhoto.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.Low;

            grPhoto.DrawImage(image,
                new Rectangle(destX, destY, (int)destWidth, (int)destHeight),
                new Rectangle(sourceX, sourceY, (int)sourceWidth, (int)sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
            
           
        }

        public static string ImageToBase64(this Stream stream, int maxWidth, int maxHeight,PixelFormat pixelFormat)
        {
            if (stream.Length == 0) return "";
            try
            {
                using (Image image = Image.FromStream(stream))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.ResizeImage(maxWidth, maxHeight, pixelFormat).Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }

            }
            catch (Exception ex)
            {
                return "";
            }
         
        }
        public static string ImageToBase64(this Stream stream)
        {
            if (stream.Length == 0) return "";
            try
            {
                using (Image image = Image.FromStream(stream))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }

            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}