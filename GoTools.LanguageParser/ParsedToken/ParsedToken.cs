using System;

namespace GoTools.LanguageParser.ParsedToken;

public record ParsedToken<TLanguageToken> : IParsedToken<TLanguageToken> where TLanguageToken : struct, Enum
{
    public TLanguageToken Declaration { get; init; }
    public string? Identifier { get; init; }
    public MatchedSpan? TextSpan { get; init; }

    public record MatchedSpan
    {
        public int Start { get; init; }
        public int Length { get; init; }
    }
}