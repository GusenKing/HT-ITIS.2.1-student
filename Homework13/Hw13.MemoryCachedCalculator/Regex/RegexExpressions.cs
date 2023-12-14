namespace Hw13.MemoryCachedCalculator.Regex;

public class RegexExpressions
{
    public static readonly System.Text.RegularExpressions.Regex SplitDelimiter = new ("(?<=[-+*/()])|(?=[-+*/()])");
    public static readonly System.Text.RegularExpressions.Regex NumberPattern = new (@"^\d+");
}