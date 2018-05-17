using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuronalesNetz_V0._5.LearnAlgos;

namespace NeuronalesNetz_V0._5
{
    public class Neuron
    {
        public static readonly int MaxDelay = 20;
        public List<Connection> connection;
        public List<NeuronPosition> zuPos;
        public List<double> lastOutputList;
        public List<double> lastDeltaList;
        public string bezeichnung;
        public double[] parameter;
        public double CurrentOutput
        {
            get
            {
                if(lastOutputList.Count > 0)
                {
                    return lastOutputList[0];
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                lastOutputList[0] = value;
            }
        }
        public double CurrentDelta
        {
            get
            {
                if(lastDeltaList.Count > 0)
                {
                    return lastDeltaList[0];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                lastDeltaList[0] = value;
            }
        }
        public double OutputFactor
        {
            get
            {
                return parameter.Last();
            }
            set
            {
                parameter[parameter.Length - 1]=value;
            }
        }

        public Neuron(string bezeichnung0,params double[] parameter0)
        {
            connection = new List<Connection>();
            bezeichnung = bezeichnung0;
            parameter = parameter0;
            lastDeltaList = new List<double>(new double[MaxDelay]);
            lastOutputList = new List<double>(new double[MaxDelay]);
        }
        public Neuron(List<Connection> connection0)
        {
            connection = connection0;
            bezeichnung = "";
            parameter = new double[0];
            lastDeltaList = new List<double>(new double[MaxDelay]);
            lastOutputList = new List<double>(new double[MaxDelay]);
        }
        public Neuron()
        {
            connection = new List<Connection>();
            bezeichnung = "";
            parameter = new double[0];
            lastDeltaList = new List<double>(new double[MaxDelay]);
            lastOutputList = new List<double>(new double[MaxDelay]);
        }

        public double OutputBerechnen(double[] connectionsToValue)
        {
            MoveLastOutput();
            double a = 0;

            for(int i = 0; i < connection.Count; ++i)
            {
                a += connection[i].GetValue(connectionsToValue[i]);
            }

            CurrentOutput = NeuroMaths.Formel(bezeichnung,a,parameter);

            return CurrentOutput;
        }

        public void MoveLastDelta()
        {
            for(int i = lastDeltaList.Count - 2; i >= 0; --i)
            {
                lastDeltaList[i + 1] = lastDeltaList[i];
            }
        }

        public void MoveLastOutput()
        {
            for (int i = lastOutputList.Count - 2; i >= 0; --i)
            {
                lastOutputList[i + 1] = lastOutputList[i];
            }
        }

        public void AddParameterHinten(char art, double t)
        {
            bezeichnung = bezeichnung + art;
            bezeichnung = LinearClassifier.SwitchPosInArr(bezeichnung.ToArray(), bezeichnung.Length - 1, bezeichnung.Length - 2);
            double[] a = { t };
            parameter = parameter.Concat(a).ToArray();
            LinearClassifier.SwitchPosInArr(parameter, parameter.Length - 1, parameter.Length - 2);
        }
        public void AddParameterVorne(char art, double t)
        {
            bezeichnung = art + bezeichnung;
            double[] a = { t };
            parameter = a.Concat(parameter).ToArray();
        }
        public void AddParameterHinten(char art)
        {
            bezeichnung = bezeichnung + art;
            bezeichnung = LinearClassifier.SwitchPosInArr(bezeichnung.ToArray(), bezeichnung.Length - 1, bezeichnung.Length - 2);
            double[] a = { 1 };
            parameter = parameter.Concat(a).ToArray();
            LinearClassifier.SwitchPosInArr(parameter, parameter.Length - 1, parameter.Length - 2);
        }
        public void AddParameterVorne(char art)
        {
            bezeichnung = art + bezeichnung;
            double[] a = { 1 };
            parameter = a.Concat(parameter).ToArray();
        }

        public void SetAF(string bezeichnung0,params double[] parameter0)
        {
            bezeichnung = bezeichnung0;
            parameter = parameter0;
        }
        public void SetAF(Neuron n)
        {
            bezeichnung = n.bezeichnung;
            parameter = n.parameter;
        }

        public void AddConnection(NeuronPosition zu, double weight)
        {
            connection.Add(new Connection(zu, weight));
        }

        public void AddConnection(NeuronPosition zu)
        {
            connection.Add(new Connection(zu));
        }

        public void AddConnection(NeuronPosition zu, double weight, int delay)
        {
            connection.Add(new Connection(zu, weight, delay));
        }

        public void AddRandomTyp(double linearProb, double schwellwertProb, double wiederholenProp)
        {
            Random rnd = new Random();
            linearProb /= 100;
            schwellwertProb /= 100;
            wiederholenProp /= 100;

            do
            {
                if (rnd.NextDouble() < linearProb)
                {
                    if (rnd.Next(2) == 0)
                        AddParameterHinten('L', -2.5 + rnd.NextDouble() * 5);
                    else
                        AddParameterVorne('L', -2.5 + rnd.NextDouble() * 5);
                }
                else if (rnd.NextDouble() < schwellwertProb)
                {
                    if (rnd.Next(2) == 0)
                    {
                        AddParameterVorne('C', -7 + rnd.NextDouble() * 14);
                    }
                    else
                    {
                        if (bezeichnung.Contains("S"))
                        {
                            if (bezeichnung.Substring(bezeichnung.IndexOf('S')).Contains("L"))
                            {
                                double a = 1;
                                for (int i = bezeichnung.IndexOf('S') + 1; i < bezeichnung.Length; ++i)
                                {
                                    if (bezeichnung[i] == 'L')
                                    {
                                        a *= parameter[i];
                                    }
                                    else if (bezeichnung[i] == 'S')
                                    {
                                        a = 1;
                                    }
                                }
                                AddParameterHinten('C', -a + rnd.NextDouble() * (2 * a));
                            }
                            else
                            {
                                AddParameterHinten('C', -1 + rnd.NextDouble() * 2);
                            }
                        }
                        else
                        {
                            AddParameterHinten('C', -4 + rnd.NextDouble() * 8);
                        }
                    }
                }
                else
                {
                    if (rnd.Next(2) == 0)
                    {
                        AddParameterVorne('S', Convert.ToDouble(connection.Count) / 4);
                        if (bezeichnung.Contains('C') || bezeichnung.Contains('S'))
                        {
                            double a = 1;
                            for (int i = 1; i < bezeichnung.Length; ++i)
                            {
                                if (bezeichnung[i] == 'C')
                                {
                                    parameter[i] = -a + rnd.NextDouble() * (a * 2);
                                }
                                else if (bezeichnung[i] == 'L')
                                {
                                    a *= parameter[i];
                                }
                                else if (bezeichnung[i] == 'S')
                                {
                                    a = 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (bezeichnung.Contains('L') || bezeichnung.Contains('S'))
                        {
                            double a = 1;
                            for (int i = 0; i < bezeichnung.Length; ++i)
                            {
                                if (bezeichnung[i] == 'L')
                                    a *= parameter[i];
                                else if (bezeichnung[i] == 'S')
                                    a = 1;
                            }
                            AddParameterHinten('S', a);
                        }
                        else
                        {
                            AddParameterVorne('S', Convert.ToDouble(connection.Count) / 4);
                        }
                    }
                }
            } while (rnd.NextDouble() < wiederholenProp);
        }

        public void BackpropaDingsBumbs(double etha)
        {
            Parallel.For(0, connection.Count, i =>
            {
                connection[i].MachBackpropadings(CurrentDelta, etha);
            });
        }

        public void BackpropaDingsBumbs(double etha, int connectionIndex)
        {
            connection[connectionIndex].MachBackpropadings(CurrentDelta, etha);
        }

        public void CalcDelta()
        {
            MoveLastDelta();
            CurrentDelta *= NeuroMaths.Ableitung(bezeichnung, CurrentOutput, parameter);
        }

        public double GetWeightAverage()
        {
            if (connection != null)
            {
                if(connection.Contains(null))
                    connection.RemoveAll(null);

                double a = 0;
                double b = connection.Count;

                for (int i = 0; i < connection.Count && !double.IsNaN(connection[i].Weight); ++i)
                {
                    if (!double.IsNaN(connection[i].Weight))
                    {
                        a += connection[i].Weight;
                    }
                    else
                    {
                        b--;
                    }
                }
                a /= b;

                return a;
            }
            else
            {
                throw new Exception("Connection list ist NULL ALARM WIU WIU WIIU BUMMMMMM EXPLOSION");
            }
        }

        public double GetAbsLastOutputAverage()
        {
            double x = 0;

            for(int i = 0; i < MaxDelay; ++i)
            {
                x += Math.Abs(lastOutputList[i]);
            }
            x /= MaxDelay;

            return x;
        }

        public override string ToString()
        {
            string name = "Neuron\n\tConnection Count:\t" + connection.Count;
            for (int i = 0; i < bezeichnung.Length; ++i)
            {
                if (bezeichnung[i] == 's')
                {
                    name += "\n\tSigmoid";
                }
                else if (bezeichnung[i] == 'b')
                {
                    name += "\n\tBias";
                }
                else if (bezeichnung[i] == 't')
                {
                    name += "\n\tTanh";
                }
                else if (bezeichnung[i] == 'c')
                {
                    name += "\n\tSchwellwert";
                }

                else if (bezeichnung[i] == 'S')
                {
                    name += "\n\tSigmoid\n\t\tParameter:\t" + parameter[i];
                }
                else if (bezeichnung[i] == 'L')
                {
                    name += "\n\tLinear\n\t\tParameter:\t" + parameter[i];
                }
                else if (bezeichnung[i] == 'C')
                {
                    name += "\n\tSchwellwert\n\t\tParameter:\t" + parameter[i];
                }
                else if (bezeichnung[i] == 'B')
                {
                    name += "\n\tBias\n\t\tParameter:\t" + parameter[i];
                }
                else if (bezeichnung[i] == 'T')
                {
                    name += "\n\tTanh\n\t\tParameter:\t" + parameter[i];
                }
            }

            return name;
        }

        public string ToString(int lol)
        {
            string name = "Neuron\n\tConnection Count:\t" + connection.Count;
            for (int i = 0; i < connection.Count; ++i)
            {
                name += "\n\t\t" + connection[i].ToString();
            }
            name += "\n" + ToString() + "\n";
            return name;
        }
    }
}
