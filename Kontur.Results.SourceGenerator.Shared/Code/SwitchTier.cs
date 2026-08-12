using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal record SwitchTier(
        string ClassName,
        SimpleNameSyntax ClassNameSyntax,
        SwitchParameterForm NextForm,
        SwitchParameterForm OtherForm,
        ExpressionSyntax NextArgument,
        ExpressionSyntax OtherArgument,
        SimpleNameSyntax DelegateTarget);
}
