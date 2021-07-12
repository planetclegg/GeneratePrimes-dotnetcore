using System;
using System.Linq;

namespace PlanetClegg.Utils.Primes.ConsoleBenchmark
{
    class Benchmark
    {

        // simple benchmark, nothing fancy, just exercise
        // single core performance along with memory/GC
        static void Main(string[] args)
        {

            var runData = new[] {
                    (1000, 168),
                    (10000, 1229),
                    (65536, 6542),
                    (100000, 9592),
                    (1000000, 78498),
                    (10_000_000, 664579),
                    (100_000_000, 5761455),
                    //(2147483647, 105097565)
            };

            //var largest = runData.Max(t => t.Item1);
            Console.WriteLine($"Prime sieve run with {runData.Length} runs:");
            foreach (var (limit, expected) in runData )
            {
                Console.WriteLine($"   -- first {limit} primes, expecting {expected} primes");
            }


            Console.WriteLine("Starting....");
            var startTime = DateTime.Now;

            foreach (var (limit, expectedCount) in runData)
            {
                var sieve = new PrimeSieve();
                var result = sieve.GeneratePrimesUpTo(limit);
                if (result.Count != expectedCount)
                    throw new ArgumentOutOfRangeException(
                        $"Failed to verify for {limit}/{expectedCount} != {result.Count}, check tests");

            }
            var endTime = DateTime.Now;

            Console.WriteLine("Finished.");
            var millis = (endTime - startTime).TotalMilliseconds;

            Console.WriteLine($"Sieve operations took {millis}ms");



        }
    }
}
