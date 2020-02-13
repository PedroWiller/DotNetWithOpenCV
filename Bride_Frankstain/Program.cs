using Bride_Frankenstein.Services;
using OpenCvSharp;
using System.Collections.Generic;
using System.IO;

namespace Bride_Frankenstein
{
    public class Program
    {
        static string filePath = @"C:\Images";

        public static void Main(string[] args)
        {
            var dir = new DirectoryInfo(filePath);
            foreach (var item in dir.GetFiles())
                Execute(item.Name);
        }

        public void Main2()
        {
            var dir = new DirectoryInfo(filePath);
            var repo =  new BaseRepository();
            repo.Execute("DELETE FROM NovoTest");
            foreach (var item in dir.GetFiles())
                Execute(item.Name);
        }

        public static decimal Calc(decimal value)
        {
            const decimal baseCalc = (decimal)18806.993913;
            value = baseCalc - value;
            if (value < 0)
                value = value * -1;

            return value / baseCalc;
        }

        private static void Execute(string fileName)
        {
            var hist = new Histogram();
            var draw = new DrawCascade();
            var image = new Image();

            var file = ReadFile(fileName);
            var matFile = image.ConvertToMat(file);

            var ret = draw.Mark(matFile);
            if (ret == null)
            {
                using (var con = new BaseRepository())
                    con.Execute($"INSERT INTO NovoTest VALUEs ('{fileName}', 'FALSE', '{0}')");

                return;
            }
            Save(ret, $@"New\{fileName}");

            var compare = ShinraTenseiCompare(ret, "POSITIVO");
            compare = Calc(compare);
            var teste = "POSITIVO";
            if (compare > (decimal).535 && compare < (decimal).60)
                teste = "FALSO";

            if (compare > (decimal).35 && compare < (decimal).39)
                teste = "FALSO";

            if (compare > (decimal).65 && compare < (decimal).78)
                teste = "FALSO";

            if (compare > (decimal).25 && compare < (decimal).29)
                teste = "FALSO";

            if (compare > (decimal).10 && compare < (decimal).20)
                teste = "FALSO";

            using (var con = new BaseRepository())
                con.Execute($"INSERT INTO NovoTest VALUEs ('{fileName}', '{teste}', '{compare}')");
            hist.CreateTextFile($"POSITIVO : {compare}", "TESTE");

        }

        private static Mat ShinraTensei(string fileName, string tipo)
        {
            var image = new Image();
            var file = ReadFile(fileName);
            var layers = image.CreateYCbGrLayers(file);
            var histogram = CreateHistograns(layers, tipo);

            return ConcatHistogram(histogram[0], histogram[1], histogram[2]);
        }

        private static decimal ShinraTenseiCompare(byte[] file, string tipo)
        {
            var image = new Image();
            var layers = image.CreateYCbGrLayers(file);
            var histogram = CreateHistograns(layers, tipo);

            var hist = new Histogram();

            return hist.Compare(histogram[0], histogram[1]);
        }

        public static List<Mat> CreateHistograns(List<ImageFile> file, string tipo)
        {
            var hist = new Histogram();
            var mats = new List<Mat>();
            for (var i = 0; i < 3; i++)
                mats.Add(hist.Test(file[i].File, tipo));

            return mats;
        }

        public static Mat ConcatHistogram(Mat h1, Mat h2, Mat h3)
        {

            var concatResult = new Mat();
            Cv2.HConcat(h1, h2, concatResult);

            var concatResult2 = new Mat();
            Cv2.HConcat(concatResult, h3, concatResult2);

            return concatResult2;
        }

        public static byte[] ReadFile(string name)
        {
            return File.ReadAllBytes($"{filePath}\\{name}");
        }

        public static void Save(byte[] file, string name)
        {
            using (FileStream fsNew = new FileStream($"{filePath}\\{name}", FileMode.Create, FileAccess.Write))
                fsNew.Write(file, 0, file.Length);
        }
    }
}

