using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class ReturnTypeCalculator
    {
        internal static TypeSyntax Get(TypeSyntax returnType, bool hasTask, bool hasValueTask)
        {
            if (hasTask)
            {
                return TypeFactory.CreateTask(returnType);
            }

            return hasValueTask
                ? TypeFactory.CreateValueTask(returnType)
                : returnType;
        }
    }
}
