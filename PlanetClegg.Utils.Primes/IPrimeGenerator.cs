using System;
using System.Collections.Generic;

namespace PlanetClegg.Utils.Primes
{
    public interface IPrimeGenerator
    {
        public int EstimatePrimeCountUpTo(int n);
        public List<int> GeneratePrimesUpTo(int limit);
    }
}
