. ./$PSScriptRoot\config.ps1

.$PSScriptRoot\nuget-pack

dotnet new -i "$TemplateRoot/scripts/$NugetPackage"