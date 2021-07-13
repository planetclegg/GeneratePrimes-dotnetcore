using System;
using System.Collections;
using System.Collections.Generic;

namespace PlanetClegg.Utils.Primes
{
    public class OptimizedPrimeSieve : PrimeSieve
    {
        public OptimizedPrimeSieve() { }

        public override List<int> GeneratePrimesUpTo(int limit)
        {
            if (limit < 2)
                return new List<int>();

            uint ulimit = (uint)limit;
            int ulimitDiv2 = (int)limit / 2;

            //playing around with inlining and alternatives to BitArray
            // var bits = new BitArray(1+(limit/2), true);

            int[] bits = new int[((limit / 2) + 32) / 32];
            var sqrt = (uint)System.Math.Sqrt(ulimit);
            var sqrtDiv2 = (int)sqrt / 2;

            for (int factorDiv2 = 1; factorDiv2 <= sqrtDiv2; factorDiv2 += 1)
            {
                
                if ((bits[factorDiv2 >> 5] & (1 << factorDiv2)) == 0)
                {
                    int factor = 1 + (factorDiv2 * 2);
                    for (int i = (factor * factor)/2; i <= ulimitDiv2; i += factor)
                    {
                        bits[(i >> 5)] |= (1 << i);
                    }
                }
            }

            int capacity = EstimatePrimeCountUpTo(limit);
            var result = new List<int>(capacity) { 2 };

            for (uint i = 3; i <= ulimit; i += 2)
            {
                int idx = (int)(i / 2);
                if ((bits[idx >> 5] & (1 << idx)) == 0)
                {
                    result.Add((int)i);
                }
            }

            return result;
        }

    }
}
