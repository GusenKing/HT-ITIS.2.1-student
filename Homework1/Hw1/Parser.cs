namespace Hw1;

public static class Parser
{
    public static void ParseCalcArguments(string[] args, 
        out double val1, 
        out CalculatorOperation operation, 
        out double val2)
    {
        if (!IsArgLengthSupported(args))
            throw new ArgumentException("Wrong amount of arguments");
        
        if (!Double.TryParse(args[0], out val1))
            throw new ArgumentException("Values must be double");
        operation = ParseOperation(args[1]);
        if (!Double.TryParse(args[2], out val2))
            throw new ArgumentException("Values must be double");

        switch (args[1])
        {
            case "+":
                operation = CalculatorOperation.Plus;
                break;
            case "-":
                operation = CalculatorOperation.Minus;
                break;
            case "*":
                operation = CalculatorOperation.Multiply;
                break;
            case "/":
                operation = CalculatorOperation.Divide;
                break;
        }
    }

    private static bool IsArgLengthSupported(string[] args) => args.Length == 3;

    private static CalculatorOperation ParseOperation(string arg) =>
        arg switch
        {
            "+" => CalculatorOperation.Plus,
            "-" => CalculatorOperation.Minus,
            "*" => CalculatorOperation.Multiply,
            "/" => CalculatorOperation.Divide,
            _ => throw new InvalidOperationException("Wrong operation"),
        };
}