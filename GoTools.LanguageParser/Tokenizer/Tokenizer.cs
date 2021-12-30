using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GoTools.LanguageParser.ParsedToken;

namespace GoTools.LanguageParser.Tokenizer;

public abstract class Tokenizer<TLanguageToken> : IDisposable where TLanguageToken : struct, Enum
{
    private readonly StreamReader _streamReader;

    protected abstract IReadOnlyDictionary<string, TLanguageToken> TokenMapping { get; }

    protected Tokenizer(Stream byteStream)
    {
        _streamReader = new(byteStream);
    }

    protected abstract IParsedToken<TLanguageToken> EmitNonSymbol(string matchingNonSymbol, out int? expectedSymbolCount);

    protected abstract IParsedToken<TLanguageToken> EmitSymbol(string? matchingSymbol);

    protected virtual bool IsSymbolTerminationCharacter(char c)
        => char.IsWhiteSpace(c);

    protected virtual bool IsValidSymbol(string symbol)
        => !string.IsNullOrWhiteSpace(symbol);

    public IEnumerable<IParsedToken<TLanguageToken>> Tokenize()
    {
        var buffer = new StringBuilder();

        var tokens = TokenMapping.Keys.OrderByDescending(key => key.Length).ToArray();
        var largestToken = tokens.Select(t => t.Length).Max();

        var index = 0;
        var emitSymbolCount = 0;
        var matchedSymbolIndex = default(int?);
        var hasFullyReadStream = false;

        while (!hasFullyReadStream || index < buffer.Length)
        {
            if (!hasFullyReadStream && _streamReader.Peek() > 0)
            {
                buffer.Append((char)_streamReader.Read());
            }
            else
            {
                hasFullyReadStream = true;
            }

            var match = buffer.ToString(index, buffer.Length - index);
            var matchingToken = tokens.FirstOrDefault(match.StartsWith);

            var isValidTokenStart = index == 0 || IsSymbolTerminationCharacter(buffer[index - 1]);

            // Should the matched token break a symbol match streak?
            if (matchingToken is not null && IsSymbolTerminationCharacter(matchingToken[0]))
            {
                isValidTokenStart = true;
            }

            if (matchingToken is not null && isValidTokenStart)
            {
                // If we were searching for a symbol and encountered a token, emit both
                if (matchedSymbolIndex is not null)
                {
                    var matchedSymbol = buffer.ToString(matchedSymbolIndex.Value, index - matchedSymbolIndex.Value);
                    matchedSymbolIndex = null;

                    if (IsValidSymbol(matchedSymbol) && emitSymbolCount > 0)
                    {
                        emitSymbolCount -= 1;
                        yield return EmitSymbol(matchedSymbol);
                    }
                }

                index += matchingToken.Length;

                yield return EmitNonSymbol(matchingToken, out var expectedSymbolCount);

                // Only set when we're not looking for any symbols, or when we should stop looking
                //if (emitSymbolCount == 0 && expectedSymbolCount > 0 || expectedSymbolCount == 0)
                if (expectedSymbolCount is not null)
                {
                    emitSymbolCount = expectedSymbolCount.Value;
                }
            }
            else if (matchingToken is null && emitSymbolCount > 0)
            {
                var hasSequenceEnded = IsSymbolTerminationCharacter(buffer[index]);
                var didEmitSymbol = false;

                if (hasSequenceEnded && matchedSymbolIndex is not null)
                {
                    // Get the text from the symbol start until now, then emit if valid
                    var matchedSymbol = buffer.ToString(matchedSymbolIndex.Value, index - matchedSymbolIndex.Value);

                    if (IsValidSymbol(matchedSymbol))
                    {
                        matchedSymbolIndex = index + 1;
                        emitSymbolCount -= 1;
                        didEmitSymbol = true;

                        yield return EmitSymbol(matchedSymbol);
                    }
                }

                // Advance index to consume needless whitespace
                if (didEmitSymbol || matchedSymbolIndex is null)
                {
                    index += 1;
                }

                // If we're not currently matching anything, start now
                matchedSymbolIndex ??= index;
            }

            // Maintain a window of largestToken characters (unless we're at the very end)
            // Do not advance reading until the 'window' is at least that long.
            if (hasFullyReadStream || buffer.Length - index > largestToken)
            {
                index += 1;
            }
        }
    }

    void IDisposable.Dispose()
    {
        GC.SuppressFinalize(this);
        _streamReader.Dispose();
    }
}