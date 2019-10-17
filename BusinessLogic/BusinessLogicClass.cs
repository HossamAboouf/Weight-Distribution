using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class InCap
    {
        public string ID;
        public decimal Participation;
    }

    public class OutCap
    {
        public string ID;
        public int Quantity;

        public override bool Equals(object obj)
        {
            var other = obj as OutCap;
            return (other != null)
                    && (this.ID == other.ID)
                    && (this.Quantity == other.Quantity);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode() ^ Quantity.GetHashCode();
        }

        public override string ToString()
        {
            return $"ID: {this.ID}, Quantity: {this.Quantity}";
        }
    }

    public static class CapOperations
    {
        /*
         * According to requirements we have to use standard mathematical equation,
         * Which can be applied on any number of fractions and option value related to each fraction.
         * 
         * This Equation is:
         * 
         * Xn = (Fn * Val) / |(1 - Sum_F)|
         * 
         * Where: 
         *  Xn => Value related to fraction.
         *  Fn => Fration represent weight of Xn in total value.
         *  Val => Value that we start by which.
         *  Sum_F => Summation of all fractions. 
         *  |(1 - Sum_F)| => Absolute value of (1 - Sum_F).
         *  
         * Note:
         *  According to diviasion  we need to Round it and convert it from double to int.
         */

        public static List<OutCap> NormalDistribution(int TotalQty, List<InCap> inCaps)
        {
            List<OutCap> outCaps = new List<OutCap>();
            decimal Sum_F = 0;

            foreach (var inCapItem in inCaps)
                Sum_F += inCapItem.Participation;

            foreach (var inCapItem in inCaps)
                outCaps.Add(new OutCap() { ID = inCapItem.ID, Quantity = (int)Math.Abs(Math.Round((inCapItem.Participation * TotalQty) / (1 - Sum_F))) });

            return outCaps;
        }
    }
}
