
RealNumber a = new RealNumber(1, 3);
RealNumber b = new RealNumber(3, 17);

Operators ops = Operators.getInstance();

Console.WriteLine(ops.getUnaryOperator("-").eval(a));
Console.WriteLine(ops.getBinaryOperator("+").eval(a, b));
Console.WriteLine(ops.getBinaryOperator("-").eval(a, b));
Console.WriteLine(ops.getBinaryOperator("*").eval(a, b));
Console.WriteLine(ops.getBinaryOperator("/").eval(a, b));
