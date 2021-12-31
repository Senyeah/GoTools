using System;

namespace GoTools.LanguageParser.Parser.Exceptions;

public class SyntaxErrorException<TLanguageToken> : Exception where TLanguageToken : struct, Enum
{
    public TLanguageToken[] ExpectedTokens { get; }
    public TLanguageToken? ActualToken { get; }

    public SyntaxErrorException(TLanguageToken expected, TLanguageToken actual)
        : base($"Expected {expected} but got {actual}")
    {
        ExpectedTokens = new[] { expected };
        ActualToken = actual;
    }

    public SyntaxErrorException(TLanguageToken expected)
        : base($"Expected {expected} but found nothing")
    {
        ExpectedTokens = new[] { expected };
    }

    public SyntaxErrorException(TLanguageToken[] expectedAny, TLanguageToken actual)
        : base($"Expected one of {string.Join(", ", expectedAny)} but instead found {actual}")
    {
        ExpectedTokens = expectedAny;
        ActualToken = actual;
    }
}