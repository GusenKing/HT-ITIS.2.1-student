using Hw13.MemoryCachedCalculator.Services;
using Hw13.MemoryCachedCalculator.Services.CachedCalculator;
using Hw13.MemoryCachedCalculator.Services.MathCalculator;
using Microsoft.Extensions.Caching.Memory;

namespace Hw13.MemoryCachedCalculator.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services.AddTransient<MathCalculatorService>();
    }
    
    public static IServiceCollection AddCachedMathCalculator(this IServiceCollection services)
    {
        return services.AddScoped<IMathCalculatorService>(s =>
            new MathCachedCalculatorService(
                s.GetRequiredService<IMemoryCache>(), 
                s.GetRequiredService<MathCalculatorService>()));
    }
}