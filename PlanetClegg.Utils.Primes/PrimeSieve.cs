using System;
using System.Collections;
using System.Collections.Generic;

namespace PlanetClegg.Utils.Primes
{
    // a simple prime sieve, optimized just enough to be quasi useful
    // stores the seive in bits, and ignores even numbers >2 to save space/time
    public class PrimeSieve : IPrimeGenerator
    {
        public PrimeSieve() { }

        public virtual List<int> GeneratePrimesUpTo(int limit)
        {
            if (limit < 2)
                return new List<int>();

            uint ulimit = (uint)limit;

            //optimize: play around with inlining and alternatives to BitArray
            var bits = new BitArray(1+(limit/2), true);
            var sqrt = (uint)System.Math.Sqrt(ulimit);
            
            for (uint factor = 3; factor <= sqrt; factor += 2)
            {
                if (bits.Get((int)(factor / 2)))
                {
                    for (uint i = factor * factor; i <= ulimit; i += factor * 2)
                    {
                        bits.Set((int)(i / 2), false);
                    }
                }
            }

            int capacity = EstimatePrimeCountUpTo(limit);
            var result = new List<int>(capacity) { 2 };
            
            for (uint i = 3 ; i <= ulimit; i += 2)
            {
                if (bits.Get((int)(i / 2)))
                {
                    result.Add((int)i);
                }
            }

            return result;
        }

        
        // used for List capacity estimation.  should be same or slightly
        // above the actual # primes up to n
        public virtual int EstimatePrimeCountUpTo(int n)
        {
            // note: the x/ln(x) estimate isn't great for small numbers,
            // so we'll fudge the smallest numbers, and estimates don't
            // really start to converge well until n > 200, where the
            // estimate is less than 10% off and gets smaller as n increases.
            if (n < 2)  return 0;
            if (n < 7)  return (n + 1) / 2;
            if (n < 11) return 4;

            // see: https://primes.utm.edu/howmany.html , but watch out for typos.
            var b = 1.04423;
            return  (int)(b * ((double)n / (System.Math.Log(n) - 1 ) ));
        }
    }
}
