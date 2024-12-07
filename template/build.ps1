Param ($Version = "1.0-dev")
$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

function Run-DotNet {
    ..\0install.ps1 run --batch --version 9.0.. https://apps.0install.net/dotnet/sdk.xml @args
    if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}
}

# Keep template and library version in-sync
Run-DotNet add MyServiceBroker.csproj package OpenServiceBroker.Server --version $Version
Run-DotNet build MyServiceBroker.csproj

# Generate template NuGet package
Run-DotNet restore .template.csproj
Run-DotNet msbuild .template.csproj /v:Quiet /t:Pack /p:Configuration=Release /p:Version=$Version

popd
