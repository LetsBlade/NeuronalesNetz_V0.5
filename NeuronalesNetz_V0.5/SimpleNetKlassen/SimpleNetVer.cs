using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz_V0._5.SimpleNetKlassen
{
    public class SimpleNetVer : SimpleNet
    {
        public List<double[]> lastInput;
        public int repeatCount;

        public SimpleNetVer(int repeatCount, double Etha, double rndStart, double rndLenght, params int[] NeuronAnz) : base(Etha, rndStart, rndLenght, NeuronAnz)
        {
            this.repeatCount = repeatCount;
            lastInput = new List<double[]>();
        }

        public override void SetInput(double[] input)
        {
            lastInput.Add(input);
            if (lastInput.Count > repeatCount)
                lastInput.RemoveAt(0);

            base.SetInput(input);
        }


        public void LearnWithRegister(double reward, bool useAbleitung, double abstieg)
        {
            for(int i = lastInput.Count - 1; i > 0; --i)
            {
                SetInput(lastInput[i]);
                OutputBerechnen();

                base.ReinforcementLearning2(reward * Math.Pow(abstieg, i), useAbleitung);
            }

        }
    }
}
