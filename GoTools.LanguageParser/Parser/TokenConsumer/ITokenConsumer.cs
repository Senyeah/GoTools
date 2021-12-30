using System;

namespace GoTools.LanguageParser.Parser.TokenConsumer;

public interface ITokenConsumer<in TLanguageToken> where TLanguageToken : struct, Enum
{
    void Consume(params TLanguageToken[] anyToken);

    string? ConsumeSymbol();

    void ConsumeUntil(TLanguageToken token);

    bool IsAnyConsumable(params TLanguageToken[] tokens);

    bool IsAnyConsumableAhead(int from, params TLanguageToken[] anyToken);

    bool IsConsumable(TLanguageToken token);

    bool IsConsumableAhead(int from, params TLanguageToken[] expectedTokens);

    bool TryConsume(TLanguageToken token);
}