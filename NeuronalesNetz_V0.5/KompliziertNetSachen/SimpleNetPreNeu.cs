using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuronalesNetz_V0._5.SimpleNetKlassen;

namespace NeuronalesNetz_V0._5.KompliziertNetSachen
{
    public class SimpleNetPreNeu : SimpleNet
    {
        public int delay;
        public NeuronPosition[] informations;

        public SimpleNetPreNeu(double Etha, int delay, params int[] NeuronAnz) : base(Etha, NeuronAnz)
        {
            this.delay = delay;
        }

        public void SetInformationSource(params NeuronPosition[] pos)
        {
            informations = pos;
        }


    }
}
