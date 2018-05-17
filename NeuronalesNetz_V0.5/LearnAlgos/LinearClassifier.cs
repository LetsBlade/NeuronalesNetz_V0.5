using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuronalesNetz_V0._5.KI_Sachen;

namespace NeuronalesNetz_V0._5.LearnAlgos
{
    public class LinearClassifier
    {
        public List<double[]> points;
        public double[] function;
        public double[][] schwerpunkte;

        public LinearClassifier(double[][] p, bool[] label)   //jedes Array der zweiten dimension muss gleich lang sein
        {
            function = GetLC(p, label);
            schwerpunkte = new double[0][];

            while(!TestLC(function,p,label))
            {
                schwerpunkte = schwerpunkte.Concat(new double[][] { GetVectorSchwerpunkt(p) }).ToArray();
                p = AddDimension(p);
                function = GetLC(p, label);
            }

            points = p.ToList();
        }

        public bool? Classify(double[] p)
        {
            AddDimension(p);

            double result = p.First() + function.Last();

            for(int i = 1; i < p.Length; ++i)
            {
                result += p[i] * function[i - 1];
            }

            if(result > 0)
            {
                return true;
            }
            if(result == 0)
            {
                return null;
            }
            return false;        
        }

        public void AddDimension(double[] p)
        {
            
            double[] a = new double[1];

            for (int i = 0; i < schwerpunkte.Length; ++i)
            {
                a[0] = Math.Pow(MatrixMafs.ArrMinusArr(p, schwerpunkte[i]).Average(), 2);
                p = p.Concat(a).ToArray();
            }
        }

        public static bool TestLC(double[] func, double[][] p,bool[] label)
        {
            bool? lbl;

            for(int i = 0; i < label.Length; ++i)
            {
                lbl = GetLabel(func, p[i]);
                if (lbl == null)
                {
                    return false;
                }
                if (lbl != label[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool? GetLabel(double[] func, double[] p)
        {
            double result = p.First() + func.Last();

            for(int i = 1; i < p.Length; ++i)
            {
                result += func[i - 1] * p[i];
            }

            if(result > 0)
            {
                return true;
            }
            else if(result < 0)
            {
                return false;
            }
            else
            {
                return null;
            }
        }

        private static double[] GetLC(double[][] p, bool[] label)   //jedes Array der zweiten dimension muss gleich lang sein
        {
            double[][] pDis = new double[p.Length][];

            for (int i0 = 0; i0 < p.Length; ++i0)
            {
                pDis[i0] = new double[p.Length];

                for (int i1 = 0; i1 < p.Length; ++i1)
                {
                    if (i0 != i1)
                    {
                        pDis[i0][i1] = GetVectorDis(p[i0], p[i1]);
                    }
                    else
                    {
                        pDis[i0][i1] = double.MaxValue;
                    }
                }
            }

            double[][] pMinDis = new double[p[0].Length][];

            for (int i0 = 0; i0 < pMinDis.Length; ++i0)
            {
                pMinDis[i0] = new double[3];
                pMinDis[i0][0] = double.MaxValue;

                for (int i1 = 0; i1 < p.Length; ++i1)
                {
                    for (int i2 = 0; i2 < p.Length; ++i2)
                    {
                        if (pDis[i1][i2] < pMinDis[i0][0] && !label[i1] && label[i2])
                        {
                            pMinDis[i0][0] = pDis[i1][i2];
                            pMinDis[i0][1] = i1;
                            pMinDis[i0][2] = i2;
                        }
                    }
                }

                pDis[Convert.ToInt32(pMinDis[i0][1])][Convert.ToInt32(pMinDis[i0][2])] = double.MaxValue;
            }

            double[][] pMid = new double[p[0].Length][];

            Parallel.For(0, pMid.Length, i =>
            {
                pMid[i] = GetVectorMid(p[Convert.ToInt32(pMinDis[i][1])], p[Convert.ToInt32(pMinDis[i][2])]);
            });

            return GaussisscherAlgorithmus(pMid);
        }

        public static double[][] AddDimension(double[][] p)
        {
            double[] schwerpunkt = GetVectorSchwerpunkt(p);
            double[] a = new double[1];

            for(int i = 0; i < p.Length; ++i)
            {
                a[0] = Math.Pow(MatrixMafs.ArrMinusArr(p[i], schwerpunkt).Average(), 2);
                p[i] = p[i].Concat(a).ToArray();
            }

            return p;
        }

        public void AddDimension()
        {
            double[] schwerpunkt = GetVectorSchwerpunkt(points.ToArray());
            double[] a = new double[1];

            for (int i = 0; i < points.Count; ++i)
            {
                a[0] = Math.Pow(MatrixMafs.ArrMinusArr(points[i], schwerpunkt).Average(), 2);
                points[i] = points[i].Concat(a).ToArray();
            }
        }

        public static double[] GaussisscherAlgorithmus(double[][] p)
        {
            double[][] x = p.Select(a => a.ToArray()).ToArray();
            double[] y = new double[x.Length];

            Parallel.For(0, y.Length, i =>
            {
                y[i] = -x[i][0];
            });

            Parallel.For(0, y.Length, i0 =>
            {
                for (int i1 = 0; i1 < x[0].Length - 1; ++i1)
                {
                    x[i0][i1] = x[i0][i1 + 1];
                }
                x[i0][x[0].Length - 1] = 1;
            });

            for(int i = 0; i < x.Length; ++i)
            {
                if (x[i][i] == 0)
                {
                    bool weiter = true;
                    for(int j = 0; j < x.Length && weiter; ++j)
                    {
                        if (x[j][i] != 0)
                        {
                            SwitchPosInArr(x, j, i);
                            
                        }
                    }
                }
            }

            for (int i0 = 0; i0 < x.Length; ++i0)
            {
                y[i0] /= x[i0][i0];
                x[i0] = MatrixMafs.ArrDivNum(x[i0], x[i0][i0]);

                Parallel.For(0, x.Length, i1 =>
                {
                    if (i1 != i0)
                    {
                        y[i1] /= -x[i1][i0];
                        x[i1] = MatrixMafs.ArrDivNum(x[i1], -x[i1][i0]);

                        x[i1] = MatrixMafs.ArrAddArr(x[i1], x[i0]);
                        y[i1] += y[i0];
                    }
                });
            }


            Parallel.For(0, x.Length, i =>
            {
                y[i] /= x[i][i];
                x[i][i] = 1;
            });

            return y;
        }

        public static double GetVectorDis(double[] p0, double[] p1)
        {
            double dis = 0;

            for (int i = 0; i < p0.Length; ++i)
            {
                dis += Math.Pow(p0[i] - p1[i], 2);
            }

            return Math.Pow(dis, 0.5);
        }

        public static double[] GetVectorMid(double[] p0, double[] p1)
        {
            double[] mid = new double[p0.Length];

            for (int i = 0; i < p0.Length; ++i)
            {
                mid[i] = (p0[i] + p1[i]) / 2;
            }

            return mid;
        }

        public static double[] GetVectorSchwerpunkt(double[][] p)
        {
            double[] x = new double[p[0].Length];

            for (int i = 0; i < p.Length; ++i)
            {
                x = MatrixMafs.ArrAddArr(x, p[i]);
            }

            return MatrixMafs.ArrDivNum(x, p.Length);
        }

        public static void SwitchPosInArr(object[] arr, int p0, int p1)
        {
            object o = arr[p0];
            arr[p0] = arr[p1];
            arr[p1] = o;
        }
        public static string SwitchPosInArr(char[] arr, int p0, int p1)
        {
            char o = arr[p0];
            arr[p0] = arr[p1];
            arr[p1] = o;

            return new string(arr);
        }
        public static void SwitchPosInArr(double[] arr, int p0, int p1)
        {
            double o = arr[p0];
            arr[p0] = arr[p1];
            arr[p1] = o;
        }
        public static void SwitchPosInArr(double[][] arr, int p0, int p1)
        {
            double[] o = arr[p0];
            arr[p0] = arr[p1];
            arr[p1] = o;
        }
    }
}
