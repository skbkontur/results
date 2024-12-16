cd .\vostok.devtools\dotnetcementrefs
dotnet build -c Release
dotnet tool update --add-source nupkg -g dotnetcementrefs

cd %~dp0
dotnetcementrefs --source:https://api.nuget.org/v3/index.json --refPrefix:Kontur --ignoreMissingPackages --allowLocalProjects

dotnet pack Kontur.Results\Kontur.Results.csproj
dotnet nuget push .\Kontur.Results\bin\Release\*.nupkg  --api-key %nugetapikey% --source https://api.nuget.org/v3/index.json

dotnet pack Kontur.Results.Monad\Kontur.Results.Monad.csproj
dotnet nuget push .\Kontur.Results.Monad\bin\Release\*.nupkg  --api-key %nugetapikey% --source https://api.nuget.org/v3/index.json