using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NeuronalesNetz_V0._5
{
  
    class Program
    {
        static void Main(string[] args)
        {

            Programme.GenerateAnimie(@"C:\gesichter arr", @"C:\byteArr für ki");

            Programme.TestLC();
            Programme.SimpleTetris();
            Programme.Q_Learning_Tetris();
            Programme.SimpleTetris();
            Programme.SimpleBilderErkennen.Run(1000000,1000);
            Programme.SimplesSpielStart();
            //Console.ReadLine();
            Programme.BilderErkennen.Run(100000,100);


            Console.ReadLine();
        }

        public static void ArrayAusgeben(double[] arr)
        {
            for (int i = 0; i < arr.Length; ++i)
                Console.Write("{ 0:F4}\t", arr[i]);

            Console.WriteLine("\n");
        }

        public static void ArrayAusgeben(Array arr)
        {
            foreach(object item in arr)
            {
                Console.Write(item + "\t");
            }
        }


        public static void Serialize(object t, string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, t);
            }
        }
        //Could explicitly return 2d array, 
        //or be casted from an object to be more dynamic
        public static object Deserialize(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                return bformatter.Deserialize(stream);
            }
        }


    }


}
