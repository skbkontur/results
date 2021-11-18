using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class TypeFactory
    {
        internal const string Optional = "Optional";
        internal const string Result = "Result";
        internal const string ResultFailure = "ResultFailure";
        private const string InterfaceOptional = "IOptional";
        private const string InterfaceResult = "IResult";

        internal static GenericNameSyntax CreateValueTask(TypeSyntax returnType)
        {
            return CreateClosedGeneric(Identifiers.ValueTaskIdentifier, returnType);
        }

        internal static GenericNameSyntax CreateTask(TypeSyntax returnType)
        {
            return CreateClosedGeneric(Identifiers.TaskIdentifier, returnType);
        }

        internal static GenericNameSyntax CreateInterfaceOptional(TypeSyntax fault)
        {
            return CreateClosedGeneric(SyntaxFactory.Identifier(InterfaceOptional), fault);
        }

        internal static GenericNameSyntax CreateInterfaceResult(TypeSyntax fault)
        {
            return CreateClosedGeneric(SyntaxFactory.Identifier(InterfaceResult), fault);
        }

        internal static GenericNameSyntax CreateInterfaceResult(TypeSyntax fault, TypeSyntax value)
        {
            return CreateClosedGeneric(SyntaxFactory.Identifier(InterfaceResult), fault, value);
        }

        internal static GenericNameSyntax CreateOptional(TypeSyntax fault)
        {
            return CreateClosedGeneric(SyntaxFactory.Identifier(Optional), fault);
        }

        internal static GenericNameSyntax CreateResult(TypeSyntax fault)
        {
            return CreateClosedGeneric(SyntaxFactory.Identifier(Result), fault);
        }

        internal static GenericNameSyntax CreateResult(TypeSyntax fault, TypeSyntax value)
        {
            return CreateClosedGeneric(SyntaxFactory.Identifier(Result), fault, value);
        }

        internal static GenericNameSyntax CreateResultFailure(TypeSyntax fault)
        {
            return CreateClosedGeneric(SyntaxFactory.Identifier(ResultFailure), fault);
        }

        internal static GenericNameSyntax CreateClosedGeneric(SyntaxToken typeName, TypeSyntax argument, params TypeSyntax[] otherArguments)
        {
            return SyntaxFactory.GenericName(typeName, SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(otherArguments.Prepend(argument))));
        }
    }
}