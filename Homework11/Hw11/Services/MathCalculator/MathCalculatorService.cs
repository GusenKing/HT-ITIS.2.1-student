using System.Linq.Expressions;
using Hw11.Dto;
using Hw11.Regex;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    { 
        MathExpressionValidator validator = new ();
        validator.Validate(expression);
        
        MathExpressionConverter expressionParser = new ();
        var expressionInPolishNotation = expressionParser.ConvertToPostfixNotation(
            RegexExpressions.SplitDelimiter.Split(expression!));

        var expressionConverted = MathConverterToExpressionTree.ConvertToExpressionTree(expressionInPolishNotation);

        return await Dispatcher.Visit((dynamic)expressionConverted);
    }
}