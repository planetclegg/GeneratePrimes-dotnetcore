using System;
using Xunit;
using PlanetClegg.Utils.Primes;
using System.Linq;
using System.Diagnostics;
using Xunit.Abstractions;

namespace PlanetClegg.Utils.Primes.Tests
{
    public class Optimized64PrimeSieveTests : GenericPrimeSieveTests<Optimized64PrimeSieve>
    {
        public Optimized64PrimeSieveTests(ITestOutputHelper output) : base(output)
        {
           
        }

    }
}
