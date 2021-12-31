using GoTools.LanguageParser.Tokenizer.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoTools.LanguageParser.Tests;

[TestClass]
public class TokenizerTests : TestBase
{
    [TestMethod]
    public void TokenizesCSharpSimpleModel()
    {
        var tokens = TokenizeCSharp(@"
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

        Assert.AreEqual(new() { Declaration = CSharpToken.UsingKeyword }, tokens[0]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "System" }, tokens[1]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[2]);

        Assert.AreEqual(new() { Declaration = CSharpToken.UsingKeyword }, tokens[3]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "System.Text" }, tokens[4]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[5]);

        Assert.AreEqual(new() { Declaration = CSharpToken.NamespaceDeclaration }, tokens[6]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "Test" }, tokens[7]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[8]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[9]);
        Assert.AreEqual(new() { Declaration = CSharpToken.ClassDeclaration }, tokens[10]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "TestModel" }, tokens[11]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[12]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[13]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[14]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "Property1" }, tokens[15]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[16]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyGetAccessorKeyword }, tokens[17]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[18]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertySetAccessorKeyword }, tokens[19]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[20]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[21]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[22]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[23]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "Property2" }, tokens[24]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[25]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyGetAccessorKeyword }, tokens[26]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[27]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertySetAccessorKeyword }, tokens[28]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[29]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[30]);

        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[31]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[32]);
    }

    [TestMethod]
    public void TokenizesCSharp10Model()
    {
        var tokens = TokenizeCSharp(@"
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

        Assert.AreEqual(new() { Declaration = CSharpToken.UsingKeyword }, tokens[0]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "System" }, tokens[1]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[2]);

        Assert.AreEqual(new() { Declaration = CSharpToken.UsingKeyword }, tokens[3]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "System.Text" }, tokens[4]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[5]);

        Assert.AreEqual(new() { Declaration = CSharpToken.NamespaceDeclaration }, tokens[6]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "Test" }, tokens[7]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[8]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[9]);
        Assert.AreEqual(new() { Declaration = CSharpToken.ClassDeclaration }, tokens[10]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "TestModel" }, tokens[11]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[12]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[13]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[14]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "Property1" }, tokens[15]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[16]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyGetAccessorKeyword }, tokens[17]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[18]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyInitAccessorKeyword }, tokens[19]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[20]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[21]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[22]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[23]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "Property2" }, tokens[24]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[25]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyGetAccessorKeyword }, tokens[26]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[27]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertySetAccessorKeyword }, tokens[28]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[29]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[30]);

        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[31]);
    }

    [TestMethod]
    public void TokenizesCSharpReservedKeywordIdentifiers()
    {
        // Identifier should be correctly extracted when terminated with a symbol-terminating token
        // Example: int[] should become ["int", Keyword.OpenBracket, Keyword.CloseBracket] and not ["int[]"]

        // Identifiers that start without a symbol-terminating character should not then match a reserved keyword
        // Example: _get should become ["_get"] and not ["_", Keyword.Get]

        var tokens = TokenizeCSharp(@"            
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

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[0]);
        Assert.AreEqual(new() { Declaration = CSharpToken.ClassDeclaration }, tokens[1]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "Test" }, tokens[2]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[3]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[4]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[5]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenSquareBracketToken }, tokens[6]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseSquareBracketToken }, tokens[7]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "TestField" }, tokens[8]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[9]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[10]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "DateTime" }, tokens[11]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "TestProperty" }, tokens[12]);
        Assert.AreEqual(new() { Declaration = CSharpToken.LambdaBodyToken }, tokens[13]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "new" }, tokens[14]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBracketToken }, tokens[15]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBracketToken }, tokens[16]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[17]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[18]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[19]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenSquareBracketToken }, tokens[20]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseSquareBracketToken }, tokens[21]);
        Assert.AreEqual(new() { Declaration = CSharpToken.QuestionMarkToken }, tokens[22]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "NullableArray" }, tokens[23]);
        Assert.AreEqual(new() { Declaration = CSharpToken.LambdaBodyToken }, tokens[24]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "new" }, tokens[25]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBracketToken }, tokens[26]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBracketToken }, tokens[27]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[28]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[29]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[30]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "_get" }, tokens[31]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[32]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyGetAccessorKeyword }, tokens[33]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[34]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyInitAccessorKeyword }, tokens[35]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[36]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[37]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[38]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[39]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "_set" }, tokens[40]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[41]);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[42]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[43]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyInitAccessorKeyword }, tokens[44]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[45]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyGetAccessorKeyword }, tokens[46]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[47]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertySetAccessorKeyword }, tokens[48]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[49]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[50]);

        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[51]);
    }

    [TestMethod]
    public void TokenizesComplexModelWithImplementedInterfaces()
    {
        var tokens = TokenizeCSharp(@"
            public class ComplexModel : IComplexModel, ComplexBase
            {
                public int _field => 3;
                public DateTime? Property { get; init; }
            }
        ");

        Assert.AreEqual(25, tokens.Length);

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[0]);
        Assert.AreEqual(new() { Declaration = CSharpToken.ClassDeclaration }, tokens[1]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "ComplexModel" }, tokens[2]);
        Assert.AreEqual(new() { Declaration = CSharpToken.ColonToken }, tokens[3]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "IComplexModel" }, tokens[4]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CommaToken }, tokens[5]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "ComplexBase" }, tokens[6]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[7]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[8]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[9]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "_field" }, tokens[10]);
        Assert.AreEqual(new() { Declaration = CSharpToken.LambdaBodyToken }, tokens[11]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "3" }, tokens[12]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[13]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[14]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "DateTime" }, tokens[15]);
        Assert.AreEqual(new() { Declaration = CSharpToken.QuestionMarkToken }, tokens[16]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "Property" }, tokens[17]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[18]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyGetAccessorKeyword }, tokens[19]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[20]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyInitAccessorKeyword }, tokens[21]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[22]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[23]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[24]);
    }

    [TestMethod]
    public void TokenizesComplexModelWithConstructor()
    {
        var tokens = TokenizeCSharp(@"
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

        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[0]);
        Assert.AreEqual(new() { Declaration = CSharpToken.ClassDeclaration }, tokens[1]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "ModelWithConstructor" }, tokens[2]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[3]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[4]);
        Assert.AreEqual(new() { Declaration = CSharpToken.ReadonlyKeyword }, tokens[5]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "int" }, tokens[6]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "_x" }, tokens[7]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[8]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PrivateKeyword }, tokens[9]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "string" }, tokens[10]);
        Assert.AreEqual(new() { Declaration = CSharpToken.QuestionMarkToken }, tokens[11]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "TestProperty" }, tokens[12]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[13]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyGetAccessorKeyword }, tokens[14]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[15]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PropertyInitAccessorKeyword }, tokens[16]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[17]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[18]);
        Assert.AreEqual(new() { Declaration = CSharpToken.PublicKeyword }, tokens[19]);
        Assert.AreEqual(new() { Declaration = CSharpToken.Symbol, Identifier = "ModelWithConstructor" }, tokens[20]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBracketToken }, tokens[21]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBracketToken }, tokens[22]);
        Assert.AreEqual(new() { Declaration = CSharpToken.OpenBraceToken }, tokens[23]);
        Assert.AreEqual(new() { Declaration = CSharpToken.SemicolonToken }, tokens[24]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[25]);
        Assert.AreEqual(new() { Declaration = CSharpToken.CloseBraceToken }, tokens[26]);
    }
}
