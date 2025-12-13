public class Number
{
    private long numerator {get;}

    private long denominator {get;}

    public Number(double value)
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

    public Number(long num, long den)
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

    //Skips GCD calculation
    //Used when we know GCD is 1
    //Side note: It's really funny that you can just put false here and it does the exact same thing lmao
    private Number(long num, long den, bool skipGCD)
    {
        this.numerator = num;

        if(num == 0)
        {
            this.denominator = 1;
            return;
        }

        this.denominator = den;
    }

    public static Number operator -(Number a)
    {
        return new Number(-a.numerator, a.denominator, true);
    }

    public static Number operator +(Number a, Number b)
    {
        long commonDenominator = a.denominator * b.denominator;
        long newNumerator = a.numerator * b.denominator + b.numerator * a.denominator;

        return new Number(newNumerator, commonDenominator);
    }

    public static Number operator -(Number a, Number b)
    {
        long commonDenominator = a.denominator * b.denominator;
        long newNumerator = a.numerator * b.denominator - b.numerator * a.denominator;
        
        return new Number(newNumerator, commonDenominator);
    }

    public static Number operator *(Number a, Number b)
    {
        return new Number(a.numerator * b.numerator, a.denominator * b.denominator);
    }

    public static Number operator /(Number a, Number b)
    {
        return new Number(a.numerator * b.denominator, a.denominator * b.numerator);
    }

    public static bool operator ==(Number a, Number b)
    {
        return a.numerator == b.numerator && a.denominator == b.denominator;
    }

    public static bool operator !=(Number a, Number b)
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
        if (obj is Number other)
        {
            return this == other;
        }

        return false;
    }
}


public class UnaryOperator
{
    private int precedence {get;}

    //False = left, True = right
    private bool fixture {get;}

    private Func<Number, Number> compute;

    public UnaryOperator(int precedence, bool fixture, Func<Number, Number> compute)
    {
        this.precedence = precedence;
        this.fixture = fixture;

        this.compute = compute;
    }

    public Number eval(Number x)
    {
        return compute(x);
    }
}

public class BinaryOperator
{
    private int precedence {get;}

    private Func<Number, Number, Number> compute;

    public BinaryOperator(int precedence, Func<Number, Number, Number> compute)
    {
        this.precedence = precedence;

        this.compute = compute;
    }

    public Number eval(Number x, Number y)
    {
        return compute(x, y);
    }
}

public class Operators
{
    private static Operators instance;

    private Dictionary<string, UnaryOperator> unaryOperators;
    private Dictionary<string, BinaryOperator> binaryOperators;

    private Operators()
    {
        unaryOperators = new Dictionary<string, UnaryOperator>
        {
            { "-", new UnaryOperator(3, true, (Number x) => { return -x; }) }
        };

        binaryOperators = new Dictionary<string, BinaryOperator>
        {
            { "+", new BinaryOperator(1, (Number x, Number y) => { return x + y; }) },
            { "-", new BinaryOperator(1, (Number x, Number y) => { return x - y; }) },
            { "*", new BinaryOperator(2, (Number x, Number y) => { return x * y; }) },
            { "/", new BinaryOperator(2, (Number x, Number y) => { return x / y; }) }
        };
    }

    public static Operators getInstance()
    {
        if(instance == null)
        {
            instance = new Operators();
        }

        return instance;
    }

    public void addUnaryOperator(string symbol, int precedence, bool fixture, Func<Number, Number> compute)
    {
        UnaryOperator op = new UnaryOperator(precedence, fixture, compute);
        unaryOperators[symbol] = op;
    }

    public void addBinaryOperator(string symbol, int precedence, Func<Number, Number, Number> compute)
    {
        BinaryOperator op = new BinaryOperator(precedence, compute);
        binaryOperators[symbol] = op;
    }

    public UnaryOperator getUnaryOperator(string symbol)
    {
        try
        {
            return unaryOperators[symbol];
        }

        catch(KeyNotFoundException)
        {
            throw new KeyNotFoundException($"Unary operator '{symbol}' does not exist!");
        }
    }

    public BinaryOperator getBinaryOperator(string symbol)
    {
        try
        {
            return binaryOperators[symbol];
        }

        catch(KeyNotFoundException)
        {
            throw new KeyNotFoundException($"Binary operator '{symbol}' does not exist!");
        }
    }
}