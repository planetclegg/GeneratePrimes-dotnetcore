using System;
using Xunit;
using PlanetClegg.Utils.Primes;
using System.Linq;
using System.Diagnostics;
using Xunit.Abstractions;

namespace PlanetClegg.Utils.Primes.Tests
{
    public class OptimizedPrimeSieveTests
    {
        private ITestOutputHelper _output;

        public OptimizedPrimeSieveTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private int[] _primesBelowThousand =
        {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29,
            31, 37, 41, 43, 47, 53, 59, 61, 67, 71,
            73, 79, 83, 89, 97, 101, 103, 107, 109, 113,
            127, 131, 137, 139, 149, 151, 157, 163, 167, 173,
            179, 181, 191, 193, 197, 199, 211, 223, 227, 229,
            233, 239, 241, 251, 257, 263, 269, 271, 277, 281,
            283, 293, 307, 311, 313, 317, 331, 337, 347, 349,
            353, 359, 367, 373, 379, 383, 389, 397, 401, 409,
            419, 421, 431, 433, 439, 443, 449, 457, 461, 463,
            467, 479, 487, 491, 499, 503, 509, 521, 523, 541,
            547, 557, 563, 569, 571, 577, 587, 593, 599, 601,
            607, 613, 617, 619, 631, 641, 643, 647, 653, 659,
            661, 673, 677, 683, 691, 701, 709, 719, 727, 733,
            739, 743, 751, 757, 761, 769, 773, 787, 797, 809,
            811, 821, 823, 827, 829, 839, 853, 857, 859, 863,
            877, 881, 883, 887, 907, 911, 919, 929, 937, 941,
            947, 953, 967, 971, 977, 983, 991, 997,
        };

        // last 4 primes we can calculate that are 31 bits.
        private int[] _last31BitPrimes = new[] {
                    2147483579,
                    2147483587,
                    2147483629,
                    2147483647, };



        [Fact]
        public void GeneratePrimesUpTo_Thousand()
        {
            var sieve = new OptimizedPrimeSieve();
            var result = sieve.GeneratePrimesUpTo(1000);

            Assert.Equal(_primesBelowThousand, result);
        }

        [Theory]
        [InlineData(-100, 0)]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 2)]
        [InlineData(5, 3)]
        [InlineData(6, 3)]
        [InlineData(7, 4)]
        [InlineData(8, 4)]
        [InlineData(9, 4)]
        [InlineData(10, 4)]
        [InlineData(89, 24)]
        [InlineData(100, 25)]
        [InlineData(127, 31)]  // 127 is a mersenne prime
        [InlineData(1000, 168)]
        [InlineData(10000, 1229)]
        [InlineData(65536, 6542)]
        [InlineData(100000, 9592)]
        [InlineData(1000000, 78498)]
        [InlineData(10_000_000, 664579)]
        [InlineData(100_000_000, 5761455)]
        //[InlineData(2147483646, 105097564)] 
        [InlineData(2147483647, 105097565)] //special case: int.MaxValue & prime
        public void GeneratePrimesUpTo(int limit, int expectedPrimeCount)
        {
            var startTime = DateTime.Now;

            var sieve = new OptimizedPrimeSieve();
            var result = sieve.GeneratePrimesUpTo(limit);

            var endTime = DateTime.Now;

            if (result.Count < 400)
                Debug.WriteLine(string.Join(",", result));

            // check that # of primes returned is correct.
            Assert.Equal(expectedPrimeCount, result.Count);

            // check beginning of result for primes <1000 being correct.
            var smallest = System.Math.Min(_primesBelowThousand.Length, result.Count);
            Assert.Equal(result.Take(smallest), _primesBelowThousand.Take(smallest));

            // extreme test. The sieve should be able to handle up to
            // int.MaxValue (2147483647), although that is memory
            // and cpu intensive (maybe 15 seconds on a fast box).
            // it will also build a list of primes thats 105,097,565 long
            // Mostly just have this as a special case test for correctness, 
            if (limit == int.MaxValue)
            {
                Assert.Equal(_last31BitPrimes, result.TakeLast(_last31BitPrimes.Length));
            }

            var millis = (endTime - startTime).TotalMilliseconds;

            //_output.WriteLine($"Sieve up to {limit} took {millis}ms");
            //Console.WriteLine($"Sieve up to {limit} took {millis}ms");
        }

    }

}
