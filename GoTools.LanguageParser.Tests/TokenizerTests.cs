using GoTools.LanguageParser.Tokenizer.CSharp;
using GoTools.LanguageParser.ParsedToken;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text;

namespace GoTools.LanguageParser.Tests;

[TestClass]
public class TokenizerTests
{
    private static ParsedToken<CSharpToken>[] Tokenize(string text)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
        using var tokenizer = new CSharpTokenizer(stream);

        return tokenizer
            .Tokenize()
            .Cast<ParsedToken<CSharpToken>>()
            .ToArray();
    }

    [TestMethod]
    public void TokenizesCSharpSimpleModel()
    {
        var tokens = Tokenize(@"
            using System;
            using System.Text;

            namespace Test
            {
                public class TestModel
                {
                    public int Property1 { get; set; }
                    public int Property2 { get; set; }
                }
            }
        ");

        Assert.AreEqual(33, tokens.Length);

        Assert.AreEqual(tokens[0], new() { Declaration = CSharpToken.UsingKeyword });
        Assert.AreEqual(tokens[1], new() { Declaration = CSharpToken.Symbol, Identifier = "System" });
        Assert.AreEqual(tokens[2], new() { Declaration = CSharpToken.SemicolonToken });

        Assert.AreEqual(tokens[3], new() { Declaration = CSharpToken.UsingKeyword });
        Assert.AreEqual(tokens[4], new() { Declaration = CSharpToken.Symbol, Identifier = "System.Text" });
        Assert.AreEqual(tokens[5], new() { Declaration = CSharpToken.SemicolonToken });

        Assert.AreEqual(tokens[6], new() { Declaration = CSharpToken.NamespaceDeclaration });
        Assert.AreEqual(tokens[7], new() { Declaration = CSharpToken.Symbol, Identifier = "Test" });
        Assert.AreEqual(tokens[8], new() { Declaration = CSharpToken.OpenBraceToken });

        Assert.AreEqual(tokens[9], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[10], new() { Declaration = CSharpToken.ClassDeclaration });
        Assert.AreEqual(tokens[11], new() { Declaration = CSharpToken.Symbol, Identifier = "TestModel" });
        Assert.AreEqual(tokens[12], new() { Declaration = CSharpToken.OpenBraceToken });

        Assert.AreEqual(tokens[13], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[14], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[15], new() { Declaration = CSharpToken.Symbol, Identifier = "Property1" });
        Assert.AreEqual(tokens[16], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[17], new() { Declaration = CSharpToken.PropertyGetAccessorKeyword });
        Assert.AreEqual(tokens[18], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[19], new() { Declaration = CSharpToken.PropertySetAccessorKeyword });
        Assert.AreEqual(tokens[20], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[21], new() { Declaration = CSharpToken.CloseBraceToken });

        Assert.AreEqual(tokens[22], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[23], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[24], new() { Declaration = CSharpToken.Symbol, Identifier = "Property2" });
        Assert.AreEqual(tokens[25], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[26], new() { Declaration = CSharpToken.PropertyGetAccessorKeyword });
        Assert.AreEqual(tokens[27], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[28], new() { Declaration = CSharpToken.PropertySetAccessorKeyword });
        Assert.AreEqual(tokens[29], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[30], new() { Declaration = CSharpToken.CloseBraceToken });

        Assert.AreEqual(tokens[31], new() { Declaration = CSharpToken.CloseBraceToken });
        Assert.AreEqual(tokens[32], new() { Declaration = CSharpToken.CloseBraceToken });
    }

    [TestMethod]
    public void TokenizesCSharp10Model()
    {
        var tokens = Tokenize(@"
            using System;
            using System.Text;

            namespace Test;

            public class TestModel
            {
                public int Property1 { get; init; }
                public int Property2 { get; set; }
            }
        ");

        Assert.AreEqual(32, tokens.Length);

        Assert.AreEqual(tokens[0], new() { Declaration = CSharpToken.UsingKeyword });
        Assert.AreEqual(tokens[1], new() { Declaration = CSharpToken.Symbol, Identifier = "System" });
        Assert.AreEqual(tokens[2], new() { Declaration = CSharpToken.SemicolonToken });

        Assert.AreEqual(tokens[3], new() { Declaration = CSharpToken.UsingKeyword });
        Assert.AreEqual(tokens[4], new() { Declaration = CSharpToken.Symbol, Identifier = "System.Text" });
        Assert.AreEqual(tokens[5], new() { Declaration = CSharpToken.SemicolonToken });

        Assert.AreEqual(tokens[6], new() { Declaration = CSharpToken.NamespaceDeclaration });
        Assert.AreEqual(tokens[7], new() { Declaration = CSharpToken.Symbol, Identifier = "Test" });
        Assert.AreEqual(tokens[8], new() { Declaration = CSharpToken.SemicolonToken });

        Assert.AreEqual(tokens[9], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[10], new() { Declaration = CSharpToken.ClassDeclaration });
        Assert.AreEqual(tokens[11], new() { Declaration = CSharpToken.Symbol, Identifier = "TestModel" });
        Assert.AreEqual(tokens[12], new() { Declaration = CSharpToken.OpenBraceToken });

        Assert.AreEqual(tokens[13], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[14], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[15], new() { Declaration = CSharpToken.Symbol, Identifier = "Property1" });
        Assert.AreEqual(tokens[16], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[17], new() { Declaration = CSharpToken.PropertyGetAccessorKeyword });
        Assert.AreEqual(tokens[18], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[19], new() { Declaration = CSharpToken.PropertyInitAccessorKeyword });
        Assert.AreEqual(tokens[20], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[21], new() { Declaration = CSharpToken.CloseBraceToken });

        Assert.AreEqual(tokens[22], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[23], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[24], new() { Declaration = CSharpToken.Symbol, Identifier = "Property2" });
        Assert.AreEqual(tokens[25], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[26], new() { Declaration = CSharpToken.PropertyGetAccessorKeyword });
        Assert.AreEqual(tokens[27], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[28], new() { Declaration = CSharpToken.PropertySetAccessorKeyword });
        Assert.AreEqual(tokens[29], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[30], new() { Declaration = CSharpToken.CloseBraceToken });

        Assert.AreEqual(tokens[31], new() { Declaration = CSharpToken.CloseBraceToken });
    }

    [TestMethod]
    public void TokenizesCSharpReservedKeywordIdentifiers()
    {
        // Identifier should be correctly extracted when terminated with a symbol-terminating token
        // Example: int[] should become ["int", Keyword.OpenBracket, Keyword.CloseBracket] and not ["int[]"]

        // Identifiers that start without a symbol-terminating character should not then match a reserved keyword
        // Example: _get should become ["_get"] and not ["_", Keyword.Get]

        var tokens = Tokenize(@"            
            public class Test
            {
                public int[] TestField;
                public DateTime TestProperty => new();
                public int[]? NullableArray => new();
                public int _get { get; init; }
                public int _set;
                public int init { get; set; }
            }
        ");

        Assert.AreEqual(52, tokens.Length);

        Assert.AreEqual(tokens[0], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[1], new() { Declaration = CSharpToken.ClassDeclaration });
        Assert.AreEqual(tokens[2], new() { Declaration = CSharpToken.Symbol, Identifier = "Test" });
        Assert.AreEqual(tokens[3], new() { Declaration = CSharpToken.OpenBraceToken });

        Assert.AreEqual(tokens[4], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[5], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[6], new() { Declaration = CSharpToken.OpenSquareBracketToken });
        Assert.AreEqual(tokens[7], new() { Declaration = CSharpToken.CloseSquareBracketToken });
        Assert.AreEqual(tokens[8], new() { Declaration = CSharpToken.Symbol, Identifier = "TestField" });
        Assert.AreEqual(tokens[9], new() { Declaration = CSharpToken.SemicolonToken });

        Assert.AreEqual(tokens[10], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[11], new() { Declaration = CSharpToken.Symbol, Identifier = "DateTime" });
        Assert.AreEqual(tokens[12], new() { Declaration = CSharpToken.Symbol, Identifier = "TestProperty" });
        Assert.AreEqual(tokens[13], new() { Declaration = CSharpToken.LambdaBodyToken });
        Assert.AreEqual(tokens[14], new() { Declaration = CSharpToken.Symbol, Identifier = "new" });
        Assert.AreEqual(tokens[15], new() { Declaration = CSharpToken.OpenBracketToken });
        Assert.AreEqual(tokens[16], new() { Declaration = CSharpToken.CloseBracketToken });
        Assert.AreEqual(tokens[17], new() { Declaration = CSharpToken.SemicolonToken });

        Assert.AreEqual(tokens[18], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[19], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[20], new() { Declaration = CSharpToken.OpenSquareBracketToken });
        Assert.AreEqual(tokens[21], new() { Declaration = CSharpToken.CloseSquareBracketToken });
        Assert.AreEqual(tokens[22], new() { Declaration = CSharpToken.QuestionMarkToken });
        Assert.AreEqual(tokens[23], new() { Declaration = CSharpToken.Symbol, Identifier = "NullableArray" });
        Assert.AreEqual(tokens[24], new() { Declaration = CSharpToken.LambdaBodyToken });
        Assert.AreEqual(tokens[25], new() { Declaration = CSharpToken.Symbol, Identifier = "new" });
        Assert.AreEqual(tokens[26], new() { Declaration = CSharpToken.OpenBracketToken });
        Assert.AreEqual(tokens[27], new() { Declaration = CSharpToken.CloseBracketToken });
        Assert.AreEqual(tokens[28], new() { Declaration = CSharpToken.SemicolonToken });

        Assert.AreEqual(tokens[29], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[30], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[31], new() { Declaration = CSharpToken.Symbol, Identifier = "_get" });
        Assert.AreEqual(tokens[32], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[33], new() { Declaration = CSharpToken.PropertyGetAccessorKeyword });
        Assert.AreEqual(tokens[34], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[35], new() { Declaration = CSharpToken.PropertyInitAccessorKeyword });
        Assert.AreEqual(tokens[36], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[37], new() { Declaration = CSharpToken.CloseBraceToken });

        Assert.AreEqual(tokens[38], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[39], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[40], new() { Declaration = CSharpToken.Symbol, Identifier = "_set" });
        Assert.AreEqual(tokens[41], new() { Declaration = CSharpToken.SemicolonToken });

        Assert.AreEqual(tokens[42], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[43], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[44], new() { Declaration = CSharpToken.PropertyInitAccessorKeyword });
        Assert.AreEqual(tokens[45], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[46], new() { Declaration = CSharpToken.PropertyGetAccessorKeyword });
        Assert.AreEqual(tokens[47], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[48], new() { Declaration = CSharpToken.PropertySetAccessorKeyword });
        Assert.AreEqual(tokens[49], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[50], new() { Declaration = CSharpToken.CloseBraceToken });

        Assert.AreEqual(tokens[51], new() { Declaration = CSharpToken.CloseBraceToken });
    }

    [TestMethod]
    public void TokenizesComplexModelWithImplementedInterfaces()
    {
        var tokens = Tokenize(@"
            public class ComplexModel : IComplexModel, ComplexBase
            {
                public int _field => 3;
                public DateTime? Property { get; init; }
            }
        ");

        Assert.AreEqual(25, tokens.Length);

        Assert.AreEqual(tokens[0], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[1], new() { Declaration = CSharpToken.ClassDeclaration });
        Assert.AreEqual(tokens[2], new() { Declaration = CSharpToken.Symbol, Identifier = "ComplexModel" });
        Assert.AreEqual(tokens[3], new() { Declaration = CSharpToken.ColonToken });
        Assert.AreEqual(tokens[4], new() { Declaration = CSharpToken.Symbol, Identifier = "IComplexModel" });
        Assert.AreEqual(tokens[5], new() { Declaration = CSharpToken.CommaToken });
        Assert.AreEqual(tokens[6], new() { Declaration = CSharpToken.Symbol, Identifier = "ComplexBase" });
        Assert.AreEqual(tokens[7], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[8], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[9], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[10], new() { Declaration = CSharpToken.Symbol, Identifier = "_field" });
        Assert.AreEqual(tokens[11], new() { Declaration = CSharpToken.LambdaBodyToken });
        Assert.AreEqual(tokens[12], new() { Declaration = CSharpToken.Symbol, Identifier = "3" });
        Assert.AreEqual(tokens[13], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[14], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[15], new() { Declaration = CSharpToken.Symbol, Identifier = "DateTime" });
        Assert.AreEqual(tokens[16], new() { Declaration = CSharpToken.QuestionMarkToken });
        Assert.AreEqual(tokens[17], new() { Declaration = CSharpToken.Symbol, Identifier = "Property" });
        Assert.AreEqual(tokens[18], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[19], new() { Declaration = CSharpToken.PropertyGetAccessorKeyword });
        Assert.AreEqual(tokens[20], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[21], new() { Declaration = CSharpToken.PropertyInitAccessorKeyword });
        Assert.AreEqual(tokens[22], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[23], new() { Declaration = CSharpToken.CloseBraceToken });
        Assert.AreEqual(tokens[24], new() { Declaration = CSharpToken.CloseBraceToken });
    }

    [TestMethod]
    public void TokenizesComplexModelWithConstructor()
    {
        var tokens = Tokenize(@"
            public class ModelWithConstructor
            {
                public readonly int _x;
                private string? TestProperty { get; init; }

                public ModelWithConstructor()
                {
                    _x = 4;
                }
            }
        ");

        Assert.AreEqual(27, tokens.Length);

        Assert.AreEqual(tokens[0], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[1], new() { Declaration = CSharpToken.ClassDeclaration });
        Assert.AreEqual(tokens[2], new() { Declaration = CSharpToken.Symbol, Identifier = "ModelWithConstructor" });
        Assert.AreEqual(tokens[3], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[4], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[5], new() { Declaration = CSharpToken.ReadonlyKeyword });
        Assert.AreEqual(tokens[6], new() { Declaration = CSharpToken.Symbol, Identifier = "int" });
        Assert.AreEqual(tokens[7], new() { Declaration = CSharpToken.Symbol, Identifier = "_x" });
        Assert.AreEqual(tokens[8], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[9], new() { Declaration = CSharpToken.PrivateKeyword });
        Assert.AreEqual(tokens[10], new() { Declaration = CSharpToken.Symbol, Identifier = "string" });
        Assert.AreEqual(tokens[11], new() { Declaration = CSharpToken.QuestionMarkToken });
        Assert.AreEqual(tokens[12], new() { Declaration = CSharpToken.Symbol, Identifier = "TestProperty" });
        Assert.AreEqual(tokens[13], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[14], new() { Declaration = CSharpToken.PropertyGetAccessorKeyword });
        Assert.AreEqual(tokens[15], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[16], new() { Declaration = CSharpToken.PropertyInitAccessorKeyword });
        Assert.AreEqual(tokens[17], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[18], new() { Declaration = CSharpToken.CloseBraceToken });
        Assert.AreEqual(tokens[19], new() { Declaration = CSharpToken.PublicKeyword });
        Assert.AreEqual(tokens[20], new() { Declaration = CSharpToken.Symbol, Identifier = "ModelWithConstructor" });
        Assert.AreEqual(tokens[21], new() { Declaration = CSharpToken.OpenBracketToken });
        Assert.AreEqual(tokens[22], new() { Declaration = CSharpToken.CloseBracketToken });
        Assert.AreEqual(tokens[23], new() { Declaration = CSharpToken.OpenBraceToken });
        Assert.AreEqual(tokens[24], new() { Declaration = CSharpToken.SemicolonToken });
        Assert.AreEqual(tokens[25], new() { Declaration = CSharpToken.CloseBraceToken });
        Assert.AreEqual(tokens[26], new() { Declaration = CSharpToken.CloseBraceToken });
    }
}
