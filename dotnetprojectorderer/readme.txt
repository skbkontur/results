Installation for .NET Core 2.1+:

dotnet build

dotnet tool install --add-source nupkg -g dotnetprojectorderer

Usage examples:

dotnetprojectorderer - return projects in topological sort order relative to their dependencies.

dotnetprojectorderer <module directory> - return projects in topological sort order relative to their dependencies in <module directory>.