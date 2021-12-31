using System;

namespace GoTools.LanguageParser.Parser.Exceptions
{
    public class DuplicateIdentifierException : Exception
    {
        public DuplicateIdentifierException(string? duplicateIdentifier)
            : base($"Duplicate identifier {duplicateIdentifier}") { }
    }
}
