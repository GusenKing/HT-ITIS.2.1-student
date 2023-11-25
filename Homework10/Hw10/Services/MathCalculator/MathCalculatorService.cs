using System.Linq.Expressions;
using Hw10.Dto;
using Hw10.Regex;

namespace Hw10.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            MathExpressionValidator validator = new ();
            validator.Validate(expression);
            
            MathExpressionConverter expressionParser = new ();
            var expressionInPolishNotation = expressionParser.ConvertToPostfixNotation(
                RegexExpressions.SplitDelimiter.Split(expression!));

            var expressionConverted = MathConverterToExpressionTree.ConvertToExpressionTree(expressionInPolishNotation);

            var result = Expression.Lambda<Func<double>>(
                await MathExpressionVisitor.VisitExpression(expressionConverted)).Compile().Invoke();

            return new CalculationMathExpressionResultDto(result);
        }
        catch (Exception ex)
        {
            return new CalculationMathExpressionResultDto(ex.Message);
        }
    }
}