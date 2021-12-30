using System.Text;
using GoTools.LanguageParser.Parser;

namespace GoTools.LanguageParser.CodeGeneration;

public class TypeScriptModelGenerator : IModelGenerator
{
    private int _indentationDepth;

    private readonly StringBuilder _stringBuilder = new();
    private readonly Dictionary<string, string[]> _declaredTypes = new();

    private string GetCurrentIndentation()
        => string.Join(string.Empty, Enumerable.Range(0, _indentationDepth).Select(_ => "    "));

    private string ResolveTypeName(string typeSymbol, int scopeDepth = 0)
    {
        var stringMappings = new HashSet<string>(new[]
        {
            "Guid",
            "string",
            "DateTime",
            "DateTimeOffset"
        });

        var numberMappings = new HashSet<string>(new[]
        {
            "int",
            "float",
            "double",
            "decimal"
        });

        var booleanMappings = new HashSet<string>(new[]
        {
            "bool"
        });

        if (_declaredTypes.ContainsKey(typeSymbol))
        {
            return string.Join(".", _declaredTypes[typeSymbol][scopeDepth..]);
        }

        if (stringMappings.Contains(typeSymbol))
        {
            return "string";
        }

        if (numberMappings.Contains(typeSymbol))
        {
            return "number";
        }

        if (booleanMappings.Contains(typeSymbol))
        {
            return "boolean";
        }

        return typeSymbol;
    }

    private void ResolveDeclaredTypes(ModelAnalysisUnit.ModelDeclaration model, IEnumerable<string> scopeHierarchy)
    {
        var nestedScope = scopeHierarchy.Append(model.Name!).ToArray();
        _declaredTypes[model.Name!] = nestedScope;

        foreach (var child in model.Children!)
        {
            ResolveDeclaredTypes(child, nestedScope);
        }
    }

    private void EmitPropertyDeclaration(ModelAnalysisUnit.ModelDeclaration.PropertyDeclaration property)
    {
        var nullableSuffix = property.IsNullable ? " | null" : string.Empty;
        var arraySuffix = property.IsArray ? "[]" : string.Empty;

        var camelCaseName = char.ToLowerInvariant(property.Name![0]) + property.Name[1..];
        var type = ResolveTypeName(property.Type!, _indentationDepth - 1);

        _stringBuilder.Append(GetCurrentIndentation());
        _stringBuilder.AppendLine($"public {camelCaseName}: {type}{arraySuffix}{nullableSuffix};");
    }

    private void EmitModelClass(ModelAnalysisUnit.ModelDeclaration model)
    {
        if (model.Properties?.Count == 0)
        {
            return;
        }

        _stringBuilder.Append(GetCurrentIndentation());
        _stringBuilder.AppendLine($"export class {model.Name} {{");

        _indentationDepth += 1;
        foreach (var property in model.Properties!)
        {
            EmitPropertyDeclaration(property);
        }
        _indentationDepth -= 1;

        _stringBuilder.Append(GetCurrentIndentation());
        _stringBuilder.AppendLine("}");
        _stringBuilder.AppendLine();

        if (model.Children?.Count > 0)
        {
            _stringBuilder.Append(GetCurrentIndentation());
            _stringBuilder.AppendLine($"export namespace {model.Name} {{");

            _indentationDepth += 1;
            foreach (var child in model.Children)
            {
                EmitModelClass(child);
            }
            _indentationDepth -= 1;

            _stringBuilder.Append(GetCurrentIndentation());
            _stringBuilder.AppendLine("}");
            _stringBuilder.AppendLine();
        }
    }

    public string GenerateModel(ModelAnalysisUnit ast)
    {
        foreach (var model in ast.Models!)
        {
            ResolveDeclaredTypes(model, Array.Empty<string>());
        }

        foreach (var model in ast.Models!)
        {
            EmitModelClass(model);
        }

        return _stringBuilder.ToString();
    }
}