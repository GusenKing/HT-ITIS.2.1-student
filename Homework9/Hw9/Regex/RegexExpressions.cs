using System.Text.RegularExpressions;


namespace Hw9.Regex;

public static class RegexExpressions
{
    public static readonly System.Text.RegularExpressions.Regex SplitDelimiter = new ("(?<=[-+*/()])|(?=[-+*/()])");
    public static readonly System.Text.RegularExpressions.Regex NumberPattern = new (@"^\d+");
}