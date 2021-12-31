using System;
using System.IO;
using System.Linq;
using System.Text;
using GoTools.LanguageParser.ParsedToken;
using GoTools.LanguageParser.Tokenizer;
using GoTools.LanguageParser.Tokenizer.CSharp;

namespace GoTools.LanguageParser.Tests;

public abstract class TestBase
{
    private static ParsedToken<TLanguageToken>[] Tokenize<TLanguageToken>(string text, Func<Stream, Tokenizer<TLanguageToken>> tokenizerFactory) where TLanguageToken : struct, Enum
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
        using var tokenizer = tokenizerFactory(stream);

        return tokenizer
            .Tokenize()
            .Cast<ParsedToken<TLanguageToken>>()
            .ToArray();
    }

    protected static ParsedToken<CSharpToken>[] TokenizeCSharp(string code)
        => Tokenize(code, stream => new CSharpTokenizer(stream));
}