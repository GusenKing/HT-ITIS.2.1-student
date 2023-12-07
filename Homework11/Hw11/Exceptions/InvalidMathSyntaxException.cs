using System.Diagnostics.CodeAnalysis;

namespace Hw11.Exceptions;

public class InvalidSyntaxException : Exception
{
	[ExcludeFromCodeCoverage]
	public InvalidSyntaxException(string message)
		: base(message)
	{
	}
}