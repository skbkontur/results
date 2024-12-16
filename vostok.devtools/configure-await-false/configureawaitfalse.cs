using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public static class Program
{
    private const string ConfigureAwaitIdentifier = "ConfigureAwait";

    private static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(LanguageVersion.Latest, DocumentationMode.None);

    public static void Main(string[] args)
    {
        var failedResults = args.Select(arg => new DirectoryInfo(arg)).Select(info =>
        {
            Console.Out.WriteLine($"Checking '{info.FullName}'..");
            return Check(info);
        }).Sum();
        if (failedResults > 0)
            throw new Exception($"{failedResults} await(s) without 'ConfigureAwait(false)' were found.");
    }

    private static int Check(DirectoryInfo directory)
    {
        var failedResults = 0;

        if (!directory.Exists || directory.EnumerateFiles(".skip-configure-await-checks").Any())
            return failedResults;

        foreach (var file in directory.EnumerateFiles("*.cs", SearchOption.TopDirectoryOnly))
        {
            foreach (var result in Check(File.ReadAllText(file.FullName)))
            {
                if (!result.HasConfigureAwaitFalse)
                {
                    Console.Out.WriteLine($"Error: missing 'ConfigureAwait(false)' in file '{file.Name}' at line {result.Location.GetMappedLineSpan().StartLinePosition.Line}.");

                    failedResults++;
                }
            }
        }

        foreach (var subDirectory in directory.EnumerateDirectories())
            failedResults += Check(subDirectory);

        return failedResults;
    }

    private static IEnumerable<CheckResult> Check(string sourceCode)
    {
        var tree = CSharpSyntaxTree.ParseText(sourceCode, ParseOptions);

        foreach (var item in tree.GetRoot().DescendantNodesAndTokens())
        {
            if (item.IsKind(SyntaxKind.AwaitExpression))
            {
                var awaitNode = (AwaitExpressionSyntax) item.AsNode();
                
                if (IsYieldAwaitable(awaitNode))
                    continue;

                yield return CheckNode(awaitNode);
            }
        }
    }

    private static bool IsYieldAwaitable(AwaitExpressionSyntax awaitNode)
    {
        if (!(awaitNode.Expression is InvocationExpressionSyntax invocation))
            return false;

        if (!(invocation.Expression is MemberAccessExpressionSyntax memberAccess))
            return false;

        var code = memberAccess.ToString();

        return code.EndsWith(".Task.Yield", StringComparison.Ordinal) ||
               code.Equals("Task.Yield", StringComparison.Ordinal);
    }

    private static CheckResult CheckNode(AwaitExpressionSyntax awaitNode)
    {
        var possibleConfigureAwait = FindExpressionForConfigureAwait(awaitNode);
        var good = possibleConfigureAwait != null && IsConfigureAwait(possibleConfigureAwait.Expression) && HasFalseArgument(possibleConfigureAwait.ArgumentList);
        return new CheckResult(good, awaitNode.GetLocation());
    }

    private static InvocationExpressionSyntax FindExpressionForConfigureAwait(SyntaxNode node)
    {
        foreach (var item in node.ChildNodes())
        {
            if (item is InvocationExpressionSyntax syntax)
                return syntax;

            return FindExpressionForConfigureAwait(item);
        }
        return null;
    }

    private static bool IsConfigureAwait(ExpressionSyntax expression)
    {
        if (!(expression is MemberAccessExpressionSyntax memberAccess))
            return false;

        if (!memberAccess.Name.Identifier.Text.Equals(ConfigureAwaitIdentifier, StringComparison.Ordinal))
            return false;

        return true;
    }

    private static bool HasFalseArgument(ArgumentListSyntax argumentList)
    {
        if (argumentList.Arguments.Count != 1)
            return false;

        if (!argumentList.Arguments[0].Expression.IsKind(SyntaxKind.FalseLiteralExpression))
            return false;

        return true;
    }

    private class CheckResult
    {
        public CheckResult(bool hasConfigureAwaitFalse, Location location)
        {
            HasConfigureAwaitFalse = hasConfigureAwaitFalse;
            Location = location;
        }

        public bool HasConfigureAwaitFalse { get; }

        public Location Location { get; }
    }
}
