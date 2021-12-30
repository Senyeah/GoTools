using System;
using System.Collections.Generic;
using System.Linq;
using GoTools.LanguageParser.ParsedToken;

namespace GoTools.LanguageParser.Parser.TokenConsumer;

public class TokenConsumer<TLanguageToken> : ITokenConsumer<TLanguageToken> where TLanguageToken : struct, Enum
{
    private readonly TLanguageToken _symbolToken;
    private readonly IEqualityComparer<TLanguageToken> _tokenEqualityComparer;
    private readonly Stack<IParsedToken<TLanguageToken>> _tokens;

    public TokenConsumer(TLanguageToken symbolToken, IEnumerable<IParsedToken<TLanguageToken>> tokens)
    {
        _tokens = new(tokens);
        _symbolToken = symbolToken;
        _tokenEqualityComparer = EqualityComparer<TLanguageToken>.Default;
    }

    public void Consume(params TLanguageToken[] anyToken)
    {
        var token = _tokens.Pop();

        if (anyToken.Length > 0 && anyToken.All(expected => !_tokenEqualityComparer.Equals(token.Declaration, expected)))
        {
            var expected = string.Join(", ", anyToken);
            throw new($"Expected one of {expected} but instead found {token.Declaration}");
        }
    }

    public string? ConsumeSymbol()
    {
        if (!_tokens.TryPop(out var token))
        {
            throw new("Syntax error: expected identifier but found end of file");
        }

        if (!_tokenEqualityComparer.Equals(_symbolToken, token.Declaration))
        {
            throw new($"Syntax error: expected identifier, got {token.Declaration}");
        }

        return token.Identifier;
    }

    public void ConsumeUntil(TLanguageToken token)
    {
        while (!IsConsumable(token))
        {
            Consume();
        }
    }

    public bool IsAnyConsumable(params TLanguageToken[] tokens)
        => tokens.Any(IsConsumable);

    public bool IsAnyConsumableAhead(int from, params TLanguageToken[] expectedTokens)
    {
        var toIndex = expectedTokens.Length + from;

        if (toIndex > _tokens.Count)
        {
            return false;
        }

        foreach (var actualToken in _tokens.ToArray()[from..toIndex])
        {
            if (expectedTokens.Any(token => _tokenEqualityComparer.Equals(token, actualToken.Declaration)))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsConsumable(TLanguageToken token)
    {
        return _tokens.Count > 0 && _tokenEqualityComparer.Equals(_tokens.Peek().Declaration, token);
    }

    public bool IsConsumableAhead(int from, params TLanguageToken[] expectedTokens)
    {
        var toIndex = expectedTokens.Length + from;

        if (toIndex > _tokens.Count)
        {
            return false;
        }

        var actualLookahead = _tokens.ToArray()[from..toIndex];

        foreach (var (actual, expected) in actualLookahead.Zip(expectedTokens))
        {
            if (!_tokenEqualityComparer.Equals(actual.Declaration, expected))
            {
                return false;
            }
        }

        return true;
    }

    public bool TryConsume(TLanguageToken token)
    {
        if (IsConsumable(token))
        {
            Consume(token);
            return true;
        }

        return false;
    }
}