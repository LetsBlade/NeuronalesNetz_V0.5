using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz_V0._5.KompliziertNetSachen
{


    public class NeuronalNet
    {
        public static readonly double zuWenig = 0.005;
        public double Etha;
        public List<Neuron>[] neuron;
        public double punkte;
        public List<double[]> inputLernVerwaltung;      //Letzte inputs für so lernen
        public List<double> letztePunkteList;           //Letzte Punkte
        public List<double[]> lastOutputList;           //letzte 5 outputs für so recurrent
        public int RnnCount;
        public Dictionary<NeuronPositionDelay,sim>

        public NeuronalNet( int RNNCount, params int[] neuronProLayerSeed)
        {
            neuron = new List<Neuron>[neuronProLayerSeed.Length];
            Etha = 0.5;
            punkte = 0;
            inputLernVerwaltung = new List<double[]>();
            letztePunkteList = new List<double>(new double[0]);
            lastOutputList = new List<double[]>();
            RnnCount = RNNCount;

            if (RNNCount>0)
                neuronProLayerSeed[0] += neuronProLayerSeed.Last() * RNNCount;

            for (int i0 = 0; i0 < neuronProLayerSeed.Length; ++i0)
            {
                neuron[i0] = new List<Neuron>();
                for (int i1 = 0; i1 < neuronProLayerSeed[i0]; ++i1)
                {
                    neuron[i0].Add(new Neuron());
                }
            }
            neuron[1].Add(neuron[1][0]);
            neuron[1][0] = new Neuron("b",1);



            for (int i = 1; i < neuronProLayerSeed.Length; ++i)
                LayerKommplettVerbinden(new NeuronPosition(1, 0), i, 100, 0);
        }

        public void StandartInitialisierung()
        {
            Random rnd = new Random();
            for (int i0 = 1; i0 < neuron.Length; ++i0)
            {
                ZweiLayerKommplettVerbinden(i0 - 1, i0, rnd.Next(70, 90));
            }
            MachSoEinPaarConnectionsKlar(90 + rnd.Next(60));

            List<NeuronPosition> listPos = new List<NeuronPosition>();
            for (int i0 = 200; i0 < neuron[0].Count; i0++)
            {
                listPos.Add(new NeuronPosition(0, i0));
            }
            MachSoEinPaarConnectionsKlarVong(listPos, 60 + rnd.Next(50));

            listPos = new List<NeuronPosition>();
            for (int i0 = 0; i0 < 5; ++i0)
            {
                listPos.Add(new NeuronPosition(neuron.Length - 1, i0));
            }
            MachSoEinPaarConnectionsKlarZu(listPos, 15 + rnd.Next(20));


            for (int i0 = 1; i0 < neuron.Length - 1; ++i0)
            {
                for (int i1 = 0; i1 < neuron[i0].Count; ++i1)
                {
                    neuron[i0][i1].AddRandomTyp(rnd.NextDouble() * 10, rnd.NextDouble() * 60, rnd.NextDouble() * 70);
                }
            }
            for (int i1 = 0; i1 < neuron.Last().Count; ++i1)
            {
                neuron[neuron.Length - 1][i1].SetAF("S", Convert.ToDouble(neuron[neuron.Length - 1][i1].connection.Count) / 4);
            }
        }

        public void StatischeInitialisierung(bool Tanh)
        {
            for (int i = 1; i < neuron.Length; ++i)
            {
                ZweiLayerKommplettVerbinden(i - 1, i, 100, 0);
            }

            if (Tanh)
            {
                for (int i0 = 1; i0 < neuron.Length; ++i0)
                {
                    for (int i1 = 0; i1 < neuron[i0].Count; ++i1)
                    {
                        neuron[i0][i1].SetAF("T", 10);
                    }
                }
            }
            else
            {
                for (int i0 = 1; i0 < neuron.Length; ++i0)
                {
                    for (int i1 = 0; i1 < neuron[i0].Count; ++i1)
                    {
                        neuron[i0][i1].SetAF("S", 10);
                    }
                }
            }


            for(int i = 0; i < neuron[2].Count; ++i)
            {
                neuron[2][i].connection.RemoveAt(0);
            }
        }

        #region Brudah, ich hab Connections

        public void LayerKommplettVerbinden(NeuronPosition pos, int layer, double connectionPropatility)    //fügt jedem Neuron in layer eine connection zu pos hinzu
        {
            Random rnd = new Random();
            connectionPropatility /= 100;
            for (int i = 0; i < neuron[layer].Count; ++i)
            {
                if (rnd.NextDouble() <= connectionPropatility)
                    neuron[layer][i].AddConnection(pos, Variablen.KuhleZufallsZahlen());
            }
        }

        public void NeuronKommplettVerbinden(NeuronPosition pos, int layer, double connectionProbatility)   //fügt dem Neuronen pos eine connection zu jedem neuronen in layer zu
        {
            connectionProbatility /= 100;
            Random rnd = new Random();
            for (int i = 0; i < neuron[layer].Count; ++i)
            {
                if (rnd.NextDouble() <= connectionProbatility)
                    neuron[pos.layer][pos.height].AddConnection(new NeuronPosition(layer, i), Variablen.KuhleZufallsZahlen());
            }
        }

        public void LayerKommplettVerbinden(NeuronPosition pos, int layer, double connectionProbatility, int delay)
        {
            Random rnd = new Random();
            connectionProbatility /= 100;
            for (int i = 0; i < neuron[layer].Count; ++i)
            {
                if (rnd.NextDouble() <= connectionProbatility)
                    neuron[layer][i].AddConnection(pos, Variablen.KuhleZufallsZahlen(), delay);
            }
        }

        public void NeuronKommplettVerbinden(NeuronPosition pos, int layer, double connectionProbatility, int delay)
        {
            connectionProbatility /= 100;
            Random rnd = new Random();
            for (int i = 0; i < neuron[layer].Count; ++i)
            {
                if (rnd.NextDouble() < connectionProbatility)
                    neuron[pos.layer][pos.height].AddConnection(new NeuronPosition(layer, i), Variablen.KuhleZufallsZahlen(), delay);
            }
        }

        public void ZweiLayerKommplettVerbinden(int layer0, int layer1, double connectionProbatility)
        {
            for (int i = 0; i < neuron[layer0].Count; ++i)
            {
                LayerKommplettVerbinden(new NeuronPosition(layer0, i), layer1, connectionProbatility);
            }
        }

        public void ZweiLayerKommplettVerbinden(int layer0, int layer1, double connectionProbatility, int delay)
        {
            for (int i = 0; i < neuron[layer0].Count; ++i)
            {
                LayerKommplettVerbinden(new NeuronPosition(layer0, i), layer1, connectionProbatility, delay);
            }
        }

        public void MachVerbindung(NeuronPosition vong, NeuronPosition zu, double weight)
        {
            neuron[vong.layer][vong.height].AddConnection(zu, weight);
        }

        public void MachSoEinPaarConnectionsKlar(int count)
        {
            Random rnd = new Random();
            for (int i = 0; i < count; ++i)
            {
                int layer0 = rnd.Next(neuron.Length);
                int height0 = rnd.Next(neuron[layer0].Count);

                int layer1 = rnd.Next(1, neuron.Length);
                int height1 = rnd.Next(neuron[layer1].Count);

                neuron[layer1][height1].AddConnection(new NeuronPosition(layer0, height0), Variablen.KuhleZufallsZahlen());
            }
        }

        public void MachSoEinPaarConnectionsKlarZu(NeuronPosition pos, int count)
        {
            Random rnd = new Random();
            for (int i = 0; i < count; ++i)
            {
                int layer0 = rnd.Next(neuron.Length);
                int height0 = rnd.Next(neuron[layer0].Count);

                neuron[pos.layer][pos.height].AddConnection(new NeuronPosition(layer0, height0));
            }
        }

        public void MachSoEinPaarConnectionsKlarZu(List<NeuronPosition> pos, int count)
        {
            Random rnd = new Random();
            for (int i = 0; i < count; ++i)
            {
                int layer0 = rnd.Next(neuron.Length);
                int height0 = rnd.Next(neuron[layer0].Count);

                int a = rnd.Next(pos.Count);

                neuron[pos[a].layer][pos[a].height].AddConnection(new NeuronPosition(layer0, height0));
            }
        }

        public void MachSoEinPaarConnectionsKlarVong(NeuronPosition pos, int count)
        {
            Random rnd = new Random();
            for (int i = 0; i < count; ++i)
            {
                int layer1 = rnd.Next(1, neuron.Length);
                int height1 = rnd.Next(neuron[layer1].Count);

                neuron[layer1][height1].AddConnection(pos, Variablen.KuhleZufallsZahlen());
            }
        }

        public void MachSoEinPaarConnectionsKlarVong(List<NeuronPosition> pos, int count)
        {
            Random rnd = new Random();
            for (int i = 0; i < count; ++i)
            {
                int layer1 = rnd.Next(1, neuron.Length);
                int height1 = rnd.Next(neuron[layer1].Count);

                neuron[layer1][height1].AddConnection(pos[rnd.Next(pos.Count)]);
            }
        }

        public void MachSoEinPaarConnectionsKlar(NeuronPosition pos, int countZu, int countVong)
        {
            MachSoEinPaarConnectionsKlarZu(pos, countZu);
            MachSoEinPaarConnectionsKlarVong(pos, countVong);
        }

        public void AddNeuron(int layer, int connectionsZu, int connectionsVong)
        {
            neuron[layer].Add(new Neuron());
            MachSoEinPaarConnectionsKlar(new NeuronPosition(layer, neuron[layer].Count - 1), connectionsZu, connectionsVong);

            neuron[layer][neuron[layer].Count - 1].AddRandomTyp(5, 40, 40);
        }

        public void RemoveNeuron(NeuronPosition pos)
        {
            neuron[pos.layer].RemoveAt(pos.height);

            for (int i0 = 1; i0 < neuron.Length; ++i0)
            {
                for (int i1 = 0; i1 < neuron[i0].Count; ++i1)
                {
                    for (int i2 = 0; i2 < neuron[i0][i1].connection.Count; ++i2)
                    {
                        if (neuron[i0][i1].connection[i2] != null)
                        {
                            if (!IstInKi(neuron[i0][i1].connection[i2].Pos))
                            {
                                neuron[i0][i1].connection.RemoveAt(i2);
                            }

                            //if (neuron[i0][i1].connection[i2].Pos == pos)
                            //{
                            //    neuron[i0][i1].connection.RemoveAt(i2);
                            //}
                            //else if (neuron[i0][i1].connection[i2].Pos.layer == pos.layer && neuron[i0][i1].connection[i2].Pos.height > pos.height)
                            //{
                            //    neuron[i0][i1].connection[i2].Pos.height = (neuron[i0][i1].connection[i2].Pos.height);
                            //}
                        }
                        else
                        {
                            neuron[i0][i1].connection.RemoveAt(i2);
                        }
                    }
                }
            }
        }

        public bool IstInKi(NeuronPosition pos)
        {
            bool ergebniss = true;

            if (neuron.Length <= pos.layer)
                ergebniss = false;
            if (neuron[pos.layer].Count <= pos.height)
                ergebniss = false;

            return ergebniss;
        }

        #endregion


        public bool LetzterOutputUber(double value)
        {
            bool a = false;
            if (letztePunkteList.Count >= 5)
            {
                for (int i0 = 0; i0 < lastOutputList.Count && !a; ++i0)
                {
                    for (int i1 = 0; i1 < lastOutputList[i0].Length && !a; ++i1)
                    {
                        if (lastOutputList[i0][i1] > value)
                            a = true;
                    }
                }
            }
            return a;
        }

        public double[] GetValues(int Layer)
        {
            double[] output = new double[neuron[Layer].Count];

            for (int i = 0; i < output.Length; ++i)
            {
                output[i] = neuron[Layer][i].CurrentOutput;
            }

            return output;
        }

        public List<double> GetValues(List<Connection> con)
        {
            List<double> a = new List<double>();

            foreach (Connection item in con)
            {
                a.Add(neuron[item.Pos.layer][item.Pos.height].CurrentOutput);
            }
            return a;
        }

        public double[] GetValuesArr(List<Connection> con)
        {
            double[] a = new double[con.Count];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = neuron[con[i].Pos.layer][con[i].Pos.height].CurrentOutput;
            }
            //Parallel.For(0, a.Length, i =>
            //{
            //    a[i] = neuron[con[i].Pos.layer][con[i].Pos.height].outputWert;
            //});

            return a;
        }

        public double GetValue(NeuronPosition Pos)
        {
            return neuron[Pos.layer][Pos.height].CurrentOutput;
        }

        public List<double> GetValue(List<NeuronPosition> Pos)
        {
            List<double> ergebniss = new List<double>();

            foreach (NeuronPosition item in Pos)
            {
                ergebniss.Add(GetValue(item));
            }
            return ergebniss;
        }

        public void InputGeben(double[] inputValues)
        {
            if (RnnCount > 0)
            {
                foreach (double[] item in lastOutputList)
                {
                    inputValues = inputValues.Concat(item).ToArray();
                }
            }

            for (int i = 0; i < neuron[0].Count; ++i)
            {
                neuron[0][i].MoveLastOutput();
                neuron[0][i].CurrentOutput = 0;
            }

            for (int i = 0; i < inputValues.Length; ++i)
            {
                neuron[0][i].MoveLastOutput();
                neuron[0][i].CurrentOutput = inputValues[i];
            }
        }

        public void AllesBerechnen()
        {
            if (RnnCount > 0)
            {
                if (lastOutputList.Count < RnnCount)
                {
                    lastOutputList.Add(GetValues(neuron.Length - 1));
                }
                else
                {
                    lastOutputList.RemoveAt(0);
                    lastOutputList.Add(GetValues(neuron.Length - 1));
                }
            }

            for (int i0 = 1; i0 < neuron.Length; ++i0)
            {
                for (int i1 = 0; i1 < neuron[i0].Count; ++i1)
                {
                    neuron[i0][i1].OutputBerechnen(GetValuesArr(neuron[i0][i1].connection));
                }
            }
        }

        public double[] GetOutput()
        {
            double[] a = new double[neuron.Last().Count];

            for (int i = 0; i < a.Length; ++i)
            {
                a[i] = neuron.Last()[i].CurrentOutput;
            }
            return a;
        }

        public double[] GetRoundOutput()
        {
            double[] a = new double[neuron.Last().Count];

            for (int i = 0; i < a.Length; ++i)
            {
                a[i] = Math.Round(neuron.Last()[i].CurrentOutput);
            }
            return a;
        }

        public void RoundOutput()
        {
            Parallel.For(0, neuron.Last().Count, i =>
            {
                neuron[neuron.Length - 1][i].CurrentOutput = Math.Round(neuron[neuron.Length - 1][i].CurrentOutput);
            });
        }

        public void ResetErinnerungen()
        {
            for (int i0 = 1; i0 < neuron.Length; ++i0)
            {
                for (int i1 = 0; i1 < neuron[i0].Count; ++i1)
                {
                    for (int i2 = 0; i2 < neuron[i0][i1].connection.Count; ++i2)
                    {
                        neuron[i0][i1].connection[i2].ErinnerungenReset();
                    }
                }
            }
        }

        public void ResetDelta()
        {
            for (int i0 = 1; i0 < neuron.Length; ++i0)
            {
                for (int i1 = 0; i1 < neuron[i0].Count; ++i1)
                {
                    neuron[i0][i1].CurrentDelta = 0;
                }
            }
        }

        public void DeepLearning(double[] outputSoll)
        {

            Parallel.For(0, outputSoll.Length, i =>
            {
                 neuron[neuron.Length - 1][i].CurrentDelta = outputSoll[i] - neuron[neuron.Length - 1][i].CurrentOutput;
                 neuron[neuron.Length - 1][i].CalcDelta();
            });


            for (int i0 = neuron.Length - 1; i0 > 0; --i0)
            {
                for (int i1 = 0; i1 < neuron[i0].Count; ++i1)
                {
                    for(int i2=0;i2< neuron[i0][i1].connection.Count; ++i2)
                    {
                        neuron[neuron[i0][i1].connection[i2].Pos.layer][neuron[i0][i1].connection[i2].Pos.height].CurrentDelta += neuron[i0][i1].connection[i2].Weight * neuron[i0][i1].lastDeltaList[neuron[i0][i1].connection[i2].delay];
                    }
                    neuron[i0][i1].CalcDelta();
                }
            }

            for (int i0 = neuron.Length - 1; i0 > 0; --i0)
            {
                for(int i1 = 0; i1 < neuron[i0].Count; ++i1)
                {
                    neuron[i0][i1].BackpropaDingsBumbs(Etha);
                }
            }

        }

        public void FactorAktualisieren()
        {
            for(int i0 = 0; i0 < neuron.Length; ++i0)
            {
                for (int i1 = 0; i1 < neuron[i0].Count; ++i1)
                {
                    Parallel.For(0, neuron[i0][i1].connection.Count, i2 => 
                    {
                        neuron[neuron[i0][i1].connection[i2].Pos.layer][neuron[i0][i1].connection[i2].Pos.height].OutputFactor += neuron[i0][i1].connection[i2].lastChange;
                    });
                }
            }
        }

        public double GetFehlerDif(double[] outputSoll)
        {
            double a = 0;

            for(int i = 0; i < neuron.Last().Count; ++i)
            {
                a += Math.Abs(neuron.Last()[i].CurrentOutput - outputSoll[i]);
            }

            return a;
        }

        public Neuron GetNeuron(NeuronPosition p)
        {
            return neuron[p.layer][p.height];
        }

        public double[] GetLastOutputs(params NeuronPosition[] pos)
        {
            double[] result = new double[0];

            foreach(NeuronPosition p in pos)
            {
                result = result.Concat(GetNeuron(p).lastOutputList).ToArray();
            }

            return result;
        }

        public void Cut(int layer)
        {
            for (int i0 = neuron.Length - 1; i0 > layer; --i0)
            {
                NeuronPosition a = new NeuronPosition(i0, 0);

                for (int i1 = 0; i1 < neuron[i0].Count;)
                {
                    RemoveNeuron(a);
                }

            }


            List<Neuron>[] temp = new List<Neuron>[layer + 1];

            for (int i = 0; i <= layer; ++i)
            {
                temp[i] = neuron[i];
            }

            neuron = temp;
        }

        public void UnnotigeNeuronenUndSoWegMachen()
        {
            for (int i0 = 1; i0 < neuron.Length; ++i0)
            {
                for (int i1 = 0; i1 < neuron[i0].Count; ++i1)
                {
                    if (neuron[i0][i1].GetAbsLastOutputAverage() < zuWenig)
                    {
                        RemoveNeuron(new NeuronPosition(i0, i1));
                        --i1;
                    }
                    else
                    {
                        for (int i2 = 0; i2 < neuron[i0][i1].connection.Count; ++i2)
                        {
                            if(Math.Abs(neuron[i0][i1].connection[i2].Weight) < zuWenig)
                            {
                                neuron[i0][i1].connection.RemoveAt(i2);
                                --i2;
                            }
                        }
                    }
                }
            }
        }

        public static NeuronalNet operator +(NeuronalNet n1, NeuronalNet n2)
        {
            Random rnd = new Random();
            for (int i = 1; i < n1.neuron.Length - 1; ++i)
            {
                if (n1.neuron[i].Count > n2.neuron[i].Count)
                {
                    while (n1.neuron[i].Count > n2.neuron[i].Count && rnd.NextDouble() < 0.8)
                    {
                        if (rnd.Next(2) == 0)
                        {
                            n1.RemoveNeuron(new NeuronPosition(i, rnd.Next(n1.neuron[i].Count)));
                        }
                        else
                        {
                            n2.AddNeuron(i, 15 + rnd.Next(10), 16 + rnd.Next(10));
                        }
                    }
                }
                else if (n1.neuron[i].Count < n2.neuron[i].Count)
                {
                    while (n1.neuron[i].Count < n2.neuron[i].Count && rnd.NextDouble() < 0.8)
                    {
                        if (rnd.Next(2) == 0)
                        {
                            n2.RemoveNeuron(new NeuronPosition(i, rnd.Next(n2.neuron[i].Count)));
                        }
                        else
                        {
                            n1.AddNeuron(i, 15 + rnd.Next(10), 16 + rnd.Next(10));
                        }
                    }
                }

            }

            for (int i0 = 1; i0 < n1.neuron.Length; ++i0)
            {
                for (int i1 = 0; i1 < n1.neuron[i0].Count; ++i1)
                {
                    if (n1.neuron[i0].Count <= n2.neuron[i0].Count)
                    {
                        n1.neuron[i0][i1].connection.AddRange(n2.neuron[i0][i1].connection);
                        int lange = n1.neuron[i0][i1].connection.Count / 2;

                        for (int i2 = 0; i2 < n1.neuron[i0][i1].connection.Count; ++i2)
                        {
                            if (double.IsNaN(n1.neuron[i0][i1].connection[i2].Weight))
                            {
                                n1.neuron[i0][i1].connection.RemoveAt(i2);
                            }
                        }

                        double toleranz = 0.01;
                        double average = n1.neuron[i0][i1].GetWeightAverage();

                        for (int i2 = 0, toleranzMulti = 1; n1.neuron[i0][i1].connection.Count > lange; ++i2, toleranzMulti += 2)
                        {
                            //wenn der 1 mal durch ist, fängt der wieder vong vorne an
                            if (i2 >= n1.neuron[i0][i1].connection.Count - 1)
                                i2 = 0;


                            if (n1.neuron[i0][i1].connection[i2].Weight < average / 3.5)
                            {
                                n1.neuron[i0][i1].connection.RemoveAt(i2);
                            }
                            else if (n1.neuron[i0][i1].connection[i2].Weight > -(toleranz * toleranzMulti) && n1.neuron[i0][i1].connection[i2].Weight < (toleranz * toleranzMulti))
                            {
                                n1.neuron[i0][i1].connection.RemoveAt(i2);
                            }

                        }

                        //Die Connections entfernen, welche jetzt ins nichts führen
                        for (int i2 = 0; i2 < n1.neuron[i0][i1].connection.Count; ++i2)
                        {
                            if (!n1.IstInKi(n1.neuron[i0][i1].connection[i2].Pos))
                                n1.neuron[i0][i1].connection.RemoveAt(i2);
                        }

                        //for (int i2 = 0; i2 < n1.neuron[i0][i1].connection.Count && i2 < n2.neuron[i0][i1].connection.Count; ++i2) 
                        //{
                        //    if((n1.neuron[i0][i1].connection[i2].Pos == n2.neuron[i0][i1].connection[i2].Pos) ^ rnd.NextDouble() < 0.15)
                        //        n1.neuron[i0][i1].connection[i2].Weight = (n1.neuron[i0][i1].connection[i2].Weight + n2.neuron[i0][i1].connection[i2].Weight) / 2; 
                        //}

                        if (rnd.Next(2) == 0)
                        {
                            n1.neuron[i0][i1].SetAF(n2.neuron[i0][i1]);
                        }

                    }
                    else
                    {
                        i1 = 10000;
                    }
                }
            }

            n1.inputLernVerwaltung = new List<double[]>();
            n1.letztePunkteList = new List<double>();
            n1.ResetDelta();
            n1.ResetErinnerungen();

            return n1;
        }

        public override string ToString()
        {
            string name = "NeuronalNet\n\tNeuronen Anzahl\n\t\tLayer Anzahl:\t" + neuron.Length;
            for (int i0 = 0; i0 < neuron.Length; ++i0)
            {
                name += "\n\t\tLayer " + i0 + ": " + neuron[i0].Count + " Neuronen";
            }
            name += "\n\tPunkte:\t" + punkte + "\n\tEtha:\t" + Etha + "\n";

            return name;
        }
    }
}
