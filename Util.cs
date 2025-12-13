public class Util
{
    public static long gcd(long a, long b)
    {
        if(a < 0 || b < 0 || (a == 0 && b == 0))
        {
            throw new ArgumentException("GCD is only defined for non-negative integers.");
        }

        if(b > a)
        {
            long temp = a;
            a = b;
            b = temp;
        }

        while (b != 1 && b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        
        if(b == 0)
        {
            return a;
        }

        return 1;
    }
}