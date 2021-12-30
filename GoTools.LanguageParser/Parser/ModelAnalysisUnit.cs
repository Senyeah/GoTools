using System.Collections.Generic;

namespace GoTools.LanguageParser.Parser;

public record ModelAnalysisUnit
{
    public string? Scope { get; init; }
    public IReadOnlySet<ModelDeclaration>? Models { get; init; }

    public record ModelDeclaration
    {
        public string? Name { get; init; }
        public IReadOnlySet<PropertyDeclaration>? Properties { get; init; }
        public IReadOnlySet<ModelDeclaration>? Children { get; init; }

        public record PropertyDeclaration
        {
            public string? Name { get; init; }
            public string? Type { get; init; }
            public bool IsArray { get; init; }
            public bool IsNullable { get; init; }
        }
    }
}