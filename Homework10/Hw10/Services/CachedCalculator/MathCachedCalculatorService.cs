using Hw10.DbModels;
using Hw10.Dto;
using Hw10.Services.MathCalculator;
using Microsoft.EntityFrameworkCore;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var dbSet = _dbContext.SolvingExpressions;

		if (await dbSet.AnyAsync(expr => expr.Expression.Equals(expression)))
		{
			var fromCache = await dbSet
				.FirstAsync(expr =>
					expr.Expression.Equals(expression));

			await Task.Delay(1000);
			return new CalculationMathExpressionResultDto(fromCache.Result);
		}

		var notCachedYetDto = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		if (notCachedYetDto.IsSuccess)
		{
			await dbSet.AddAsync(new SolvingExpression() { Expression = expression!, Result = notCachedYetDto.Result });
			await _dbContext.SaveChangesAsync();
		}

		return notCachedYetDto;
	}
}