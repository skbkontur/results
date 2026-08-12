using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class SwitchTierGenerator
    {
        internal static SwitchTier[] Create(
            ClassNamesSwitch classNames,
            SwitchParameterForm nextPassForm,
            IReadOnlyCollection<SwitchParameterForm> nextFactoryForms,
            SwitchParameterForm otherPassForm,
            IReadOnlyCollection<SwitchParameterForm> otherFactoryForms)
        {
            var factoryTiers = otherFactoryForms
                .Select(otherForm => new SwitchTier(
                    classNames.Factory,
                    classNames.FactoryNameSyntax(),
                    nextPassForm,
                    otherForm,
                    CreateArgument(nextPassForm.ParameterName),
                    CreateFactoryArgument(otherForm.ParameterName),
                    classNames.PassNameSyntax()));

            var factoryNextTiers = nextFactoryForms
                .Select(nextForm => new SwitchTier(
                    classNames.FactoryNext,
                    classNames.FactoryNextNameSyntax(),
                    nextForm,
                    otherPassForm,
                    CreateFactoryArgument(nextForm.ParameterName),
                    CreateArgument(otherPassForm.ParameterName),
                    classNames.PassNameSyntax()));

            var factoryBothTiers = nextFactoryForms
                .SelectMany(nextForm => otherFactoryForms
                    .Select(otherForm => new SwitchTier(
                        classNames.FactoryBoth,
                        classNames.FactoryBothNameSyntax(),
                        nextForm,
                        otherForm,
                        CreateFactoryArgument(nextForm.ParameterName),
                        CreateArgument(otherForm.ParameterName),
                        classNames.FactoryNameSyntax())));

            return factoryTiers
                .Concat(factoryNextTiers)
                .Concat(factoryBothTiers)
                .ToArray();
        }

        private static ExpressionSyntax CreateArgument(SyntaxToken parameterName)
        {
            return SyntaxFactory.IdentifierName(parameterName);
        }

        private static ExpressionSyntax CreateFactoryArgument(SyntaxToken parameterName)
        {
            return SyntaxFactory.SimpleLambdaExpression(
                SyntaxFactory.Parameter(Identifiers.Discard),
                SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(parameterName)));
        }
    }
}
