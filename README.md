## GeneratePrimes-dotnetcore

Just playing around with .NET 5 on a M1 Mac...

The library (PlanetClegg.Utils.Primes) generates prime numbers with a sieve.   The 
code is reasonably optimized for what it is:  Uses BitArray, ignores even 
numbers, skips over obvious numbers that have already been marked off, etc. 
On an M1 it can generate all primes up to 2^31-1 (which is itself a Mersenne prime)
in about 10 seconds in a release build, with no real attempt to optimize it very 
much beyond the basic algorithm.  Any significant improvements would 
best be done with a better algorithm.

There are xUnit tests (Utils.Tests) and a console runner app.  

Addendum: Out of curiosity, I've done several micro-optimizations over the original 
in the classes OptimizedPrimeSieve / Optimized64PrimeSieve: eliminating the function call 
overhead from using BitArray, and pre-setting multiples of 3 & 5 with a rotating bit wheel.
Resulted in a 30% speedup, but probably not worth the loss of readability.  Strangely, 
the 'Optimized64' version runs a little faster than the 'Optimized' on Intel (as I expected), 
but a little slower on an M1. The difference between these two versions is merely
that one does bit-twiddling on uints, and the other on ulongs.

The M1 beats my Intel boxes either way, even though it is running in rosetta2 (!!!)

NOTE: I'm seeing evidence that CLR JITing and/or the OS scheduler is skewing these
tests a bit.  Needs a better benchmark runner to get more consistent results.

### Various console test runs, compiled as 'Release' build, run in VS for Mac.
#### Mac Mini M1 (Macmini9,1)
```
CPU info: Apple M1
Testing 3 prime number generators, with 7 tests.

Using generator 'PrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took     2.7 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.5 ms.
 - Calc primes up to    1000000, (    78498 expected)... took    27.1 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    30.5 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   314.1 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  4320.3 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took  9872.1 ms.
 
Using generator 'OptimizedPrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took    41.0 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.4 ms.
 - Calc primes up to    1000000, (    78498 expected)... took     1.9 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    17.8 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   180.4 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  2948.7 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took  6824.6 ms.

Using generator 'Optimized64PrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took     1.0 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.7 ms.
 - Calc primes up to    1000000, (    78498 expected)... took     3.7 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    21.6 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   213.1 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  3327.6 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took  7643.6 ms.
```

#### MBP 2015 (MacBookPro11,5)
```
CPU info: Intel(R) Core(TM) i7-4870HQ CPU @ 2.50GHz
Testing 4 prime number generator, with 7 tests.

Using generator 'PrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took    17.6 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.4 ms.
 - Calc primes up to    1000000, (    78498 expected)... took     3.4 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    31.6 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   343.1 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  3968.7 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took 10173.5 ms.

Using generator 'OptimizedPrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took     1.1 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.2 ms.
 - Calc primes up to    1000000, (    78498 expected)... took    49.4 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    23.0 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   255.4 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  3233.0 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took  8531.2 ms.

Using generator 'Optimized64PrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took     1.0 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.2 ms.
 - Calc primes up to    1000000, (    78498 expected)... took    50.4 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    21.8 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   234.0 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  3039.1 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took  8102.3 ms.
Finished.
```
#### MBP 2013 (MacBookPro10,1)
```
CPU info: Intel(R) Core(TM) i7-3840QM CPU @ 2.80GHz
Testing 3 prime number generators, with 7 tests.

Using generator 'PrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took     0.0 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.3 ms.
 - Calc primes up to    1000000, (    78498 expected)... took     4.4 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    35.1 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   311.3 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  5105.6 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took 11579.2 ms.

Using generator 'OptimizedPrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took     1.7 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.3 ms.
 - Calc primes up to    1000000, (    78498 expected)... took     2.3 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    22.5 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   248.3 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  4258.3 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took  9740.0 ms.

Using generator 'Optimized64PrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took     1.5 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.2 ms.
 - Calc primes up to    1000000, (    78498 expected)... took    26.8 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    26.1 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   242.6 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  4056.5 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took  9363.3 ms.
```
