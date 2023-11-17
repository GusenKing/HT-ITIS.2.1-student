using System.Linq.Expressions;
using Hw9.Dto;
using Hw9.ExpressionHelper;
using Hw9.Regex;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            var validator = new ExpressionValidator();
            validator.Validate(expression);
            
            var expressionParser = new ExpressionConverter();
            var expressionInPolishNotation = expressionParser.ConvertToPostfixNotation(
                RegexExpressions.SplitDelimiter.Split(expression!));

            var expressionConverted = ExpressionTreeConverter.ConvertToExpressionTree(expressionInPolishNotation);

            var result = Expression.Lambda<Func<double>>(
                await MyExpressionVisitor.VisitExpression(expressionConverted)).Compile().Invoke();
            return new CalculationMathExpressionResultDto(result);
        }
        catch (Exception exception)
        {
            return new CalculationMathExpressionResultDto(exception.Message);
        }
    }
}