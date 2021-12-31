using GoTools.LanguageParser.Parser;

namespace GoTools.CodeGeneration;

public interface IModelGenerator
{
    string GenerateModel(ModelAnalysisUnit ast);
}