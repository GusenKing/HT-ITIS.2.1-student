using System.Diagnostics.CodeAnalysis;

namespace Hw11.Exceptions;

public class ExceptionHandler : IExceptionHandler
{
	private const string UnknownError = "Unknown error";
	private const string InvalidNumber = "Invalid number";
	private const string InvalidSyntax = "Invalid syntax";
	private const string InvalidSymbol = "Invalid symbol";
	
	private readonly ILogger<ExceptionHandler> _logger;

	public ExceptionHandler(ILogger<ExceptionHandler> logger)
	{
		_logger = logger;
	}

	public void HandleException<T>(T exception) where T : Exception
	{
		this.Handle((dynamic) exception);
	}
	
	[ExcludeFromCodeCoverage]
	private void Handle(Exception exception)
	{
		_logger.LogError($"{UnknownError}: {exception.Message}");
	}
	[ExcludeFromCodeCoverage]
	private void Handle(InvalidNumberException exception)
	{
		_logger.LogError($"{InvalidNumber}: {exception.Message}");
	}
	[ExcludeFromCodeCoverage]
	private void Handle(InvalidSyntaxException exception)
	{
		_logger.LogError($"{InvalidSyntax}: {exception.Message}");
	}
	[ExcludeFromCodeCoverage]
	private void Handle(InvalidSymbolException exception)
	{
		_logger.LogError($"{InvalidSymbol}: {exception.Message}");
	}
	[ExcludeFromCodeCoverage]
	private void Handle(DivideByZeroException exception)
	{
		_logger.LogError(exception.Message);
	}
}