cd ..
call cm get vostok.devtools
cd vostok.devtools\dotnetcementrefs
dotnet build -c Release
dotnet tool update --add-source nupkg -g dotnetcementrefs
cd %~dp0
dotnetcementrefs --source:https://api.nuget.org/v3/index.json --refPrefix:Kontur --ignoreMissingPackages --allowLocalProjects

dotnet pack Kontur.Results\Kontur.Results.csproj
dotnet nuget push .\bin\Kontur.Results\*.nupkg  --api-key %nugetapikey% --source https://api.nuget.org/v3/index.json

dotnet pack Kontur.Results.Monad\Kontur.Results.Monad.csproj
dotnet nuget push .\bin\Kontur.Results.Monad\*.nupkg  --api-key %nugetapikey% --source https://api.nuget.org/v3/index.json