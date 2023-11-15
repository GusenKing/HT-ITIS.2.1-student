using Hw9.Dto;
using Hw9.Expressions;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            ExpressionValidator.Validate(expression);
            return new CalculationMathExpressionResultDto(1);
        }
        catch (Exception exception)
        {
            return new CalculationMathExpressionResultDto(exception.Message);
        }
    }
}