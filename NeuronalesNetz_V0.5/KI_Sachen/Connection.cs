using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz_V0._5
{

    public class Connection
    {
        public int delay;
        public NeuronPosition Pos;
        public double Weight;
        public List<double> time;
        private static Random rnd = new Random();
        public double lastChange;

        public Connection(NeuronPosition pos0, double newWeight, int Delay)
        {
            lastChange = 0;
            Pos = pos0;
            Weight = newWeight;
            delay = Delay;
            time = new List<double>(new double[Delay + 1]);
        }

        public Connection(NeuronPosition pos0, double newWeight)
        {
            lastChange = 0;
            int a = rnd.Next(100);

            Pos = pos0;
            Weight = newWeight;

            if (a > 7)
                delay = 0;
            else
                delay = a;
            time = new List<double>(new double[delay + 1]);
        }

        public Connection(NeuronPosition pos0)
        {
            lastChange = 0;
            int a = rnd.Next(100);

            Pos = pos0;
            Weight = Variablen.KuhleZufallsZahlen();

            if (a > 7)
                delay = 0;
            else
                delay = a;
            time = new List<double>(new double[delay + 1]);
        }

        public void ErinnerungenReset()
        {
            time = new List<double>(new double[delay + 1]);
        }

        public double GetValue(double value)
        {
            time.Add(value * Weight);
            time.RemoveAt(0);

            return time[0];
        }

        public void MachBackpropadings(double deltaVongI, double etha)
        {
            lastChange = 0;
            if (!double.IsNaN((time[0] / Weight)))
            {
                lastChange = (time[0] / Weight);

                lastChange *= deltaVongI * etha;
                Weight += lastChange;
            }

        }

        public override string ToString()
        {
            string name = "Connection\n\tDelay:\t";
            name += delay + "\n" + Pos.ToString();
            name += "\n\tWeight:\t" + Weight + "\n\tTime:";
            for (int i = 0; i < time.Count; ++i)
            {
                name += "\n\t\t" + time[i];
            }
            return name + "\n";
        }

        public static Connection operator +(Connection c0,Connection c1)
        {
            if (c0 != c1)
            {
                throw new Exception("Es dürfen nur gleiche Connections addiert werden!");
            }

            c0.Weight += c1.Weight;
            c0.ErinnerungenReset();
            return c0;
        }

        public static bool operator ==(Connection c0, Connection c1)
        {
            return c0.delay == c1.delay && c0.Pos == c1.Pos;
        }

        public static bool operator !=(Connection c0, Connection c1)
        {
            return !(c0.delay == c1.delay && c0.Pos == c1.Pos);
        }
    }

}
