using System.Collections.Generic;
using System.IO;
using GoTools.LanguageParser.ParsedToken;

namespace GoTools.LanguageParser.Tokenizer.CSharp;

public sealed class CSharpTokenizer : Tokenizer<CSharpToken>
{
    protected override IReadOnlyDictionary<string, CSharpToken> TokenMapping => new Dictionary<string, CSharpToken>
    {
        { "class", CSharpToken.ClassDeclaration },
        { "record", CSharpToken.RecordDeclaration },
        { "namespace", CSharpToken.NamespaceDeclaration },
        { "public", CSharpToken.PublicKeyword },
        { "private", CSharpToken.PrivateKeyword },
        { "readonly", CSharpToken.ReadonlyKeyword },
        { "virtual", CSharpToken.VirtualKeyword },
        { "get", CSharpToken.PropertyGetAccessorKeyword },
        { "set", CSharpToken.PropertySetAccessorKeyword },
        { "init", CSharpToken.PropertyInitAccessorKeyword },
        { "using", CSharpToken.UsingKeyword },
        { "(", CSharpToken.OpenBracketToken },
        { ")", CSharpToken.CloseBracketToken },
        { "{", CSharpToken.OpenBraceToken },
        { "}", CSharpToken.CloseBraceToken },
        { "[", CSharpToken.OpenSquareBracketToken },
        { "]", CSharpToken.CloseSquareBracketToken },
        { "=>", CSharpToken.LambdaBodyToken },
        { "?", CSharpToken.QuestionMarkToken },
        { ":", CSharpToken.ColonToken },
        { ";", CSharpToken.SemicolonToken }
    };

    public CSharpTokenizer(Stream byteStream)
        : base(byteStream) { }

    protected override ParsedToken<CSharpToken> EmitNonSymbol(string matchingNonSymbol, out int? expectedSymbolCount)
    {
        var token = TokenMapping[matchingNonSymbol];

        expectedSymbolCount = token switch
        {
            CSharpToken.PublicKeyword => 2,
            CSharpToken.PrivateKeyword => 2,
            CSharpToken.ReadonlyKeyword => 2,
            CSharpToken.NamespaceDeclaration => 1,
            CSharpToken.ClassDeclaration => 1,
            CSharpToken.RecordDeclaration => 1,
            CSharpToken.ColonToken => 1,
            CSharpToken.UsingKeyword => 1,
            CSharpToken.OpenBraceToken => 0,
            _ => null
        };

        return new() { Declaration = token };
    }

    protected override ParsedToken<CSharpToken> EmitSymbol(string? matchingSymbol)
        => new()
        {
            Declaration = CSharpToken.Symbol,
            Identifier = matchingSymbol
        };

    protected override bool IsSymbolTerminationCharacter(char c)
        => c is '(' or ')' or '?' or ';' || char.IsWhiteSpace(c);
}