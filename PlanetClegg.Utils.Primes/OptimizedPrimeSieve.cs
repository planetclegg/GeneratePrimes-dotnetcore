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
            uint[] bits = new uint[((limit / 2) + 32) / 32];

            // we're just going to repeat this 30 bit pattern over and over
            // to mark the multiples of 3s and 5s in a more efficient way
            uint wheel = 0b011010010010110; //15 bit seed
            // repeat 15 bit seed to make 60 bits
            wheel |= wheel << 15;

            for (int i =0; i < bits.Length; i++)
            {
                // set first 30 bits, then repeat cycle for remaining 2
                bits[i] = (wheel << 30 | wheel);
                // rotate wheel right by 2 bits. top 2 bits stay 0
                wheel = (wheel >> 2) | ((wheel & 3) << 28);
            }
            bits[0] &= ~0b110u; // manually mark 3,5, because the wheel doesnt

            var sqrt = (uint)System.Math.Sqrt(ulimit);
            var sqrtDiv2 = (int)sqrt / 2;

            // for factorDiv2 1->3, 2->5, 3->7, etc since even numbers are not tracked
            for (int factorDiv2 = 3; factorDiv2 <= sqrtDiv2; factorDiv2 += 1)
            {
                if ((bits[factorDiv2 >> 5] & (1 << factorDiv2)) == 0)
                {
                    int factor = 1 + (factorDiv2 * 2);
                    for (int i = (factor * factor)/2; i <= ulimitDiv2; i += factor)
                    {
                        bits[(i >> 5)] |= (1u << i);
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
