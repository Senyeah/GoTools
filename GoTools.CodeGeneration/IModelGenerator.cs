using GoTools.LanguageParser.Parser;

namespace GoTools.LanguageParser.CodeGeneration;

public interface IModelGenerator
{
    string GenerateModel(ModelAnalysisUnit ast);
}