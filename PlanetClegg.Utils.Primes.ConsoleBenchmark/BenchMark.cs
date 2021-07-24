using System;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PlanetClegg.Utils.Primes.ConsoleBenchmark
{
    class Benchmark
    {
        // simple benchmark, nothing fancy, just exercise
        // single core performance along with memory/GC
        static void Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var hardwareInfo = GetMachineInfo();
                Console.WriteLine($"CPU info: {hardwareInfo}");
            }

            var runData = new[] {
                    //(1000, 168),
                    (10000, 1229),
                    //(65536, 6542),
                    (100000,        9592),
                    (1000000,       78498),
                    (10_000_000,    664579),
                    (100_000_000,   5761455),
                    (1_000_000_000, 50847534),
                    (2147483647,    105097565)
            };


            IPrimeGenerator[] generators = new IPrimeGenerator[] {
                new PrimeSieve(),
                new OptimizedPrimeSieve(),
                new Optimized64PrimeSieve(),
            };

            Console.WriteLine($"Testing {generators.Length} prime number generators," +
                              $" with {runData.Length} tests.");
                
            foreach (var generator in generators)
            {
                var className = generator.GetType().Name;
                Console.WriteLine($"\nUsing generator '{className}':");

                foreach (var (limit, expected) in runData)
                {
                    Console.Write($" - Calc primes up to {limit,10}, ({expected,9} expected)...");

                    var startTime = DateTime.Now;
                    var sieve = generator;
                    var result = sieve.GeneratePrimesUpTo(limit);
                    if (result.Count != expected)
                        throw new ArgumentOutOfRangeException(
                            $"Failed to verify prime count for {limit}, {expected} != {result.Count}");
                    var endTime = DateTime.Now;
                    var millis = (endTime - startTime).TotalMilliseconds;

                    Console.WriteLine($" took {millis,7:0.0} ms.");

                }
            }
            Console.WriteLine("\nFinished.");
        }

        [DllImport("libc")]
        static extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, byte[] output,
                                        ref Int64 oldLen, IntPtr newp, uint newLen);

        // hackish way to get cpu info.  no worky on windows.
        public static string GetMachineInfo()
        {
            string property = "machdep.cpu.brand_string";
            var result = "unknown/error";
            Int64 len = 0;

            try
            {
                if (0 == sysctlbyname(property, null, ref len, IntPtr.Zero, 0))
                {
                    var bytes = new byte[len];
                    if (0 == sysctlbyname(property, bytes, ref len, IntPtr.Zero, 0))
                    {
                        result = Encoding.ASCII.GetString(bytes);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                result += " " + e.Message;
                return result;
            }
        }


    }
}
