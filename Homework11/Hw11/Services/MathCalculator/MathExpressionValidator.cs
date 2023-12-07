using Hw11.ErrorMessages;
using Hw11.Regex;

namespace Hw11.Services.MathCalculator;

public class MathExpressionValidator
{
    private static readonly string[] OperationsList = { "+", "-", "*", "/" };

    public void Validate(string? expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new ArgumentException(MathErrorMessager.EmptyString);

        var splitExpression = RegexExpressions.SplitDelimiter.Split(expression)
            .Select(x => x.Replace(" ", ""))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        if (IsOperation(splitExpression.First()))
            throw new AggregateException(MathErrorMessager.StartingWithOperation);

        if(IsOperation(splitExpression.Last()))
            throw new ArgumentException(MathErrorMessager.EndingWithOperation);

        ValidateParenthesis(splitExpression);
        ValidateExpressionCharacters(expression);
        ValidateExpressionNumbers(splitExpression);
    }

    private bool IsOperation(string token) => 
        OperationsList.Contains(token);

    private bool IsNumber(string token) =>
        double.TryParse(token, out var _);

    private void ValidateParenthesis(string[] expression)
    {
        var parenthesisStack = new Stack<string>();

        for(int i = 0; i < expression.Length; i++)
        {
            if (IsOperation(expression[i]) && i + 1 < expression.Length && IsOperation(expression[i + 1]))
                throw new ArgumentException(MathErrorMessager.TwoOperationInRowMessage(
                    expression[i],
                    expression[i + 1]));

            switch (expression[i])
            {
                case "(":
                    if (i + 1 < expression.Length && IsOperation(expression[i + 1]) &&
                        !(expression[i + 1] == "-" && i + 2 < expression.Length &&
                          double.TryParse(expression[i + 2], out _)))
                    {
                        throw new ArgumentException(
                            MathErrorMessager.InvalidOperatorAfterParenthesisMessage(expression[i + 1]));
                    }

                    parenthesisStack.Push(expression[i]);
                    break;
                case ")":
                    if (i - 1 >= 0 && IsOperation(expression[i - 1]))
                        throw new ArgumentException(
                            MathErrorMessager.OperationBeforeParenthesisMessage(expression[i - 1]));

                    if (parenthesisStack.Count == 0 || parenthesisStack.Pop() != "(")
                        throw new ArgumentException(MathErrorMessager.IncorrectBracketsNumber);
                    break;
            }
        }
        if (parenthesisStack.Count != 0)
            throw new ArgumentException(MathErrorMessager.IncorrectBracketsNumber);
    }

    private void ValidateExpressionCharacters(string expression)
    {
        var splitExpression = expression.Replace(" ", "");

        var expressionDigitsOnly = splitExpression
            .Where(x => !(OperationsList.Contains(x.ToString()) 
                          || new[] { "(", ")", ",", "."}.Contains(x.ToString())))
            .Where(x => !char.IsDigit(x))
            .ToArray();

        if (expressionDigitsOnly.Length != 0)
            throw new ArgumentException(MathErrorMessager.UnknownCharacterMessage(expressionDigitsOnly.First()));
    }

    private void ValidateExpressionNumbers(string[] expression)
    {
        var expressionNumbersOnly = expression
            .Where(x => !(OperationsList.Contains(x) || new[] { "(", ")"}.Contains(x)))
            .ToArray();

        foreach (var token in expressionNumbersOnly)
        {
            if (!IsNumber(token))
                throw new ArgumentException(MathErrorMessager.NotNumberMessage(token));
        }
    }
}