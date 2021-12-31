using System.Linq;
using GoTools.LanguageParser.Parser;
using GoTools.LanguageParser.Parser.CSharp;
using GoTools.LanguageParser.Parser.Exceptions;
using GoTools.LanguageParser.Tokenizer.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoTools.LanguageParser.Tests;

[TestClass]
public class CSharpParserTests : TestBase
{
    private static ModelAnalysisUnit CreateAst(string code)
        => new CSharpModelParser(TokenizeCSharp(code)).Parse();

    [TestMethod]
    public void ParsesSimpleModel()
    {
        var ast = CreateAst(@"
            using System;
            
            namespace GoTools.Testing
            {
                public class TestModel 
                {
                    public int TestProperty { get; set; }
                }
            }
        ");

        Assert.IsNotNull(ast);

        Assert.AreEqual("GoTools.Testing", ast.Scope);

        Assert.AreEqual(1, ast.Models!.Count);
        Assert.AreEqual("TestModel", ast.Models.First().Name);

        Assert.AreEqual(0, ast.Models.First().Children!.Count);
        Assert.AreEqual(1, ast.Models.First().Properties!.Count);

        Assert.AreEqual(
            new() { Name = "TestProperty", Type = "int", IsArray = false, IsNullable = false },
            ast.Models.First().Properties!.First());
    }

    [TestMethod]
    public void ParsesSimpleModel_WithConstructor()
    {
        var ast = CreateAst(@"
            using System;
            
            namespace GoTools.Testing
            {
                public class TestModel 
                {
                    public TestModel()
                    {
                        TestProperty = 7;
                    }

                    public int TestProperty { get; set; }
                }
            }
        ");

        Assert.IsNotNull(ast);

        Assert.AreEqual("GoTools.Testing", ast.Scope);

        Assert.AreEqual(1, ast.Models!.Count);
        Assert.AreEqual("TestModel", ast.Models.First().Name);

        Assert.AreEqual(0, ast.Models.First().Children!.Count);
        Assert.AreEqual(1, ast.Models.First().Properties!.Count);

        Assert.AreEqual(
            new() { Name = "TestProperty", Type = "int", IsArray = false, IsNullable = false },
            ast.Models.First().Properties!.First());
    }

    [TestMethod]
    public void ParsesSimpleModel_WithVirtualKeyword()
    {
        var ast = CreateAst(@"
            using System;
            
            namespace GoTools.Testing
            {
                public class TestModel 
                {
                    public virtual int TestProperty { get; set; }
                }
            }
        ");

        Assert.IsNotNull(ast);

        Assert.AreEqual("GoTools.Testing", ast.Scope);

        Assert.AreEqual(1, ast.Models!.Count);
        Assert.AreEqual("TestModel", ast.Models.First().Name);

        Assert.AreEqual(0, ast.Models.First().Children!.Count);
        Assert.AreEqual(1, ast.Models.First().Properties!.Count);

        Assert.AreEqual(
            new() { Name = "TestProperty", Type = "int", IsArray = false, IsNullable = false },
            ast.Models.First().Properties!.First());
    }

    [TestMethod]
    public void ParsesSimpleRecord()
    {
        var ast = CreateAst(@"
            using System;
            
            namespace GoTools.Testing;
            
            public record TestModel 
            {
                public int TestProperty { get; init; }
                public string TestString { get; init; }
            }
        ");

        Assert.IsNotNull(ast);

        Assert.AreEqual("GoTools.Testing", ast.Scope);

        Assert.AreEqual(1, ast.Models!.Count);
        Assert.AreEqual("TestModel", ast.Models.First().Name);

        Assert.AreEqual(0, ast.Models.First().Children!.Count);
        Assert.AreEqual(2, ast.Models.First().Properties!.Count);

        Assert.AreEqual(
            new() { Name = "TestProperty", Type = "int", IsArray = false, IsNullable = false },
            ast.Models.First().Properties!.ElementAt(0));

        Assert.AreEqual(
            new() { Name = "TestString", Type = "string", IsArray = false, IsNullable = false },
            ast.Models.First().Properties!.ElementAt(1));
    }

    [TestMethod]
    public void ParsesSimpleRecord_WithNullableProperty()
    {
        var ast = CreateAst(@"
            using System;
            
            namespace GoTools.Testing;
            
            public record TestModel 
            {
                public int TestProperty { get; init; }
                public string? NullableString { get; init; }
            }
        ");

        Assert.IsNotNull(ast);

        Assert.AreEqual("GoTools.Testing", ast.Scope);

        Assert.AreEqual(1, ast.Models!.Count);
        Assert.AreEqual("TestModel", ast.Models.First().Name);

        Assert.AreEqual(0, ast.Models.First().Children!.Count);
        Assert.AreEqual(2, ast.Models.First().Properties!.Count);

        Assert.AreEqual(
            new() { Name = "TestProperty", Type = "int", IsArray = false, IsNullable = false },
            ast.Models.First().Properties!.ElementAt(0));

        Assert.AreEqual(
            new() { Name = "NullableString", Type = "string", IsArray = false, IsNullable = true },
            ast.Models.First().Properties!.ElementAt(1));
    }

    [TestMethod]
    public void ParsesSimpleRecord_WithArrayProperty()
    {
        var ast = CreateAst(@"
            using System;
            
            namespace GoTools.Testing;
            
            public record TestModel 
            {
                public int[] TestArray { get; init; }
            }
        ");

        Assert.IsNotNull(ast);

        Assert.AreEqual("GoTools.Testing", ast.Scope);

        Assert.AreEqual(1, ast.Models!.Count);
        Assert.AreEqual("TestModel", ast.Models.First().Name);

        Assert.AreEqual(0, ast.Models.First().Children!.Count);
        Assert.AreEqual(1, ast.Models.First().Properties!.Count);

        Assert.AreEqual(
            new() { Name = "TestArray", Type = "int", IsArray = true, IsNullable = false },
            ast.Models.First().Properties!.ElementAt(0));
    }

    [TestMethod]
    public void ParsesSimpleRecord_WithNullableArrayProperty()
    {
        var ast = CreateAst(@"
            using System;
            
            namespace GoTools.Testing;
            
            public record TestModel 
            {
                public int[]? TestNullableArray { get; init; }
            }
        ");

        Assert.IsNotNull(ast);

        Assert.AreEqual("GoTools.Testing", ast.Scope);

        Assert.AreEqual(1, ast.Models!.Count);
        Assert.AreEqual("TestModel", ast.Models.First().Name);

        Assert.AreEqual(0, ast.Models.First().Children!.Count);
        Assert.AreEqual(1, ast.Models.First().Properties!.Count);

        Assert.AreEqual(
            new() { Name = "TestNullableArray", Type = "int", IsArray = true, IsNullable = true },
            ast.Models.First().Properties!.ElementAt(0));
    }

    [TestMethod]
    public void ParsesComplexNestedModel()
    {
        var ast = CreateAst(@"
            using System;
            
            namespace GoTools.Testing;
            
            public class TestModel 
            {
                public int[] TestArray { get; init; }
                public NestedModel[]? TestNestedModel { get; }
        
                public class NestedModel
                {
                    public int _testField;
                }
            }
        ");

        Assert.IsNotNull(ast);

        Assert.AreEqual("GoTools.Testing", ast.Scope);

        Assert.AreEqual(1, ast.Models!.Count);
        Assert.AreEqual("TestModel", ast.Models.First().Name);

        Assert.AreEqual(1, ast.Models.First().Children!.Count);
        
        var childModel = ast.Models.First().Children!.First();
        Assert.AreEqual("NestedModel", childModel.Name);

        Assert.AreEqual(2, ast.Models.First().Properties!.Count);

        Assert.AreEqual(
            new() { Name = "TestArray", Type = "int", IsArray = true, IsNullable = false },
            ast.Models.First().Properties!.ElementAt(0));

        Assert.AreEqual(
            new() { Name = "TestNestedModel", Type = "NestedModel", IsArray = true, IsNullable = true },
            ast.Models.First().Properties!.ElementAt(1));
    }

    [TestMethod]
    public void ParsesFileScopedNamespace()
    {
        var ast = CreateAst(@"
            using System;
            
            namespace GoTools.Testing;

            public class TestModel 
            {
                public int TestProperty { get; init; }
            }
        ");

        Assert.IsNotNull(ast);

        Assert.AreEqual("GoTools.Testing", ast.Scope);

        Assert.AreEqual(1, ast.Models!.Count);
        Assert.AreEqual("TestModel", ast.Models.First().Name);

        Assert.AreEqual(0, ast.Models.First().Children!.Count);
        Assert.AreEqual(1, ast.Models.First().Properties!.Count);

        Assert.AreEqual(
            new() { Name = "TestProperty", Type = "int", IsArray = false, IsNullable = false },
            ast.Models.First().Properties!.First());
    }

    [TestMethod]
    public void ParsesGetOnlyProperty()
    {
        var ast = CreateAst(@"
            using System;
            
            namespace GoTools.Testing;

            public class TestModel 
            {
                public int TestProperty { get; }
            }
        ");

        Assert.IsNotNull(ast);

        Assert.AreEqual("GoTools.Testing", ast.Scope);

        Assert.AreEqual(1, ast.Models!.Count);
        Assert.AreEqual("TestModel", ast.Models.First().Name);

        Assert.AreEqual(0, ast.Models.First().Children!.Count);
        Assert.AreEqual(1, ast.Models.First().Properties!.Count);

        Assert.AreEqual(
            new() { Name = "TestProperty", Type = "int", IsArray = false, IsNullable = false },
            ast.Models.First().Properties!.First());
    }

    [TestMethod]
    [ExpectedException(typeof(SyntaxErrorException<CSharpToken>))]
    public void ThrowsSyntaxError_NoPropertyGetter()
    {
        try
        {
            CreateAst(@"
                using System;
                
                public class TestModel
                {
                    public int TestProperty { set; }
                }
            ");
        }
        catch (SyntaxErrorException<CSharpToken> exception)
        {
            Assert.AreEqual(CSharpToken.PropertySetAccessorKeyword, exception.ActualToken);
            Assert.AreEqual(CSharpToken.PropertyGetAccessorKeyword, exception.ExpectedTokens[0]);

            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(SyntaxErrorException<CSharpToken>))]
    public void ThrowsSyntaxError_MissingOpenBrace()
    {
        try
        {
            CreateAst(@"
                using System;
                
                public class TestModel

                    public int TestProperty { get; set; }
                }
            ");
        }
        catch (SyntaxErrorException<CSharpToken> exception)
        {
            Assert.AreEqual(CSharpToken.PublicKeyword, exception.ActualToken);
            Assert.AreEqual(CSharpToken.OpenBraceToken, exception.ExpectedTokens[0]);

            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(SyntaxErrorException<CSharpToken>))]
    public void ThrowsSyntaxError_InvalidPropertyName()
    {
        try
        {
            CreateAst(@"
                using System;
                
                public class TestModel
                {
                    public int init { get; set; }
                }
            ");
        }
        catch (SyntaxErrorException<CSharpToken> exception)
        {
            Assert.AreEqual(CSharpToken.PropertyInitAccessorKeyword, exception.ActualToken);
            Assert.AreEqual(CSharpToken.Symbol, exception.ExpectedTokens[0]);

            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(DuplicateIdentifierException))]
    public void ThrowsSyntaxError_DuplicatePropertyName()
    {
        CreateAst(@"
            using System;
            
            public class TestModel
            {
                public int TestProperty { get; set; }
                public string TestProperty;
            }
        ");
    }
}
