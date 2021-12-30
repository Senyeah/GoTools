using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoTools.LanguageParser.CodeGeneration;
using GoTools.LanguageParser.Parser.CSharp;
using GoTools.LanguageParser.Tokenizer.CSharp;
using GoTools.LanguageParser.Tokenizer.TypeScript;

namespace GoTools.LanguageParser;

public static class Program
{
    private static void ParseCSharp(Stream file)
    {
        using var tokenizer = new CSharpTokenizer(file);

        var parser = new CSharpModelParser(tokenizer.Tokenize());
        var modelGenerator = new TypeScriptModelGenerator();

        var output = modelGenerator.GenerateModel(parser.Parse());

        using (var outputFile = File.OpenWrite(@"C:\Users\jack.greenhill\Desktop\out.ts"))
        {
            outputFile.Write(Encoding.UTF8.GetBytes(output));
        }

        new Process
        {
            StartInfo = new(@"C:\Users\jack.greenhill\Desktop\out.ts")
            {
                UseShellExecute = true
            }
        }.Start();
    }

    private static void ParseTypeScript(Stream file)
    {
        using var tokenizer = new TypeScriptTokenizer(file);

        var depth = 0;

        foreach (var token in tokenizer.Tokenize())
        {
            depth = token switch
            {
                { Declaration: TypeScriptToken.OpenBraceToken } => depth + 4,
                { Declaration: TypeScriptToken.CloseBraceToken } => Math.Max(0, depth - 4),
                _ => depth
            };

            if (token is { Declaration: TypeScriptToken.OpenBraceToken or TypeScriptToken.CloseBraceToken })
            {
                continue;
            }

            var prefix = string.Join("", from _ in Enumerable.Range(0, depth) select " ");

            Console.WriteLine($"TS: {prefix}{token.Declaration} {token.Identifier ?? ""}", token.Declaration, token.Identifier);
        }
    }

    public static async Task Main(string[] args)
    {
        await using var file = File.OpenRead(
            @"C:\Users\jack.greenhill\Desktop\Repos\Consignly\Consignly.Portal.Server\Models\ConsignmentImports\ConsignmentImportDetailViewModel.cs"
        );

        if (file.Name.EndsWith("cs"))
        {
            ParseCSharp(file);
        }
        else
        {
            ParseTypeScript(file);
        }

        Console.ReadKey();
    }
}