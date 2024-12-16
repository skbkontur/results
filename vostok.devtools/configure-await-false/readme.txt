Installation for .NET Core 2.1+:

dotnet build -c Release

dotnet tool install --add-source nupkg -g configureawaitfalse

Usage example:

configureawaitfalse <directory>
	Recursively scans all *.cs files in given <directory> and prints any 'await' expressions 
	without ConfigureAwait(false). Fails in the end if any incorrect awaits were found. 

