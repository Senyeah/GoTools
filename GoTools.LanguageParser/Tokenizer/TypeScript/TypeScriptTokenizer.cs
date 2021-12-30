using System.Collections.Generic;
using System.IO;
using GoTools.LanguageParser.ParsedToken;

namespace GoTools.LanguageParser.Tokenizer.TypeScript;

public sealed class TypeScriptTokenizer : Tokenizer<TypeScriptToken>
{
    protected override IReadOnlyDictionary<string, TypeScriptToken> TokenMapping => new Dictionary<string, TypeScriptToken>
    {
        { "class", TypeScriptToken.ClassDeclaration },
        { "namespace", TypeScriptToken.NamespaceDeclaration },
        { "public", TypeScriptToken.PublicKeyword },
        { "import", TypeScriptToken.ImportKeyword },
        { "export", TypeScriptToken.ExportKeyword },
        { "{", TypeScriptToken.OpenBraceToken },
        { "}", TypeScriptToken.CloseBraceToken },
        { "[", TypeScriptToken.OpenBracketToken },
        { "]", TypeScriptToken.CloseBracketToken },
        { ":", TypeScriptToken.ColonToken }
    };

    public TypeScriptTokenizer(Stream byteStream)
        : base(byteStream) { }

    protected override ParsedToken<TypeScriptToken> EmitNonSymbol(string matchingNonSymbol, out int? expectedSymbolCount)
    {
        var token = TokenMapping[matchingNonSymbol];

        expectedSymbolCount = token switch
        {
            TypeScriptToken.NamespaceDeclaration => 1,
            TypeScriptToken.ClassDeclaration => 1,
            TypeScriptToken.ColonToken => 1,
            TypeScriptToken.PublicKeyword => 1,
            TypeScriptToken.ImportKeyword => 1,
            _ => null
        };

        return new()
        {
            Declaration = token
        };
    }

    protected override ParsedToken<TypeScriptToken> EmitSymbol(string? matchingSymbol)
        => new()
        {
            Declaration = TypeScriptToken.Symbol,
            Identifier = matchingSymbol
        };

    protected override bool IsSymbolTerminationCharacter(char c)
        => c is '(' or ')' or ';' || char.IsWhiteSpace(c);
}