using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator;

public static class Dispatcher
{
    public static async Task<double> Visit(BinaryExpression expression)
    {
        var exp = await new MathExpressionVisitor().VisitBinaryAsync(expression);
        return Expression.Lambda<Func<double>>(exp).Compile().Invoke();
    }
    
    public static async Task<double> Visit(ConstantExpression expression)
    {
        var exp =  new MathExpressionVisitor().VisitConstant(expression);
        return Expression.Lambda<Func<double>>(exp).Compile().Invoke();
    }
}