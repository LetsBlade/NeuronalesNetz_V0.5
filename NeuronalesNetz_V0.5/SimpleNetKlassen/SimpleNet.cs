using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz_V0._5.SimpleNetKlassen
{
    public partial class SimpleNet
    {
        private static Random rnd = new Random();
        public double[][] NeuronValue;
        public double[][] NeuronDeltaValue;
        public double[][][] Weights;

        public double Etha;

        public string[] NeuronArt;
        public double[][] ArtT;

        public SimpleNet(double Etha, params int[] NeuronAnz)
        {
            this.Etha = Etha;

            NeuronValue = new double[NeuronAnz.Length][];
            NeuronDeltaValue = new double[NeuronAnz.Length][];
            ArtT = new double[NeuronAnz.Length][];
            NeuronArt = new string[NeuronAnz.Length];

            Weights = new double[NeuronAnz.Length - 1][][];

            for(int i = 0; i < NeuronAnz.Length; ++i)
            {
                NeuronValue[i] = new double[NeuronAnz[i]];
                NeuronDeltaValue[i] = new double[NeuronAnz[i]];
            }

            for (int i0 = 0; i0 < NeuronAnz.Length - 1; ++i0)
            {
                Weights[i0] = new double[NeuronAnz[i0] + 1][];
                for (int i1 = 0; i1 < NeuronAnz[i0]+1; ++i1)
                {
                    Weights[i0][i1] = GetRandArr(NeuronAnz[i0 + 1], -2, 4);
                }
            }
        }

        public SimpleNet(double Etha,double rndStart,double rndLenght, params int[] NeuronAnz)
        {
            this.Etha = Etha;

            NeuronValue = new double[NeuronAnz.Length][];
            NeuronDeltaValue = new double[NeuronAnz.Length][];
            ArtT = new double[NeuronAnz.Length][];
            NeuronArt = new string[NeuronAnz.Length];

            Weights = new double[NeuronAnz.Length - 1][][];

            for (int i = 0; i < NeuronAnz.Length; ++i)
            {
                NeuronValue[i] = new double[NeuronAnz[i]];
                NeuronDeltaValue[i] = new double[NeuronAnz[i]];
            }

            for (int i0 = 0; i0 < NeuronAnz.Length - 1; ++i0)
            {
                Weights[i0] = new double[NeuronAnz[i0] + 1][];
                for (int i1 = 0; i1 < NeuronAnz[i0] + 1; ++i1)
                {
                    Weights[i0][i1] = GetRandArr(NeuronAnz[i0 + 1], rndStart, rndLenght);
                }
            }
        }

        public void SetAfNames(params string[] name)
        {
            string[] a = new string[1];
            
            NeuronArt = a.Concat(name).ToArray();
        }

        public void SetAfT(int index,params double[] T)
        {
            ArtT[index] = T;
        }

        public virtual void SetInput(double[] input)
        {
            if (input.Length > NeuronValue[0].Length)
            {
                throw new Exception("Der Input hat die Falsche Länge");
            }

            int addInput = 0;

            if(NeuronValue[0].Length > input.Length)
            {
                addInput = NeuronValue[0].Length - input.Length;
            }

            NeuronValue[0] = input;
            NeuronValue[0] = NeuronValue[0].Concat(new double[addInput]).ToArray();
        }

        public virtual void OutputBerechnen()
        {
            for(int i0 = 1; i0 < NeuronValue.Length; ++i0)
            {
                Parallel.For(0, NeuronValue[i0].Length, i1 =>
                {
                    NeuronValue[i0][i1] = 0;

                    for (int i2 = 0; i2 < NeuronValue[i0 - 1].Length; ++i2)
                    {
                        NeuronValue[i0][i1] += NeuronValue[i0 - 1][i2] * Weights[i0 - 1][i2][i1];
                    }
                    NeuronValue[i0][i1] += Weights[i0 - 1][NeuronValue[i0 - 1].Length][i1];
                    NeuronValue[i0][i1] = NeuroMaths.Formel(NeuronArt[i0], NeuronValue[i0][i1], ArtT[i0]);
                });
            }
        }

        public double[] GetOutput()
        {
            return NeuronValue.Last();
        }

        public void DeltawertBerechen(double[] outputSoll)
        {   
            if (outputSoll.Length != NeuronDeltaValue.Last().Length)
            {
                throw new Exception("Der gewollte Output hat die Falsche länge");
            }

            for(int i = 0; i < NeuronValue.Last().Length; ++i)
            {
                NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = 0;
                NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = NeuroMaths.Ableitung(NeuronArt.Last(), NeuronValue.Last()[i], ArtT.Last()) * (outputSoll[i] - NeuronValue.Last()[i]);
            }

            for(int i0 = NeuronValue.Length - 2; i0 > 0; --i0)
            {
                Parallel.For(0, NeuronValue[i0].Length, i1 =>
                {
                    NeuronDeltaValue[i0][i1] = 0;
                    for (int i2 = 0; i2 < NeuronValue[i0 + 1].Length; ++i2)
                    {
                        NeuronDeltaValue[i0][i1] += Weights[i0][i1][i2] * NeuronDeltaValue[i0 + 1][i2];
                    }
                    NeuronDeltaValue[i0][i1] *= NeuroMaths.Ableitung(NeuronArt[i0], NeuronValue[i0][i1], ArtT[i0]);
                });
            }
        }

        public double[] GetFirstLayerDelta()
        {
            double[] arr = new double[NeuronValue.First().Length];

            Parallel.For(0, arr.Length, i0 => 
            {
                for(int i1 = 0; i1 < NeuronValue[1].Length; ++i1)
                {
                    arr[i0] += NeuronDeltaValue[1][i1] * Weights[0][i0][i1];
                }
            });

            return arr;
        }

        public void ReinforcementLearning(double reward,bool useAbleitung)
        {
            if (useAbleitung)
            {
                for (int i = 0; i < NeuronValue.Last().Length; ++i)
                {
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = 0;
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = reward * NeuroMaths.Ableitung(NeuronArt.Last(), NeuronValue.Last()[i], ArtT.Last());
                }
            }
            else
            {
                for (int i = 0; i < NeuronValue.Last().Length; ++i)
                {
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = 0;
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = reward;
                }
            }


            for (int i0 = NeuronValue.Length - 2; i0 > 0; --i0)
            {
                for (int i1 = 0; i1 < NeuronValue[i0].Length; ++i1)
                {
                    NeuronDeltaValue[i0][i1] = 0;
                    for (int i2 = 0; i2 < NeuronValue[i0 + 1].Length; ++i2)
                    {
                        NeuronDeltaValue[i0][i1] += Weights[i0][i1][i2] * NeuronDeltaValue[i0 + 1][i2];
                    }
                    NeuronDeltaValue[i0][i1] *= NeuroMaths.Ableitung(NeuronArt[i0],NeuronValue[i0][i1],ArtT[i0]);
                }
            }
        }

        public virtual void ReinforcementLearning2(double reward ,bool useAbleitung)
        {
            if (useAbleitung)
            {
                for (int i = 0; i < NeuronValue.Last().Length; ++i)
                {
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = 0;
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = ((NeuronValue.Last()[i] - 0.5) * reward * 2) * NeuroMaths.Ableitung(NeuronArt.Last(), NeuronValue.Last()[i], ArtT.Last());
                }
            }
            else
            {
                for (int i = 0; i < NeuronValue.Last().Length; ++i)
                {
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = 0;
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = ((NeuronValue.Last()[i] - 0.5) * reward * 2);
                }
            }


            for (int i0 = NeuronValue.Length - 2; i0 > 0; --i0)
            {
                for (int i1 = 0; i1 < NeuronValue[i0].Length; ++i1)
                {
                    NeuronDeltaValue[i0][i1] = 0;
                    for (int i2 = 0; i2 < NeuronValue[i0 + 1].Length; ++i2)
                    {
                        NeuronDeltaValue[i0][i1] += Weights[i0][i1][i2] * NeuronDeltaValue[i0 + 1][i2];
                    }
                    NeuronDeltaValue[i0][i1] *= NeuroMaths.Ableitung(NeuronArt[i0],NeuronValue[i0][i1],ArtT[i0]);
                }
            }
        }

        public virtual void ReinforcementLearning3(double reward, bool useAbleitung)
        {
            if (useAbleitung)
            {
                for (int i = 0; i < NeuronValue.Last().Length; ++i)
                {
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = 0;
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = (NeuronValue.Last()[i] * reward) * NeuroMaths.Ableitung(NeuronArt.Last(), NeuronValue.Last()[i], ArtT.Last());
                }
            }
            else
            {
                for (int i = 0; i < NeuronValue.Last().Length; ++i)
                {
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = 0;
                    NeuronDeltaValue[NeuronDeltaValue.Length - 1][i] = (NeuronValue.Last()[i] * reward);
                }
            }


            for (int i0 = NeuronValue.Length - 2; i0 > 0; --i0)
            {
                for (int i1 = 0; i1 < NeuronValue[i0].Length; ++i1)
                {
                    NeuronDeltaValue[i0][i1] = 0;
                    for (int i2 = 0; i2 < NeuronValue[i0 + 1].Length; ++i2)
                    {
                        NeuronDeltaValue[i0][i1] += Weights[i0][i1][i2] * NeuronDeltaValue[i0 + 1][i2];
                    }
                    NeuronDeltaValue[i0][i1] *= NeuroMaths.Ableitung(NeuronArt[i0], NeuronValue[i0][i1], ArtT[i0]);
                }
            }
        }

        public void IsertDelta(double[] deltaArr)
        {
            NeuronDeltaValue[NeuronDeltaValue.Length - 1] = deltaArr;
        }

        public void Backpropagation()
        {
            for(int i0 = 0; i0 < NeuronValue.Length - 1; ++i0)
            {
                Parallel.For(0, NeuronValue[i0].Length, i1 =>
                {
                    if (NeuronValue[i0][i1] != 0)
                    {
                        for (int i2 = 0; i2 < NeuronValue[i0 + 1].Length; ++i2)
                        {
                            Weights[i0][i1][i2] += Etha * NeuronValue[i0][i1] * NeuronDeltaValue[i0 + 1][i2];
                        }
                    }
                });
                Parallel.For(0, NeuronValue[i0 + 1].Length, i2 =>
                {
                    Weights[i0][NeuronValue[i0].Length][i2] += Etha * NeuronDeltaValue[i0 + 1][i2];
                });
            }
        }

        public double GetFehlerDif(double[] erwarteterOutput)
        {
            double output = 0;

            for(int i = 0; i < erwarteterOutput.Length; ++i)
            {
                output += Math.Abs(erwarteterOutput[i] - NeuronValue.Last()[i]);
            }

            return output;
        }
        
        public int GetMaxOutputIndex()
        {
            int max = 0;

            for(int i = 0; i < NeuronValue.Last().Length; ++i)
            {
                if (NeuronValue.Last()[i] > NeuronValue.Last()[max])
                    max = i;
            }

            return max;
        }

        public int? GetMaxOutputIndex(double min)
        {
            int max = 0;

            for (int i = 0; i < NeuronValue.Last().Length; ++i)
            {
                if (NeuronValue.Last()[i] > NeuronValue.Last()[max])
                    max = i;
            }

            if (NeuronValue.Last()[max] > min)
                return max;
            else
                return null;
        }


        public static double[] GetRandArr(int length, double start, double lenghtRand)
        {
            double[] output = new double[length];

            for(int i = 0; i < length; ++i)
            {
                output[i] = start + rnd.NextDouble() * lenghtRand;
            }

            return output;
        }

        public override string ToString()
        {

            return "Etha: "+Etha;
        }
    }
}
