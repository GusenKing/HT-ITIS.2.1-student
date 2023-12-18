using Hw13.MemoryCachedCalculator.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace Hw13.MemoryCachedCalculator.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly IMemoryCache _memoryCache;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(IMemoryCache memoryCache, IMathCalculatorService simpleCalculator)
	{
		_memoryCache = memoryCache;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		return await _memoryCache.GetOrCreateAsync<CalculationMathExpressionResultDto>(
			expression,
			async _ =>
			{
				var res = await _simpleCalculator.CalculateMathExpressionAsync(expression);
				return res.IsSuccess
					? new CalculationMathExpressionResultDto(res.Result)
					: new CalculationMathExpressionResultDto(res.ErrorMessage);
			});
	}
}