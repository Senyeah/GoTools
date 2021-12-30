using System;

namespace GoTools.LanguageParser.ParsedToken;

public interface IParsedToken<out TLanguageToken> where TLanguageToken : struct, Enum
{
    TLanguageToken Declaration { get; }
    public string? Identifier { get; }
}