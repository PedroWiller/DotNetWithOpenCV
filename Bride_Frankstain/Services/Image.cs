using OpenCvSharp;
using System.Collections.Generic;
using System.Drawing;

namespace Bride_Frankenstein.Services
{
    public class Image
    {
        public byte[] ConverToGrayscale(byte[] file)
        {
            var grayscaleMat = Mat.FromImageData(file, ImreadModes.Grayscale);
            return ConvertToByte(grayscaleMat);
        }

        public byte[] ConverToNegative(byte[] file)
        {
            var grayscaleMat = Mat.FromImageData(file, ImreadModes.ReducedGrayscale2);
            return ConvertToByte(grayscaleMat);
        }

        public Mat ConvertToMat(byte[] file)
        {
            return Cv2.ImDecode(file, ImreadModes.Unchanged);
        }

        public byte[] ConvertToByte(Mat file)
        {
            Cv2.ImEncode(".jpg", file, out var result);
            return result;
        }

        private Bitmap ConvertToBitmap(byte[] file)
        {
            var mat = Cv2.ImDecode(file, ImreadModes.Color);
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);
        }

        public byte[] Test(byte[] file)
        {
            var bmp = ConvertToBitmap(file);
            int width = bmp.Width;
            int height = bmp.Height;
            //negative
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //get pixel value
                    var p = bmp.GetPixel(x, y);

                    //extract ARGB value from p
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    //find negative value
                    r = 255 - r;
                    g = 255 - g;
                    b = 255 - b;

                    //set new ARGB value in pixel
                    bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }

            var mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmp);
            return ConvertToByte(mat);
        }


        public byte[] Predador(byte[] file)
        {
            Mat input = ConvertToMat(file);
            Mat grayInput;
            if (input.Channels() == 1)
                grayInput = input;
            else
            {
                grayInput = new Mat(input.Rows, input.Cols, MatType.CV_8UC3);
                Cv2.CvtColor(input, grayInput, ColorConversionCodes.BGR2GRAY);
            }

            var coloredImage = new Mat();
            Cv2.ApplyColorMap(grayInput, coloredImage, ColormapTypes.Hsv);
            return ConvertToByte(coloredImage);
        }

        public List<ImageFile> CreateYCbGrLayers(byte[] file)
        {
            Mat input = ConvertToMat(file);
            var grayInput = new Mat();
            if (input.Channels() == 1)
                grayInput = input;
            else
                Cv2.CvtColor(input, grayInput, ColorConversionCodes.BGR2YCrCb);


            //Cv2.ApplyColorMap(grayInput, coloredImage, ColormapTypes.Winter);

            var files = new List<ImageFile>();

            foreach (var item in Cv2.Split(grayInput))
                files.Add(new ImageFile { File = ConvertToByte(item) });

            return files;
        }
    }
}
