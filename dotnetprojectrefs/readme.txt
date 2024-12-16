Installation for .NET Core 2.1+:

dotnet build

dotnet tool install --add-source nupkg -g dotnetprojectrefs

Usage examples:

dotnetprojectrefs - converts project references to nuget package references (with same version) for all solutions in current directory.

dotnetprojectrefs <module directory> - converts project references to nuget package references (with same version) for all solutions in <module directory>.