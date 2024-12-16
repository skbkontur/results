rmdir /S /Q nupkg
dotnet build -c Release
nuget push nupkg\*.nupkg -Source https://api.nuget.org/v3/index.json -ApiKey %1