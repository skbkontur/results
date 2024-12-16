using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public static class Program
{
    private const string TaskCompletionSourceIdentifier = "TaskCompletionSource";

    private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(LanguageVersion.Latest, DocumentationMode.None);

    public static void Main(string[] args)
    {
        var failedResults = 0;

        foreach (var directory in args.Select(arg => new DirectoryInfo(arg)))
        {
            if (!directory.Exists)
                continue;
            
            Console.Out.WriteLine($"Checking '{directory.FullName}'..");

            foreach (var file in directory.EnumerateFiles("*.cs", SearchOption.AllDirectories))
            {
                foreach (var result in Check(File.ReadAllText(file.FullName)))
                {
                    if (!result.HasTaskCreationOptionsArgument)
                    {
                        Console.Out.WriteLine("Error: 'TaskCompletionSource' creation without configured 'TaskCreationOptions.RunContinuationsAsynchronously' in file '{0}' at line {1}.", file.Name, result.Location.GetMappedLineSpan().StartLinePosition.Line);

                        failedResults++;
                    }
                }
            }
        }

        var message = $"\n{failedResults} 'TaskCompletionSource' creations without configured 'TaskCreationOptions.RunContinuationsAsynchronously' were found.";
        if (failedResults > 0)
            //Console.WriteLine(message);
            throw new Exception(message);
    }

    private static IEnumerable<CheckResult> Check(string sourceCode)
    {
        var aliases = new List<string>() {TaskCompletionSourceIdentifier};
        var tree = CSharpSyntaxTree.ParseText(sourceCode, ParseOptions);

        foreach (var item in tree.GetRoot().DescendantNodesAndTokens())
        {
            if (item.IsKind(SyntaxKind.UsingDirective))
            {
                TryAddAlias((UsingDirectiveSyntax) item, aliases);
            }

            if (item.IsKind(SyntaxKind.ObjectCreationExpression))
            {
                var creation = (ObjectCreationExpressionSyntax) item.AsNode();

                if (!IsTaskCompletionSourceCreation(creation, aliases))
                    continue;

                yield return new CheckResult(HasTaskCreationOptionsArgument(creation.ArgumentList), creation.GetLocation());
            }
        }
    }

    private static void TryAddAlias(UsingDirectiveSyntax item, List<string> aliases)
    {
        if (item.Alias == null)
            return;

        var name = item.Alias.Name.Identifier.Text;
        if (!(item.Name is QualifiedNameSyntax nameSyntax))
            return;

        if (IsTaskCompletionSourceIdentifier(nameSyntax.Right.Identifier))
            aliases.Add(name);
    }

    private static bool IsTaskCompletionSourceCreation(ObjectCreationExpressionSyntax creation, List<string> aliases)
    {
        if (creation.Type is GenericNameSyntax generic)
            return IsTaskCompletionSourceIdentifier(generic.Identifier);

        if (creation.Type is IdentifierNameSyntax nameSyntax)
            return aliases.Contains(nameSyntax.Identifier.Text);

        return false;
    }

    private static bool IsTaskCompletionSourceIdentifier(SyntaxToken token) =>
        token.Text == TaskCompletionSourceIdentifier;

    private static bool HasTaskCreationOptionsArgument(ArgumentListSyntax argumentList)
    {
        foreach (var arg in argumentList.Arguments)
        {
            var str = arg.Expression.ToString();
            if (str.Contains("TaskCreationOptions"))
                return true;
        }

        return false;
    }

    private class CheckResult
    {
        public CheckResult(bool hasTaskCreationOptionsArgument, Location location)
        {
            HasTaskCreationOptionsArgument = hasTaskCreationOptionsArgument;
            Location = location;
        }

        public bool HasTaskCreationOptionsArgument { get; }

        public Location Location { get; }
    }
}