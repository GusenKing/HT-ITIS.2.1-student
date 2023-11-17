using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Hw9.ErrorMessages;

namespace Hw9.Services.MathCalculator;

public class MyExpressionVisitor : ExpressionVisitor
{
    public static Task<Expression> VisitExpression(Expression expression)
    {
        return Task.Run(() => new MyExpressionVisitor().Visit(expression));
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        try
        {
            return VisitBinaryAsync(node).Result;
        }
        catch (AggregateException ex)
        {
            throw ex.InnerException;
        }
    }

    [ExcludeFromCodeCoverage]
    protected override Expression VisitUnary(UnaryExpression node)
    {
        try
        {
            return VisitUnaryAsync(node).Result;
        }
        catch (AggregateException ex)
        {
            throw ex.InnerException;
        }
    }

    private async Task<Expression> VisitUnaryAsync(UnaryExpression node)
    {
        await Task.Delay(1000);
        var operand = node.Operand;
        if (operand is BinaryExpression binaryNode)
            return Expression.Negate(VisitBinaryAsync(binaryNode).Result);
        
        if (operand is UnaryExpression unaryNode)
            return Expression.Negate(VisitUnaryAsync(unaryNode).Result);
        
        return node;
    }
    
    private async Task<Expression> VisitBinaryAsync(BinaryExpression node)
    {
        var firstTask = new Lazy<Task<Expression>>(async () =>
        {
            await Task.Delay(1000);
            if (node.Left is BinaryExpression binaryLeft)
                return await VisitBinaryAsync(binaryLeft);
            
            if (node.Left is UnaryExpression unaryLeft)
                return await VisitUnaryAsync(unaryLeft);
            
            return node.Left;
        });
        var secondTask = new Lazy<Task<Expression>>(async () =>
        {
            await Task.Delay(1000);
            if (node.Right is BinaryExpression binaryRight)
                return await VisitBinaryAsync(binaryRight);

            if (node.Right is UnaryExpression unaryRight)
                return await VisitUnaryAsync(unaryRight);
            
            return node.Right;
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