
RealNumber a = new RealNumber(1, 2);
RealNumber b = new RealNumber(3, 4);

Operators ops = Operators.getInstance();

Console.WriteLine(ops.getOperator("+").calculate((1, 2)));