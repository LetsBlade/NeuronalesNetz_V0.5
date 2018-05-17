using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz_V0._5.KI_Sachen
{
    public partial class SimpleNetRNN : SimpleNet
    {
        public List<double> rnnList;

        public SimpleNetRNN(double Etha, int rnnCount, params int[] NeuronAnz) : base(Etha, NeuronAnz)
        {
            this.Etha = Etha;
            rnnList = new List<double>(new double[rnnCount*NeuronAnz.Last()]);
            NeuronAnz[0] += rnnCount * NeuronAnz.Last();

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
                    Weights[i0][i1] = GetRandArr(NeuronAnz[i0 + 1], -2, 4);
                }
            }
        }

        public override void SetInput(double[] input)
        {
            if (input.Length + rnnList.Count > NeuronValue[0].Length)
            {
                throw new Exception("Der Input hat die Falsche Länge");
            }

            int addInput = 0;

            if (NeuronValue[0].Length + rnnList.Count > input.Length)
            {
                addInput = NeuronValue[0].Length - input.Length - rnnList.Count;
            }



            NeuronValue[0] = input;
            NeuronValue[0] = NeuronValue[0].Concat(new double[addInput]).ToArray();
            NeuronValue[0] = NeuronValue[0].Concat(rnnList.ToArray()).ToArray();
        }

        public override void OutputBerechnen()
        {
            base.OutputBerechnen();

            rnnList.AddRange(NeuronValue.Last());
            for(int i = 0; i < NeuronValue.Last().Length; ++i)
            {
                rnnList.RemoveAt(i);
            }
        }
    }
}
