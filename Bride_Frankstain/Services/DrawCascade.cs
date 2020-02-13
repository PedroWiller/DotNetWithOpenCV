using OpenCvSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using CascadeClassifier = OpenCvSharp.CascadeClassifier;
using CoreRectangle = SixLabors.Primitives.Rectangle;
using ImageSharp = SixLabors.ImageSharp.Image;
using Mat = OpenCvSharp.Mat;


namespace Bride_Frankenstein.Services
{
    public class DrawCascade
    {
        public byte[] Mark(Mat srcImage)
        {
            try
            {
                var grayImage = new Mat();
                Cv2.CvtColor(srcImage, grayImage, ColorConversionCodes.BGRA2GRAY);
                Cv2.EqualizeHist(grayImage, grayImage);

                var cascade = new CascadeClassifier($@"{AppDomain.CurrentDomain.BaseDirectory}/Services/Data/haarcascade_frontalface_alt.xml");

                var faces = cascade.DetectMultiScale(
                    grayImage,
                          1.1,
                    3, HaarDetectionType.DoRoughSearch | HaarDetectionType.ScaleImage
                );


                if (faces.Length < 1)
                    return null;

                var face = faces.FirstOrDefault();
                var image = new Image();

                var file = image.ConvertToByte(srcImage);
                return Crop(file, face.X, face.Y, face.Width, face.Height);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public byte[] Crop(byte[] file, int x, int y, int width, int height)
        {
            try
            {
                if (x == default(int))
                    return file;

                var format = ImageSharp.DetectFormat(file);
                using (var image = ImageSharp.Load(file))
                {
                    using (var ms = new MemoryStream())
                    {
                        image.Mutate(img => img.Crop(new CoreRectangle(x, y, width, height)));
                        image.Save(ms, format);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception)
            {
                return file;
            }
        }
    }
}
