public class RealNumber
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

    //Skips GCD calculation
    //Used when we know GCD is 1
    //Side note: It's really funny that you can just put false here and it does the exact same thing lmao
    private RealNumber(long num, long den, bool skipGCD)
    {
        this.numerator = num;

        if(num == 0)
        {
            this.denominator = 1;
            return;
        }

        this.denominator = den;
    }

    public static RealNumber operator -(RealNumber a)
    {
        return new RealNumber(-a.numerator, a.denominator, true);
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
        return new RealNumber(a.numerator * b.numerator, a.denominator * b.denominator);
    }

    public static RealNumber operator /(RealNumber a, RealNumber b)
    {
        return new RealNumber(a.numerator * b.denominator, a.denominator * b.numerator);
    }

    public static bool operator ==(RealNumber a, RealNumber b)
    {
        return a.numerator == b.numerator && a.denominator == b.denominator;
    }

    public static bool operator !=(RealNumber a, RealNumber b)
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


public class UnaryOperator<I, O>
{
    private int precedence {get;}

    //False = left, True = right
    private bool fixture {get;}
    private string symbol {get;}

    Func<I, O> compute;

    public UnaryOperator(int precedence, bool fixture, string symbol, Func<I, O> compute)
    {
        this.precedence = precedence;
        this.fixture = fixture;
        this.symbol = symbol;

        this.compute = compute;
    }

    public O eval(I x)
    {
        return compute(x);
    }
}

public class BinaryOperator<I1, I2, O>
{
    private int precedence {get;}

    private string symbol {get;}

    private Func<I1, I2, O> computeL;

    public BinaryOperator(int precedence, string symbol, Func<I1, I2, O> compute)
    {
        this.precedence = precedence;
        this.symbol = symbol;

        this.computeL = compute;
    }

    public O eval(I1 x, I2 y)
    {
        return computeL(x, y);
    }
}

public class Operators
{
    public static Operators instance = null;

    Dictionary<string, UnaryOperator<RealNumber, RealNumber>> unaryOperators = new Dictionary<string, UnaryOperator<RealNumber, RealNumber>>();
    Dictionary<string, BinaryOperator<RealNumber, RealNumber, RealNumber>> binaryOperators = new Dictionary<string, BinaryOperator<RealNumber, RealNumber, RealNumber>>();

    private Operators()
    {
        registerUnaryOperator("-", false, 3, (a) => -a);

        registerBinaryOperator("+", 1, (a, b) => a + b);
        registerBinaryOperator("-", 1, (a, b) => a - b);
        registerBinaryOperator("*", 2, (a, b) => a * b);
        registerBinaryOperator("/", 2, (a, b) => a / b);
    }

    public static Operators getInstance()
    {
        if(instance == null)
        {
            instance = new Operators();
        }

        return instance;
    }

    public void registerUnaryOperator(string symbol, bool fixture, int precedence, Func<RealNumber, RealNumber> compute)
    {
        unaryOperators.Add(symbol, new UnaryOperator<RealNumber, RealNumber>(precedence, fixture, symbol, compute));
    }

    public void registerBinaryOperator(string symbol, int precedence, Func<RealNumber, RealNumber, RealNumber> compute)
    {
        binaryOperators.Add(symbol, new BinaryOperator<RealNumber, RealNumber, RealNumber>(precedence, symbol, compute));
    }

    public UnaryOperator<RealNumber, RealNumber> getUnaryOperator(string symbol)
    {
        return unaryOperators[symbol];
    }

    public BinaryOperator<RealNumber, RealNumber, RealNumber> getBinaryOperator(string symbol)
    {
        return binaryOperators[symbol];
    }
}