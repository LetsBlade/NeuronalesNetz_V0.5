using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
     
namespace NeuronalesNetz_V0._5
{    
    public class AF
    {
        public static readonly string Sigmoid = "S";
        public static readonly string Tanh = "T";
        public static readonly string Bias = "B";
        public static readonly string Linear = "L";
        public static readonly string Schwellwert = "C";
            
    }

    public class Variablen
    {       
        public static void Setup()
        {

        }
    
        public static double KuhleZufallsZahlen()
        {
            Random rnd = new Random();
            return -2 + rnd.Next(4) + rnd.NextDouble();
        }
     
        public static double[] Random1andn1Array(int lenght, double Prob1)
        {
            double[] a = new double[lenght];
            Random rnd = new Random();
        
            for(int i = 0; i < lenght; i++)
            {
                if (rnd.NextDouble() < Prob1)
                    a[i] = 1;
                else
                    a[i] = -1;
            }
            return a;
        }
    }
  

}    