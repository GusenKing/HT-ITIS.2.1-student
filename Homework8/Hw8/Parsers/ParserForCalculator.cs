using Hw8.Calculator;

namespace Hw8.Parsers;

public static class ParserForCalculator
{
    public static (double, Operation, double) ParseParams(string val1, string operation, string val2)
    {
        if (!(Double.TryParse(val1, out double parsedVal1) | Double.TryParse(val2, out double parsedVal2)))
        {
            throw new ArgumentException(Messages.InvalidNumberMessage);
        }

        return (parsedVal1, ParseOperation(operation), parsedVal2);
    }

    private static Operation ParseOperation(string operation) =>
    operation switch
        {
            "Operation.Plus" => Operation.Plus,
            "Operation.Minus" => Operation.Minus,
            "Operation.Multiply" => Operation.Multiply,
            "Operation.Divide" => Operation.Divide,
            _ => throw new ArgumentException(Messages.InvalidOperationMessage),
        };
}