using System.Text.Json;
using Hw9.ErrorMessages;

namespace Hw9.Expressions;

public static class ExpressionValidator
{
    private static readonly char[] OperationsList = { '+', '-', '*', '/' };
        
    public static void Validate(string? expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new ArgumentException(MathErrorMessager.EmptyString);

        expression = expression.Replace(" ", "");

        
        if (IsCharOperation(expression.First()))
            throw new AggregateException(MathErrorMessager.StartingWithOperation);
        
        if(IsCharOperation(expression.Last()))
            throw new ArgumentException(MathErrorMessager.EndingWithOperation);

        ValidateParenthesis(expression);
    }
    
    private static bool IsCharOperation(char ch)
    {
        return OperationsList.Contains(ch);
    }

    private static void ValidateParenthesis(string expression)
    {
        var parenthesisStack = new Stack<char>();

        for(int i = 0; i < expression.Length; i++)
        {
            if (IsCharOperation(expression[i]) && i + 1 < expression.Length && IsCharOperation(expression[i + 1]))
                throw new ArgumentException(MathErrorMessager.TwoOperationInRowMessage(
                    expression[i].ToString(),
                    expression[i + 1].ToString()));
            
            switch (expression[i])
            {
                case '(':
                    if (i + 1 < expression.Length && IsCharOperation(expression[i + 1]))
                        if(!(expression[i + 1] == '-' && i + 2 < expression.Length && double.TryParse(expression[i + 2].ToString(), out _)))
                            throw new ArgumentException(
                                MathErrorMessager.InvalidOperatorAfterParenthesisMessage(expression[i + 1].ToString()));
                    
                    parenthesisStack.Push(expression[i]);
                    break;
                case ')':
                    if (i - 1 >= 0 && IsCharOperation(expression[i - 1]))
                        throw new ArgumentException(
                            MathErrorMessager.OperationBeforeParenthesisMessage(expression[i - 1].ToString()));

                    if (parenthesisStack.Count == 0 || parenthesisStack.Pop() != '(')
                        throw new ArgumentException(MathErrorMessager.IncorrectBracketsNumber);
                    break;
            }
        }
        if (parenthesisStack.Count != 0)
            throw new ArgumentException(MathErrorMessager.IncorrectBracketsNumber);
    }
}