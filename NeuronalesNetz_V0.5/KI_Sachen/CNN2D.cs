using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz_V0._5.KI_Sachen
{
    public class CNN2D
    {
        private object[] layer;


        public class PoolingLayer
        {
            public int xSize;
            public int ySize;
            public double[,] value;
            public double[,] deltaValue;

            public PoolingLayer(int xSize, int ySize)
            {
                this.xSize = xSize;
                this.ySize = ySize;
            }

            public void OutputBerechnen(double[,] input)
            {
                value = new double[Convert.ToInt32(Math.Round(Convert.ToDouble(input.GetLength(0)) / Convert.ToDouble(xSize), MidpointRounding.AwayFromZero)), Convert.ToInt32(Math.Round(Convert.ToDouble(input.GetLength(1)) / Convert.ToDouble(ySize), MidpointRounding.AwayFromZero))];
                deltaValue = new double[input.GetLength(0), input.GetLength(1)];

                for (int i0 = 0; i0 < input.GetLength(0); i0 += xSize)
                {
                    for (int i1 = 0; i1 < input.GetLength(1); i1 += ySize)
                    {
                        value[i0/xSize, i1/ySize] = GetMaxValue(GetPice(input, i0, i1, xSize, ySize));
                    }
                }
            }

            public void DeltaValueBerechnen(double[,] input)
            {
                for(int x = 0; x < input.GetLength(0); x+=xSize)
                {
                    for (int y = 0; y < input.GetLength(1); y+=ySize)
                    {
                        InsertValue(deltaValue, x, y, xSize, ySize, input[x / xSize, y / ySize]);
                    }
                }
            }
        }

        public class ConvolutionalLayer
        {
            private double[,] filter;
            public double[,] value;
            public double[,] deltaValue;

            public ConvolutionalLayer(int xSize,int ySize, double rndStart, double rndLenght)
            {
                Random rnd = new Random();
                filter = new double[xSize, ySize];
                for(int i = 0; i < xSize * ySize; ++i)
                {
                    filter.SetValue(rndStart + rnd.NextDouble() * rndLenght, i);
                }

            }

            public void OutputBerechnen(double[,] input)
            {

            }

            public void DeltaValueBerechnen(double[,] input)
            {
               
            }
        }

        public static double[] GetMatrixSum(double[,] arr)
        {
            double sum = 0;

            foreach8
        }

        public static double[,] MultMartix(double[,] arr0, double[,] arr1)
        {
            double[,] result = new double[arr0.GetLength(0), arr0.GetLength(1)];
            

            Parallel.For(0, arr0.GetLength(0), i0 => 
            {
                for(int i1 = 0; i1 < arr0.GetLength(1); ++i1)
                {
                    result[i0, i1] = arr0[i0, i1] * arr1[i0, i1];
                }
            });

            return result;
        }

        public static double[,] GetPice(double[,] input, int xOffset, int yOffset, int xSize, int ySize)
        {
            int xLength = xSize - Convert.ToInt32(NeuroMaths.RelU(xOffset + xSize - input.GetLength(0)));
            int yLength = ySize - Convert.ToInt32(NeuroMaths.RelU(yOffset + ySize - input.GetLength(1)));
            double[,] output = new double[xLength, yLength];

            for (int i0 = 0; i0 < xLength; ++i0)
            {
                for (int i1 = 0; i1 < yLength; ++i1)
                {
                    output[i0, i1] = input[xOffset + i0, yOffset + i1];
                }
            }

            return output;
        }

        public static double GetMaxValue(double[,] arr)
        {
            double max = 0;

            foreach(double item in arr)
            {
                if (item > max)
                {
                    max = item;
                }
            }

            return max;
        }

        public static void InsertValue(double[,] arr, int xOffset, int yOffset, int xSize, int ySize, double value)
        {
            xSize = xSize - Convert.ToInt32(NeuroMaths.RelU(xOffset + xSize - arr.GetLength(0)));
            ySize = ySize - Convert.ToInt32(NeuroMaths.RelU(yOffset + ySize - arr.GetLength(1)));

            for (int x = 0; x < xSize; ++x)
            {
                for (int y = 0; y < ySize; ++y)
                {
                    arr[xOffset + x, yOffset + y] = value;
                }
            }
        }
    }


}
