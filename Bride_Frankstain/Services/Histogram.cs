using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using ImreadModes = OpenCvSharp.ImreadModes;

namespace Bride_Frankenstein.Services
{
    public class Histogram
    {
        public byte[] CastImage(byte[] file)
        {
            string filePath = @"C:\Images";
            var src = Cv2.ImDecode(file, ImreadModes.Color);
            using (var gray = src.CvtColor(ColorConversionCodes.RGB2YCrCb))
            {
                gray.SaveImage($@"{filePath}\5.jpg");
                return gray.ToBytes();
            }
        }

        public Mat Test(byte[] file, string tipo)
        {
            var src = Mat.FromImageData(file, ImreadModes.Color);
            // Histogram view
            const int Width = 260, Height = 200;
            var render = new Mat(new Size(Width, Height), MatType.CV_8UC3, Scalar.All(255));

            // Calculate histogram
            var hist = new Mat();

            int[] hdims = { 256 }; // Histogram size for each dimension
            Rangef[] ranges = { new Rangef(0, 256), }; // min/max 
            Cv2.CalcHist(
                new Mat[] { src },
                new int[] { 0 },
                null,
                hist,
                1,
                hdims,
                ranges);

            // Get the max value of histogram
            double minVal, maxVal;
            Cv2.MinMaxLoc(hist, out minVal, out maxVal);

            var color = Scalar.All(100);
            // Scales and draws histogram
            hist = hist * (maxVal != 0 ? Height / maxVal : 0.0);

            var data = new List<Graph>();
            for (int j = 0; j < hdims[0]; ++j)
            {
                int binW = (int)((double)Width / hdims[0]);

                var x = j * binW;
                var y = render.Rows - (int)(hist.Get<float>(j));
                //using (var con = new BaseRepository())
                //    con.Execute($"INSERT INTO Valores  VALUEs ({x}, {y}, '{tipo}')");

                //    CreateTextFile($"{x} - {y}");

                render.Rectangle(
                    new Point(j * binW, render.Rows - (int)(hist.Get<float>(j))),
                    new Point((j + 1) * binW, render.Rows),
                    color, -1);
            }

            //using (new Window("Image", WindowMode.AutoSize | WindowMode.FreeRatio, src))
            //using (new Window("Histogram", WindowMode.AutoSize | WindowMode.FreeRatio, render))
            //{
            //    Cv2.WaitKey();
            //}

            return hist;
        }

        public Mat CreateGraph(Mat hist)
        {
            // Histogram view
            const int Width = 260, Height = 200;
            var render = new Mat(new Size(Width, Height), MatType.CV_8UC3, Scalar.All(255));

            // Calculate histogram
            int[] hdims = { 256 }; // Histogram size for each dimension
            Rangef[] ranges = { new Rangef(0, 256), }; // min/max 
           

            // Get the max value of histogram
            double minVal, maxVal;
            Cv2.MinMaxLoc(hist, out minVal, out maxVal);

            var color = Scalar.All(100);
            // Scales and draws histogram
            hist = hist * (maxVal != 0 ? Height / maxVal : 0.0);

            var data = new List<Graph>();
            for (int j = 0; j < hdims[0]; ++j)
            {
                int binW = (int)((double)Width / hdims[0]);

                render.Rectangle(
                    new Point(j * binW, render.Rows - (int)(hist.Get<float>(j))),
                    new Point((j + 1) * binW, render.Rows),
                    color, -1);
            }

            using (new Window("Histogram", WindowMode.AutoSize | WindowMode.FreeRatio, render))
            {
                Cv2.WaitKey();
            }

            return hist;
        }

        public decimal Compare(Mat first, Mat second)
        {
            var result = Cv2.CompareHist(first, second, HistCompMethods.Chisqr);
            return (decimal)Math.Round(result, 2);
        }

        public void CreateTextFile(string message, string name = "MyTest")
        {
         

            string path = $@"C:\Images\{name}.txt";
            if (!File.Exists(path))
                using (var sw = File.CreateText(path))
                    sw.WriteLine(message);
            else
                File.AppendAllText(path, message + Environment.NewLine);
        }
    }

    public class Graph
    {
        public int x { get; set; }
        public int y { get; set; }
    }

}

