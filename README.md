## GeneratePrimes-dotnetcore

Just playing around with .NET 5 on a M1 Mac...

The library (PlanetClegg.Utils.Primes) generates prime numbers with a sieve.  
The code is reasonably optimized for what it is:  Uses BitArray, ignores even 
numbers, skips over obvious numbers that have already been marked off etc. 
On an M1 it can generate all primes up to 2^31-1 (which is itself a mersenne prime)
in about ~~19 seconds~~(derp) 10 seconds in a release build, 
with no real attempt to optimize it very much beyond the basic 
algorithm.  Any significant improvements would probably best be done with a better 
algorithm.

Will create a branch for some ugly micro-optimizations

There are xUnit tests (Utils.Tests) and a console runner.  

