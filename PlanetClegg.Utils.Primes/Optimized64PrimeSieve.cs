using System;
using System.Collections;
using System.Collections.Generic;

namespace PlanetClegg.Utils.Primes
{
    public class Optimized64PrimeSieve : PrimeSieve
    {
        public Optimized64PrimeSieve() { }

        public override List<int> GeneratePrimesUpTo(int limit)
        {
            if (limit < 2)
                return new List<int>();

            uint ulimit = (uint)limit;
            int ulimitDiv2 = (int)limit / 2;

            //playing around with inlining and alternatives to BitArray
            // var bits = new BitArray(1+(limit/2), true);
            ulong[] bits = new ulong[((limit / 2) + 64) / 64];

            // we're just going to repeat this 60 bit pattern over and over
            // to mark the multiples of 3s and 5s in a more efficient way
            ulong wheel = 0b011010010010110;  // 15 bit seed, odd numbes up to 30
            // repeat 15 bit seed to make 60 bits
            wheel = wheel | wheel << 15 | wheel << 30 | wheel << 45;

            for (int i =0; i < bits.Length; i++)
            {
                // set first 60 bits, then repeat cycle for remaining 4
                bits[i] = (wheel << 60 | wheel);
                // rotate 60-bit wheel right by 4 bits. top 4 out of 64 bits stay 0
                wheel = (wheel >> 4) | ((wheel & 0b1111) << 56);
            }
            bits[0] &= ~0b110UL; // manually mark 3,5, because the wheel doesnt

            var sqrt = (uint)System.Math.Sqrt(ulimit);
            var sqrtDiv2 = (int)sqrt / 2;

            // for factorDiv2 1->3, 2->5, 3->7, etc since even numbers are not tracked
            for (int factorDiv2 = 3; factorDiv2 <= sqrtDiv2; factorDiv2 += 1)
            {
                if ((bits[factorDiv2 >> 6] & (1UL << factorDiv2)) == 0)
                {
                    int factor = 1 + (factorDiv2 * 2);
                    for (int i = (factor * factor)/2; i <= ulimitDiv2; i += factor)
                    {
                        bits[(i >> 6)] |= (1UL << i);
                    }
                }
            }

            int capacity = EstimatePrimeCountUpTo(limit);
            var result = new List<int>(capacity) { 2 };

            for (uint i = 3; i <= ulimit; i += 2)
            {
                int idx = (int)(i / 2);
                if ((bits[idx >> 6] & (1UL << idx)) == 0)
                {
                    result.Add((int)i);
                }
            }
           
            return result;
        }

    }
}
