using Hw13.MemoryCachedCalculator.Dto;

namespace Hw13.MemoryCachedCalculator.Services;

public interface IMathCalculatorService
{
    public Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression);
}