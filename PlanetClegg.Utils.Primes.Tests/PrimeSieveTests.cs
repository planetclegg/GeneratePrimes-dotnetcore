using System;
using Xunit;
using PlanetClegg.Utils.Primes;
using System.Linq;
using System.Diagnostics;
using Xunit.Abstractions;

namespace PlanetClegg.Utils.Primes.Tests
{
    public class PrimeSieveTests : GenericPrimeSieveTests<PrimeSieve>
    {
        public PrimeSieveTests(ITestOutputHelper output) : base(output)
        {

        }
    }
}
