using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuronalesNetz_V0._5.KI_Sachen;
using System.IO;
using System.Drawing;

namespace NeuronalesNetz_V0._5.SimpleNetKlassen
{
    class UCDIG
    {
        public SimpleNet discriminator;
        public SimpleNet encoder;
        public SimpleNet decoder;
        public int savePer;
        public string outputPath;

        public UCDIG(SimpleNet discriminator, SimpleNet encoder, SimpleNet decoder)
        {
            this.discriminator = discriminator;
            this.encoder = encoder;
            this.decoder = decoder;
        }

        public void TrainDisciminator(int cycles, string animieFolder)
        {
            string[] files = Directory.GetFiles(animieFolder);

            for(int i=0;i< cycles; ++i)
            {
                discriminator.SetInput(MatrixMafs.ArrDivNum(GetDoubleArr(File.ReadAllBytes(files[i % files.Length])),255));
                discriminator.OutputBerechnen();
                discriminator.DeltawertBerechen(new double[] { 1 });
                discriminator.Backpropagation();

                discriminator.SetInput(SimpleNet.GetRandArr(750000,0,1));
                discriminator.OutputBerechnen();
                discriminator.DeltawertBerechen(new double[] { 0 });
                discriminator.Backpropagation();

                if (i % 1000 == 0)
                {
                    Console.WriteLine("Trainingscycle nr " + i);
                }
            }
        }

        public static double[] GetDoubleArr(byte[] arrB)
        {
            double[] arrD = new double[arrB.Length];

            Parallel.For(0, arrD.Length, i => 
            {
                arrD[i] = Convert.ToDouble(arrB[i]) / 255;
            });

            return arrD;
        }

        public void TrainingsCycle(double[] echtBild, double[] animie)
        {
            encoder.SetInput(echtBild);
            encoder.OutputBerechnen();
            decoder.SetInput(encoder.GetOutput());
            decoder.OutputBerechnen();

            double[] encoderOutput = encoder.GetOutput();

            encoder.SetInput(decoder.GetOutput());
            encoder.OutputBerechnen();
            encoder.DeltawertBerechen(encoderOutput);

            discriminator.SetInput(decoder.GetOutput());
            discriminator.OutputBerechnen();
            discriminator.DeltawertBerechen(new double[] { 1 });

            decoder.IsertDelta(discriminator.GetFirstLayerDelta());
            decoder.Backpropagation();
            encoder.Backpropagation();

            discriminator.DeltawertBerechen(new double[] { 0 });
            discriminator.Backpropagation();

            discriminator.SetInput(animie);
            discriminator.OutputBerechnen();
            discriminator.DeltawertBerechen(new double[] { 1 });
            discriminator.Backpropagation();

        }

        public void Train(int cycles,string animiePath, string echtBildPath)
        {
            string[] animieFiles = Directory.GetFiles(animiePath);
            string[] echtBildFiles = Directory.GetFiles(echtBildPath);

            for(int i = 0; i < cycles; ++i)
            {
                TrainingsCycle(MatrixMafs.ArrDivNum(GetDoubleArr(File.ReadAllBytes(echtBildFiles[i % echtBildFiles.Length])), 255), MatrixMafs.ArrDivNum(GetDoubleArr(File.ReadAllBytes(animieFiles[i % animieFiles.Length])), 255));

                if ((i + 1) % savePer == 0)
                {
                    File.Create(outputPath + i + ".arr");
                    Convert2Bitmap(decoder.GetOutput()).Save(outputPath + i + ".arr");
                }
            }
        }

        public static Bitmap Convert2Bitmap(double[] arr0)
        {
            double[] arr = MatrixMafs.ArrMultNum(arr0, 255);
            Bitmap bmp = new Bitmap(500, 500);

            for(int i = 0; i < 250000; ++i)
            {
                bmp.SetPixel(i % 500, (i - (i % 500)) / 500, Color.FromArgb(Convert.ToByte(arr[i * 3]),Convert.ToByte(arr[i * 3 + 1]), Convert.ToByte(arr[i * 3 + 2])));
            }

            return bmp;
        } 
    }
}
