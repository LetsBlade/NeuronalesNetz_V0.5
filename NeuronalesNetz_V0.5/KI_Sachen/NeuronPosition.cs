﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz_V0._5
{


    public class NeuronPosition : IEquatable<NeuronPosition>
    {
        public int layer;
        public int height;

        public NeuronPosition(int Layer, int Height)
        {
            this.layer = Layer;
            this.height = Height;
        }

        #region Vallah chäck ich nicht
        public override bool Equals(object obj)
        {
            return obj is NeuronPosition && Equals((NeuronPosition)obj);
        }
        public bool Equals(NeuronPosition other)
        {
            return layer == other.layer &&
                   height == other.height;
        }
        public override int GetHashCode()
        {
            var hashCode = 1654509736;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + layer.GetHashCode();
            hashCode = hashCode * -1521134295 + height.GetHashCode();
            return hashCode;
        }
        #endregion

        public override string ToString()
        {
            string name = "NeuronPosition\n\tLayer:\t";
            name += layer;
            name += "\n\tHeight:\t";
            name += height;
            return name;
        }

        public static bool operator ==(NeuronPosition p1, NeuronPosition p2)
        {
            return p1.height == p2.height && p1.layer == p2.layer;
        }

        public static bool operator !=(NeuronPosition p1, NeuronPosition p2)
        {
            return !(p1.height == p2.height && p1.layer == p2.layer);
        }

        public static bool operator >(NeuronPosition p1, NeuronPosition p2)
        {
            return p1.layer > p2.layer || p1.layer == p2.layer && p1.height < p2.height;
        }

        public static bool operator <(NeuronPosition p1, NeuronPosition p2)
        {
            return p1.layer < p2.layer || p1.layer == p2.layer && p1.height > p2.height;
        }

        public static bool operator >=(NeuronPosition p1, NeuronPosition p2)
        {
            return p1.layer >= p2.layer;
        }

        public static bool operator <=(NeuronPosition p1, NeuronPosition p2)
        {
            return p1.layer <= p2.layer;
        }

        public static NeuronPosition operator +(NeuronPosition  p,int i)
        {
            return new NeuronPosition(p.layer, p.height + i);
        }
    }
}
