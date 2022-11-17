using System;
using static System.Console;

namespace Bme121
{
    static class Program
    {
        // -----------------------------------------------------------------------------------------
        // Compute 'nBase' raised to the power 'nExponent' modulo 'nModulo' using
        // the 'exponentiation by squaring' algorithm, basically selecting between
        // 'b^e=b*b^(e-1)' when 'e' is odd and 'b^e=(b^2)^(e/2)' when 'e' is even
        // to accumulate the 'b^e' product while reducing 'e' as fast as possible.
        // Calculations are done using the 'long' type to avoid overflow.
        // Correct only if 'nExponent' is nonnegative and 'nModulo' is 2 or larger.
        
        static int ModPow( long nBase, int nExponent, int nModulo )
        {
            WriteLine( $"--> ModPow( ) called for {nBase}^{nExponent} mod {nModulo}" );
            
             long accumulatedProduct = 1;
             // long adjustedBase1= 1
             
             // wa2 start here
            
            long adjustedBase = nBase;
            long e = nExponent;
            
             while( e > 0 )
                    {
						if( e % 2 == 0 ) // even
						{
							nBase = ( nBase * nBase ) % nModulo;
							e = e / 2;
						}
						else // odd
						{
							accumulatedProduct = ( accumulatedProduct * nBase ) % nModulo;
							e = e - 1; 
						}
					}
            
            WriteLine( $"<-- ModPow( ) returning {accumulatedProduct}" );
            return ( int ) accumulatedProduct;
        }
        
        // -----------------------------------------------------------------------------------------
        // Determine whether an integer is prime by seeing if it is simultaneously 
        // a strong probable prime base 2, 7, and 61. This works for positive integers
        // less than 4,759,123,141 so works for any number storable in the 'int' type.
        
        static bool IsPrime( int n )
        {
            WriteLine( $"--> IsPrime( ) called for {n}" );
            
            bool isPrime; // holds the answer for eventual display
            
            // Initial tests handle 'n < 2' and rule out 2, 7, and 61 as factors of 'n'.
            // The latter is because the strong-probable-prime tests require the base
            // and 'n' to be relatively prime (no common factors other than one).
            
            if( n < 2 ) // not prime by definition
            {
                isPrime = false;
                WriteLine( "Number is less than 2" );
            }
            else if( n == 2 || n == 7 || n == 61 ) // known primes
            {
                isPrime = true;
                WriteLine( "Number is 2, 7, or 61" );
            }
            else if( n % 2 == 0 || n % 7 == 0 || n % 61 == 0 ) // ensure bases are coprime to n
            {
                isPrime = false;
                WriteLine( "Number has 2, 7, or 61 as a factor" );
            }
            else // do the strong-probable-prime tests
            {
                // Find the 'reduced' representation.
                // The strong-probable-prime tests use 's' and 't' gotten from
                // representing 'n-1' as '2^s*t' where 't' is odd. Here, 's' is just
                // how many factors of 2 can be removed from 'n-1' and 't' is what's left.
                // We start 't' at 'n-1' and evenly divide by 2 as many times as possible.
                
                int t = n - 1;
                int s = 0;
                while( t % 2 == 0 )
                {
                    t = t / 2;
                    s = s + 1;
                }
                WriteLine( $"Reduced form is {n}-1 = 2^{s} * {t}" );
                
                // Loop over testing 'n' for strong probable primality (SPP) base 2, 7, and 61.
                
                int[ ] basesTested = { 2, 7, 61 };
                
                isPrime = true; // the loop will change this if an SPP test fails in any base
                
                foreach( int b in basesTested )
                {
                    WriteLine( $"Test strong probable primality base {b}" );
                    
                    // Number 'n' is SPP base 'b' if 'b^t mod n' is 1 or 'n-1' (first test)
                    // or if successively squaring the first result (also mod 'n')
                    // up to 's-1' times gives a result of 'n-1' (second tests).
                    
                    bool passedSppTest = false; // will change if pass the first or second tests
                    int testValue; // holds 'b^t mod n' or its successive squares mod 'n'
                    
                    testValue = ModPow( nBase: b, nExponent: t, nModulo: n );
                    
                    if( testValue == 1 || testValue == n - 1 ) passedSppTest = true; // first test
                    else // second tests
                    {
                        int r = 1;
                        while( r < s && passedSppTest == false )
                        {
                            testValue = ModPow( nBase: testValue, nExponent: 2, nModulo: n );
                            
                            if( testValue == n - 1 ) passedSppTest = true;
                            
                            r = r + 1;
                        }
                    }
                    
                    // If 'n' is not SPP for this base then it can't be prime 
                    // and we can stop checking other bases.
                    
                    if( passedSppTest == false )
                    {
                        isPrime = false;
                        break;
                    }
                }
            }
            
            WriteLine( $"<-- IsPrime( ) returning {isPrime}" );
            
            return isPrime;
        }
        
        // -----------------------------------------------------------------------------------------
        // Test a few numbers for their primality.
        
        static void Main( )
        {
            WriteLine( "--> Main( ) called" );
            
            // The specific numbers are picked to exercise the various tests in 'IsPrime( )'.
            
            //int[ ] numbersTested = { 1, 7, 49, 53, 131, 145, 457, 2_147_483_647 }; // long test
            int[ ] numbersTested = { 457 }; // short test
            
            foreach( int n in numbersTested )
            {
                WriteLine( );
                WriteLine( $"Testing primality of {n}" );
                
                if( IsPrime( n ) ) WriteLine( $"{n} is prime"     );
                else               WriteLine( $"{n} is not prime" );
            } 
            WriteLine( );
            
            WriteLine( "<-- Main( ) returning" );
        }
    }
}


