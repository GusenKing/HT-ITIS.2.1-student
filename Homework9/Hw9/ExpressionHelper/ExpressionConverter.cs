using System.Text;
using Hw9.Regex;


namespace Hw9.ExpressionHelper;

public class ExpressionConverter
{
    private readonly Dictionary<string, int> _operationPriority = new()
    {
        { "(", 0 },
        { "+", 1 },
        { "-", 1 },
        { "*", 2 },
        { "/", 2 },
        { "~", 4 }
    };

    public string ConvertToPostfixNotation(string[] expressionInput)
    {
        var expression = expressionInput
            .Select(i => i.Replace(" ", ""))
            .Where(i => i != "")
            .ToArray();
        var postfix = new StringBuilder();
        var operations = new Stack<string>();
        var isPreviousOpenParenthesis = false;

        foreach (var t in expression)
        {
            var element = t;

            if (RegexExpressions.NumberPattern.IsMatch(element))
            {
                postfix.Append(element);
                postfix.Append(' ');
                isPreviousOpenParenthesis = false;
                continue;
            }

            switch (element)
            {
                case "(":
                {
                    operations.Push(element);
                    isPreviousOpenParenthesis = true;
                    continue;
                }
                case ")":
                {
                    while (operations.Peek() != "(")
                    {
                        postfix.Append(operations.Pop());
                        postfix.Append(' ');
                    }

                    operations.Pop();
                    isPreviousOpenParenthesis = false;
                    continue;
                }
            }

            if (element == "-" && isPreviousOpenParenthesis)
                element = "~";
            isPreviousOpenParenthesis = false;

            while (operations.Any() && _operationPriority[operations.Peek()] >= _operationPriority[element])
            {
                postfix.Append(operations.Pop());
                postfix.Append(' ');
            }

            operations.Push(element);
        }

        foreach (var operation in operations)
        {
            postfix.Append(operation);
            postfix.Append(' ');
        }

        return postfix.ToString();
    }
}