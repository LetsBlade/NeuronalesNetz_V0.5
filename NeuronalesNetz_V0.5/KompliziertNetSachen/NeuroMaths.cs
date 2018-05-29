using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz_V0._5.KompliziertNetSachen
{
    public class NeuroMaths
    {
        #region quick Mafs
        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));
        }

        public static double SigmoidAbleitung(double x)
        {
            double a = 1 / (1 + Math.Pow(Math.E, -x));
            return a * (1 - a);
        }

        public static double Sigmoid(double x, double t)
        {
            return 1 / (1 + Math.Pow(Math.E, -x / t));
        }
        public static double SigmoidAbleitung(double x, double t)
        {
            double a = Math.Pow(Math.E, x / t);
            return a / (t * (a + 1) * (a + 1));
        }

        public static double Tanh(double x)
        {
            return 2 / (1 + Math.Pow(Math.E, -x)) - 1;
        }

        public static double TanhAbleitung(double x)
        {
            double a = 1 / (1 + Math.Pow(Math.E, -x));
            return a * (1 - a);
        }

        public static double Tanh(double x, double t)
        {
            return 2/(1 + Math.Pow(Math.E, -x / t))-1;
        }
        public static double TanhAbleitung(double x, double t)
        {
            double a = Math.Pow(Math.E, x / t);
            return 2*(a / (t * (a + 1) * (a + 1)));
        }

        public static double Linear(double x, double t)
        {
            return x * t;
        }


        public static double Schwellwert(double x)
        {
            if (x > 0)
                return x;
            else
                return 0;
        }

        public static double SchwellwertAbleitung(double x)
        {
            if (x > 0)
                return 1;
            else
                return 0;
        }

        public static double Schwellwert(double x,double t)
        {
            if (x > t)
                return x;
            else
                return 0;
        }

        public static double SchwellwertAbleitung(double x, double t)
        {
            if (x > t)
                return 1;
            else
                return 0;
        }

        public static double RelU(double x)
        {
            if (x >= 0)
                return x;
            else
                return 0.1*x;
        }

        public static double RelUAbleitung(double x)
        {
            if (x >= 0)
                return 1;
            else
                return 0.1;
        }

        public static double RelU(double x, double t)
        {
            if (x >= t)
                return x;
            else
                return 0.1*x;
        }

        public static double RelUAbleitung(double x, double t)
        {
            if (x >= t)
                return 1;
            else
                return 0.1;
        }
        //public static double Tanh(double x)
        //{
        //    return (Math.Pow(Math.E, x) - Math.Pow(Math.E, -x)) / (Math.Pow(Math.E, x) + Math.Pow(Math.E, -x));
        //}
        #endregion

        public static double Formel(char Func,double x,double t)
        {
            if(Func == 's')
            {
                return Sigmoid(x);
            }
            else if (Func == 'S')
            {
                return Sigmoid(x,t);
            }
            else if (Func == 't')
            {
                return Tanh(x);
            }
            else if (Func == 'T')
            {
                return Tanh(x,t);
            }
            else if (Func == 'l')
            {
                return x;
            }
            else if (Func == 'L')
            {
                return Linear(x, t);
            }
            else if (Func == 'r')
            {
                return RelU(x);
            }
            else if (Func == 'R')
            {
                return RelU(x, t);
            }
            else if (Func == 'c')
            {
                return Schwellwert(x);
            }
            else if (Func == 'C')
            {
                return Schwellwert(x, t);
            }
            else
            {
                throw new Exception("Funktion " + Func + " ist nicht definiert!");
            }
        }

        public static double Formel(string Func,double x,double[] t)
        {

            for(int i = 0; i < Func.Length; ++i)
            {
                x = Formel(Func[i], x, t[i]);
            }

            return x;
        }

        public static double Ableitung(char Func,double x,double t)
        {
            if (Func == 's')
            {
                return SigmoidAbleitung(x);
            }
            else if (Func == 'S')
            {
                return SigmoidAbleitung(x, t);
            }
            else if (Func == 't')
            {
                return TanhAbleitung(x);
            }
            else if (Func == 'T')
            {
                return TanhAbleitung(x, t);
            }
            else if (Func == 'l')
            {
                return 1;
            }
            else if (Func == 'L')
            {
                return t;
            }
            else if (Func == 'r')
            {
                return RelUAbleitung(x);
            }
            else if (Func == 'R')
            {
                return RelUAbleitung(x, t);
            }
            else if (Func == 'c')
            {
                return SchwellwertAbleitung(x);
            }
            else if (Func == 'C')
            {
                return SchwellwertAbleitung(x, t);
            }
            else
            {
                throw new Exception("Funktion " + Func + " ist nicht definiert!");
            }
        }

        public static double Ableitung(string Func, double x, double[] t)
        {
            double y = 1;

            for(int i0 = 0; i0 < Func.Length; ++i0)
            {
                double j = x;

                for (int i1 = 0; i1 < i0; ++i1)
                {
                    j = Formel(Func[i1], j, t[i1]);
                }

                y *= Ableitung(Func[i0], j, t[i0]);
            }

            return y;
        }


    }

    public class MatrixMafs
    {

        public static double[] ArrDivNum(double[] x0, double x1)
        {
            double[] y = new double[x0.Length];

            for (int i = 0; i < x0.Length; ++i)
            {
                y[i] = x0[i] / x1;
            }

            return y;
        }

        public static double[] ArrDivNum(double[] x0, double x1,int startIndex)
        {
            double[] y = new double[x0.Length];
            for (int i = startIndex; i < x0.Length; ++i)
            {
                y[i] = x0[i] / x1;
            }

            return y;
        }

        public static double[] ArrMultNum(double[] x0, double x1)
        {
            double[] y = new double[x0.Length];

            for (int i = 0; i < x0.Length; ++i)
            {
                y[i] = x0[i] * x1;
            }

            return y;
        }

        public static double[] ArrMultNum(double[] x0, double x1,int startIndex)
        {
            double[] y = new double[x0.Length];
            for (int i = startIndex; i < x0.Length; ++i)
            {
                y[i] = x0[i] * x1;
            }

            return y;
        }

        public static double[] ArrAddArr(double[] x0, double[] x1)
        {
            double[] y = new double[x0.Length];
            for (int i = 0; i < x0.Length; ++i)
            {
                y[i] = x0[i] + x1[i];
            }

            return y;
        }

        public static double[] ArrAddArr(double[] x0, double[] x1, int startIndex)
        {
            double[] y = new double[x0.Length];
            for (int i = startIndex; i < x0.Length; ++i)
            {
                y[i] = x0[i] + x1[i];
            }

            return y;
        }

        public static double[] ArrMinusArr(double[] x0, double[] x1)
        {
            double[] y = new double[x0.Length];
            for (int i = 0; i < x0.Length; ++i)
            {
                y[i] = x0[i] - x1[i];
            }

            return y;
        }
    }
}
