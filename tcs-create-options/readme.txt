Installation for .NET Core 2.1+:

dotnet build -c Release

dotnet tool install --add-source nupkg -g tcscreateoptions

Usage example:

tcscreateoptions <directory>
	Recursively scans all *.cs files in given <directory> and prints any 'TaskCompletionSource' creations 
	without configured 'TaskCreationOptions'. Fails in the end if any incorrect creations were found. 

