pushd %~dp0
dotnet build -c Release
dotnet tool update --add-source nupkg -g dotnetcementrefs
popd