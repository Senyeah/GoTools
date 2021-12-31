using System.Collections.Generic;
using System.Linq;
using GoTools.LanguageParser.ParsedToken;
using GoTools.LanguageParser.Parser.Exceptions;
using GoTools.LanguageParser.Parser.TokenConsumer;
using GoTools.LanguageParser.Tokenizer.CSharp;

namespace GoTools.LanguageParser.Parser.CSharp;

public sealed class CSharpModelParser : IModelParser
{
    private readonly ITokenConsumer<CSharpToken> _tokenConsumer;

    public CSharpModelParser(IEnumerable<IParsedToken<CSharpToken>> tokens)
    {
        _tokenConsumer = new TokenConsumer<CSharpToken>(CSharpToken.Symbol, tokens.Reverse());
    }

    public ModelAnalysisUnit Parse()
    {
        var scope = default(string?);
        var isFileScopedNamespace = false;

        var modelNames = new HashSet<string>();
        var models = new HashSet<ModelAnalysisUnit.ModelDeclaration>();

        while (_tokenConsumer.IsConsumable(CSharpToken.UsingKeyword))
        {
            _tokenConsumer.Consume(CSharpToken.UsingKeyword);
            _tokenConsumer.ConsumeSymbol();
            _tokenConsumer.Consume(CSharpToken.SemicolonToken);
        }

        if (_tokenConsumer.IsConsumable(CSharpToken.NamespaceDeclaration))
        {
            _tokenConsumer.Consume(CSharpToken.NamespaceDeclaration);

            scope = _tokenConsumer.ConsumeSymbol();
            isFileScopedNamespace = _tokenConsumer.TryConsume(CSharpToken.SemicolonToken);

            if (!isFileScopedNamespace)
            {
                _tokenConsumer.Consume(CSharpToken.OpenBraceToken);
            }
        }

        while (_tokenConsumer.IsConsumable(CSharpToken.PublicKeyword))
        {
            var model = ParseClassDeclaration();

            if (modelNames.Contains(model.Name!))
            {
                throw new DuplicateIdentifierException(model.Name);
            }

            modelNames.Add(model.Name!);
            models.Add(model);
        }

        if (!isFileScopedNamespace)
        {
            _tokenConsumer.Consume(CSharpToken.CloseBraceToken);
        }

        return new()
        {
            Scope = scope,
            Models = models
        };
    }

    private void ParseAndDiscardMethod()
    {
        _tokenConsumer.Consume(CSharpToken.PublicKeyword, CSharpToken.PrivateKeyword);

        // Type info/method name
        _tokenConsumer.ConsumeUntil(CSharpToken.OpenBracketToken);

        // Argument list
        _tokenConsumer.Consume(CSharpToken.OpenBracketToken);
        _tokenConsumer.ConsumeUntil(CSharpToken.CloseBracketToken);
        _tokenConsumer.Consume(CSharpToken.CloseBracketToken);

        // Method body
        _tokenConsumer.Consume(CSharpToken.OpenBraceToken);

        // Consume everything inside method body until opening brace is closed
        var openBraceCount = 1;

        while (openBraceCount > 0)
        {
            if (_tokenConsumer.TryConsume(CSharpToken.OpenBraceToken))
            {
                openBraceCount += 1;
            }
            else if (_tokenConsumer.TryConsume(CSharpToken.CloseBraceToken))
            {
                openBraceCount -= 1;
            }
            else
            {
                _tokenConsumer.Consume();
            }
        }
    }

    private ModelAnalysisUnit.ModelDeclaration ParseClassDeclaration()
    {
        _tokenConsumer.Consume(CSharpToken.PublicKeyword);

        // abstract/sealed/partial etc
        while (!_tokenConsumer.IsAnyConsumable(CSharpToken.ClassDeclaration, CSharpToken.RecordDeclaration))
        {
            _tokenConsumer.Consume();
        }

        _tokenConsumer.Consume(CSharpToken.ClassDeclaration, CSharpToken.RecordDeclaration);

        // Class name
        var modelName = _tokenConsumer.ConsumeSymbol();

        // Implemented interfaces
        if (_tokenConsumer.IsConsumable(CSharpToken.ColonToken))
        {
            _tokenConsumer.Consume(CSharpToken.ColonToken);
            _tokenConsumer.ConsumeUntil(CSharpToken.OpenBraceToken);
        }

        _tokenConsumer.Consume(CSharpToken.OpenBraceToken);

        var propertyNames = new HashSet<string>();
        var properties = new HashSet<ModelAnalysisUnit.ModelDeclaration.PropertyDeclaration>();

        var childNames = new HashSet<string>();
        var children = new HashSet<ModelAnalysisUnit.ModelDeclaration>();

        while (_tokenConsumer.IsAnyConsumable(CSharpToken.PublicKeyword, CSharpToken.PrivateKeyword))
        {
            if (_tokenConsumer.IsAnyConsumableAhead(1, CSharpToken.ClassDeclaration, CSharpToken.RecordDeclaration))
            {
                var childModel = ParseClassDeclaration();

                if (childNames.Contains(childModel.Name!))
                {
                    throw new DuplicateIdentifierException(childModel.Name);
                }

                childNames.Add(childModel.Name!);
                children.Add(childModel);
            }
            else if (_tokenConsumer.IsConsumableAhead(1, CSharpToken.Symbol, CSharpToken.OpenBracketToken)
                        || _tokenConsumer.IsConsumableAhead(1, CSharpToken.Symbol, CSharpToken.Symbol, CSharpToken.OpenBracketToken))
            {
                // Constructor/method: consume symbols but emit no metadata
                ParseAndDiscardMethod();
            }
            else
            {
                // Property
                var property = ParsePropertyDeclaration();

                if (propertyNames.Contains(property.Name!))
                {
                    throw new DuplicateIdentifierException(property.Name);
                }

                propertyNames.Add(property.Name!);
                properties.Add(property);
            }
        }

        _tokenConsumer.Consume(CSharpToken.CloseBraceToken);

        return new()
        {
            Name = modelName,
            Children = children,
            Properties = properties
        };
    }

    private ModelAnalysisUnit.ModelDeclaration.PropertyDeclaration ParsePropertyDeclaration()
    {
        _tokenConsumer.Consume(CSharpToken.PublicKeyword, CSharpToken.PrivateKeyword);

        // readonly etc
        _tokenConsumer.ConsumeUntil(CSharpToken.Symbol);

        var type = _tokenConsumer.ConsumeSymbol();
        var isArray = _tokenConsumer.IsConsumable(CSharpToken.OpenSquareBracketToken);

        if (isArray)
        {
            _tokenConsumer.Consume(CSharpToken.OpenSquareBracketToken);
            _tokenConsumer.Consume(CSharpToken.CloseSquareBracketToken);
        }

        var isNullable = _tokenConsumer.TryConsume(CSharpToken.QuestionMarkToken);
        var name = _tokenConsumer.ConsumeSymbol();

        // Expression-bodied properties
        if (_tokenConsumer.IsConsumable(CSharpToken.LambdaBodyToken))
        {
            // Lambda body
            _tokenConsumer.Consume(CSharpToken.LambdaBodyToken);
            _tokenConsumer.ConsumeUntil(CSharpToken.SemicolonToken);
            _tokenConsumer.Consume(CSharpToken.SemicolonToken);
        }

        // Getter/setter
        if (_tokenConsumer.IsConsumable(CSharpToken.OpenBraceToken))
        {
            _tokenConsumer.Consume(CSharpToken.OpenBraceToken);
            _tokenConsumer.Consume(CSharpToken.PropertyGetAccessorKeyword);
            _tokenConsumer.Consume(CSharpToken.SemicolonToken);

            if (_tokenConsumer.IsConsumable(CSharpToken.PropertySetAccessorKeyword))
            {
                _tokenConsumer.Consume(CSharpToken.PropertySetAccessorKeyword);
                _tokenConsumer.Consume(CSharpToken.SemicolonToken);
            }
            else if (_tokenConsumer.IsConsumable(CSharpToken.PropertyInitAccessorKeyword))
            {
                _tokenConsumer.Consume(CSharpToken.PropertyInitAccessorKeyword);
                _tokenConsumer.Consume(CSharpToken.SemicolonToken);
            }

            _tokenConsumer.Consume(CSharpToken.CloseBraceToken);
        }

        // Field
        if (_tokenConsumer.IsConsumable(CSharpToken.SemicolonToken))
        {
            _tokenConsumer.Consume(CSharpToken.SemicolonToken);
        }

        return new()
        {
            Type = type,
            Name = name,
            IsArray = isArray,
            IsNullable = isNullable
        };
    }
}