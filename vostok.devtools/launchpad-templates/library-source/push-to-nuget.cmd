rmdir /S /Q nupkg
nuget pack pack.nuspec -OutputDirectory nupkg
nuget push nupkg\*.nupkg -Source https://api.nuget.org/v3/index.json -ApiKey %1