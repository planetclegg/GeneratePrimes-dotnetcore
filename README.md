## GeneratePrimes-dotnetcore

Just playing around with .NET 5 on a M1 Mac...

The library (PlanetClegg.Utils.Primes) generates prime numbers with a sieve.  The
code is reasonably optimized for what it is:  Uses BitArray, ignores even 
numbers, skips over obvious numbers that have already been marked off, etc. 
On an M1 it can generate all primes up to 2^31-1 (which is itself a Mersenne prime)
in about ~~19 seconds~~(derp) 10 seconds in a release build, 
with no real attempt to optimize it very much beyond the basic 
algorithm.  Any significant improvements would best be done with a better 
algorithm.

There are xUnit tests (Utils.Tests) and a console runner app.  

Addendum: Out of curiosity, I've done some micro-optimizations to the inner loop 
in the class OptimizedPrimeSieve, primarily just eliminating the function call 
overhead to from using BitArray.  Resulted in a 10% speedup, but probably not worth 
the loss of readability.  

### Various console test runs, compiled as 'Release' build, run in VS for Mac.
#### Mac Mini M1 (Macmini9,1)
```
CPU info: Apple M1
Testing 2 prime number generator, with 7 tests.

Using generator 'PrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took    39.5 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.6 ms.
 - Calc primes up to    1000000, (    78498 expected)... took     3.0 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    31.5 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   320.0 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  4337.8 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took  9916.9 ms.

Using generator 'OptimizedPrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took     1.0 ms.
 - Calc primes up to     100000, (     9592 expected)... took     1.0 ms.
 - Calc primes up to    1000000, (    78498 expected)... took     2.9 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    27.2 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   275.6 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  3829.4 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took  8773.9 ms.
```

#### MBP 2015
```
CPU info: Intel(R) Core(TM) i7-4870HQ CPU @ 2.50GHz
Testing 2 prime number generator, with 7 tests.

Using generator 'PrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took    16.7 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.3 ms.
 - Calc primes up to    1000000, (    78498 expected)... took     3.1 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    31.1 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   335.9 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  3915.4 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took 10090.4 ms.

Using generator 'OptimizedPrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took     1.0 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.3 ms.
 - Calc primes up to    1000000, (    78498 expected)... took    50.7 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    26.2 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   286.4 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  3487.4 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took  9085.8 ms.
```
#### MBP 2013 (MacBookPro10,1)
```
CPU info: Intel(R) Core(TM) i7-3840QM CPU @ 2.80GHz
Testing 2 prime number generator, with 7 tests.

Using generator 'PrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took    18.9 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.4 ms.
 - Calc primes up to    1000000, (    78498 expected)... took     3.1 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    33.2 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   380.9 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  5751.9 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took 12854.7 ms.

Using generator 'OptimizedPrimeSieve':
 - Calc primes up to      10000, (     1229 expected)... took     1.1 ms.
 - Calc primes up to     100000, (     9592 expected)... took     0.3 ms.
 - Calc primes up to    1000000, (    78498 expected)... took    62.8 ms.
 - Calc primes up to   10000000, (   664579 expected)... took    28.8 ms.
 - Calc primes up to  100000000, (  5761455 expected)... took   335.2 ms.
 - Calc primes up to 1000000000, ( 50847534 expected)... took  5030.7 ms.
 - Calc primes up to 2147483647, (105097565 expected)... took 11390.5 ms.
```