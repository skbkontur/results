using System.Collections.Generic;
using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Public;
using Microsoft.CodeAnalysis;

namespace Kontur.Results.SourceGenerator.Code
{
    internal record SwitchParameterForm(
        SyntaxToken ParameterName,
        IReadOnlyCollection<InternalTypeParameter> InternalTypes,
        IReadOnlyCollection<MethodTypeGenerator.ParameterInfo> PublicTypes);
}
