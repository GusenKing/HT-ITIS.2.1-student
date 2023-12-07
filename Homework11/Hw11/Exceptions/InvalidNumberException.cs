using System.Diagnostics.CodeAnalysis;

namespace Hw11.Exceptions;

public class InvalidNumberException: Exception
{
	[ExcludeFromCodeCoverage]
	public InvalidNumberException(string message)
		: base(message)
	{
	}
}