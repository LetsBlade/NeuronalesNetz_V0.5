using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz_V0._5.KompliziertNetSachen
{
    [Serializable]
    public class Connection : IEquatable<Connection>
    {
        public int delay;
        public NeuronPosition Pos;
        public double Weight;
        public List<double> time;
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
            Random rnd = new Random();
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
            Random rnd = new Random();
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

        #region voll kompliziert
        public override bool Equals(object obj)
        {
            return Equals(obj as Connection);
        }

        public bool Equals(Connection other)
        {
            return other != null &&
                   delay == other.delay &&
                   EqualityComparer<NeuronPosition>.Default.Equals(Pos, other.Pos);
        }

        public override int GetHashCode()
        {
            var hashCode = -1831020351;
            hashCode = hashCode * -1521134295 + delay.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<NeuronPosition>.Default.GetHashCode(Pos);
            return hashCode;
        }
        #endregion

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
