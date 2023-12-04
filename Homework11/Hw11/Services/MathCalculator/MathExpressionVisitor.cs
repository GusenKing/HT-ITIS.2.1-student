using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Hw11.ErrorMessages;

namespace Hw11.Services.MathCalculator;

public static class MathExpressionVisitor
{
    public static Task<Expression> VisitExpression(Expression expression)
    {
        return Visit((dynamic)expression);
    }

    private static async Task<Expression> Visit(ConstantExpression expression)
    {
        return expression;
    }
    
    private static async Task<Expression> Visit(UnaryExpression expression)
    {
        return expression;
    }    
    
    [ExcludeFromCodeCoverage]
    private static async Task<Expression> Visit(BinaryExpression node)
    {
        var firstTask = new Lazy<Task<Expression>>(async () =>
        {
            await Task.Delay(1000);
            return await Visit((dynamic)node.Left);
        });
        var secondTask = new Lazy<Task<Expression>>(async () =>
        {
            await Task.Delay(1000);
            return await Visit((dynamic)node.Right);
        });

        var result = await Task.WhenAll(firstTask.Value, secondTask.Value);

        if (node.NodeType == ExpressionType.Divide)
        {
            if (Expression.Lambda<Func<double>>(result[1]).Compile().Invoke() == 0)
                throw new Exception(MathErrorMessager.DivisionByZero);
        }
        
        return node.NodeType switch
        {
            ExpressionType.Add => Expression.Add(result[0], result[1]),
            ExpressionType.Subtract => Expression.Subtract(result[0], result[1]),
            ExpressionType.Multiply => Expression.Multiply(result[0], result[1]),
            ExpressionType.Divide => Expression.Divide(result[0], result[1]),
        };
    }
}