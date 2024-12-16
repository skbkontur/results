Installation for .NET Core 2.1+:

dotnet build

dotnet tool install --add-source nupkg -g dotnetversionsuffix

Usage example:

dotnetversionsuffix pre0000001 - sets given version suffix for all projects included in current directory's solutions.

dotnetversionsuffix pre0000001 vostok.devtools - sets given version suffix for all projects included in given directory's solutions.