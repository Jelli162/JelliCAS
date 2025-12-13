public class Operator<I, O>
{
    private int priority;
    private Func<I, O> compute;

    public Operator(int priority, Func<I, O> compute)
    {
        this.priority = priority;
        this.compute = compute;
    }

    public O calculate(I input)
    {
        return compute(input);
    }

    public int getPriority()
    {
        return priority;
    }
}

/*
    Fixture = false: prefix
    Fixture = true: postfix
*/
public class UnaryOperator<I, O> : Operator<I, O>
{
    private bool fixture;


    public UnaryOperator(int priority, bool fixture, Func<I, O> compute) : base(priority, compute)
    {
        this.fixture = fixture;
    }

    public bool getFixture()
    {
        return fixture;
    }
}

public class BinaryOperator<I1, I2, O> : Operator<(I1, I2), O>
{
    public BinaryOperator(int priority, Func<(I1, I2), O> compute) : base(priority, compute)
    {    }
}

public class Number
{}

public class RealNumber : Number
{
    private long numerator {get;}

    private long denominator {get;}

    public RealNumber(double value)
    {
        if(value % 1 == 0)
        {
            this.numerator = (long) value;
            this.denominator = 1;

            return;
        }

        string str = value.ToString();
        int decimalPlaces = str.Length - str.IndexOf('.') - 1;

        long den = (long) Math.Pow(10, decimalPlaces);
        long num = (long) (value * den);
        
        long gcd = Util.gcd(Math.Abs(num), Math.Abs(den));

        this.numerator = num / gcd;
        this.denominator = den / gcd;
    }

    public RealNumber(long num, long den)
    {
        if(den == 0)
        {
            throw new ArgumentException("Denominator cannot be zero.");
        }
        
        if(num == 0)
        {
            this.numerator = 0;
            this.denominator = 1;
            return;
        }

        if(den < 0)
        {
            this.numerator = -num;
            this.denominator = -den;
        }

        else
        {
            this.numerator = num;
            this.denominator = den;
        }

        long gcd = Util.gcd(Math.Abs(num), Math.Abs(den));

        this.numerator /= gcd;
        this.denominator /= gcd;
    }

    public static RealNumber operator +(RealNumber a, RealNumber b)
    {
        long commonDenominator = a.denominator * b.denominator;
        long newNumerator = a.numerator * b.denominator + b.numerator * a.denominator;

        return new RealNumber(newNumerator, commonDenominator);
    }

    public static RealNumber operator -(RealNumber a, RealNumber b)
    {
        long commonDenominator = a.denominator * b.denominator;
        long newNumerator = a.numerator * b.denominator - b.numerator * a.denominator;
        
        return new RealNumber(newNumerator, commonDenominator);
    }

    public static RealNumber operator *(RealNumber a, RealNumber b)
    {
        long newNumerator = a.numerator * b.numerator;
        long newDenominator = a.denominator * b.denominator;

        return new RealNumber(newNumerator, newDenominator);
    }

    public static RealNumber operator /(RealNumber a, RealNumber b)
    {
        long newNumerator = a.numerator * b.denominator;
        long newDenominator = a.denominator * b.numerator;

        return new RealNumber(newNumerator, newDenominator);
    }

    public static bool operator ==(RealNumber a, RealNumber b)
    {
        return a.numerator == b.numerator && a.denominator == b.denominator;
    }

    public static bool operator !=(RealNumber a, RealNumber b)
    {
        return !(a == b);
    }

    public static bool operator ==(RealNumber a, long b)
    {
        return a.denominator == 1 && a.numerator == b;
    }

    public static bool operator !=(RealNumber a, long b)
    {
        return !(a == b);
    }

    public static bool operator ==(long a, RealNumber b)
    {
        return b == a;
    }

    public static bool operator !=(long a, RealNumber b)
    {
        return !(a == b);
    }

    public static bool operator ==(RealNumber a, double b)
    {
        return a == new RealNumber(b);
    }

    public static bool operator !=(RealNumber a, double b)
    {
        return !(a == b);
    }

    public static bool operator ==(double a, RealNumber b)
    {
        return b == a;
    }

    public static bool operator !=(double a, RealNumber b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        if (this.denominator == 1)
        {
            return numerator.ToString();
        }

        return $"{this.numerator}/{this.denominator}";
    }

    public override bool Equals(object obj)
    {
        if (obj is RealNumber other)
        {
            return this == other;
        }

        return false;
    }
}

public class Operators
{
    private static readonly Operators instance = new Operators();

    private Dictionary<string, Operator<object, object>> registry = new Dictionary<string, Operator<object, object>>();

    private Operators()
    {
        // Register default operators
        registerBinaryOperator<RealNumber, RealNumber, RealNumber>("+", 1, (x) => x.Item1 + x.Item2);
        registerBinaryOperator<RealNumber, RealNumber, RealNumber>("-", 1, (x) => x.Item1 - x.Item2);
        registerBinaryOperator<RealNumber, RealNumber, RealNumber>("*", 2, (x) => x.Item1 * x.Item2);
        registerBinaryOperator<RealNumber, RealNumber, RealNumber>("/", 2, (x) => x.Item1 / x.Item2);
    }

    public static Operators getInstance()
    {
        return instance;
    }

    public void registerUnaryOperator<I, O>(string key, int priority, bool fixture, Func<I, O> compute)
    {
        registry.Add(key, new UnaryOperator<I, O>(priority, fixture, compute) as Operator<object, object>);
    }

    public void registerBinaryOperator<I1, I2, O>(string key, int priority, Func<(I1, I2), O> compute)
    {
        registry.Add(key, new BinaryOperator<I1, I2, O>(priority, compute) as Operator<object, object>);
    }

    public Operator<object, object> getOperator(string key)
    {
        return registry[key];
    }
}