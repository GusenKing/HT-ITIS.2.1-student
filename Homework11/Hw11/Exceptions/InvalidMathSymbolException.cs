using System.Diagnostics.CodeAnalysis;

namespace Hw11.Exceptions;

public class InvalidSymbolException: Exception
{
	[ExcludeFromCodeCoverage]
	public InvalidSymbolException(string message)
		: base(message)
	{
	}
}