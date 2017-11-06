$SrcRoot = "$PSScriptRoot\..\src"

# Run the server
dotnet restore "$SrcRoot\WebApiWithSpa.Api"
Start-Process 'dotnet' -WorkingDirectory "$SrcRoot\WebApiWithSpa.Api" -ArgumentList 'watch run'