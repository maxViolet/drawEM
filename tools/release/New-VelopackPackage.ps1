<#
.SYNOPSIS
Packs a Windows x64 publish directory with Velopack and names the installer drawEM-<version>-win-x64-Setup.exe.

.DESCRIPTION
Runs "dotnet vpk pack" into an empty -OutputDirectory with full packages only
(no portable ZIP: the release already has one; no deltas). Renames
<PackId>-win-Setup.exe and keeps the tool-generated update feed names. Writes
the full paths of the three downloads, one per line: the Setup.exe, the full
.nupkg, and releases.win.json. RELEASES (Squirrel migration only) and
assets.win.json (build metadata) stay in the folder but are not listed.

-PackCommand replaces "dotnet vpk" with a script, for tests.

.EXAMPLE
./tools/release/New-VelopackPackage.ps1 -PublishDirectory src/DrawEM.App/bin/Release/net8.0-windows/win-x64/publish -Version 1.2.3 -OutputDirectory velopack -PackId drawEM.Desktop -Icon src/DrawEM.App/Assets/drawEM.ico
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string] $PublishDirectory,

    [Parameter(Mandatory)]
    [string] $Version,

    [Parameter(Mandatory)]
    [string] $OutputDirectory,

    [Parameter(Mandatory)]
    [string] $PackId,

    [Parameter(Mandatory)]
    [string] $Icon,

    [string] $PackCommand = ''
)

$ErrorActionPreference = 'Stop'

# X.Y.Z from a release tag, or X.Y.Z-<prerelease> for the CI smoke build.
if ($Version -cnotmatch '^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(-[0-9A-Za-z]+(\.[0-9A-Za-z]+)*)?\z') {
    throw "Version '$Version' is not of the form X.Y.Z or X.Y.Z-prerelease."
}

$publish = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($PublishDirectory)
$output = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($OutputDirectory)
$iconPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Icon)
if (-not (Test-Path -LiteralPath $publish -PathType Container)) {
    throw "Publish directory '$publish' does not exist."
}
if ((Test-Path -LiteralPath $output) -and @(Get-ChildItem -LiteralPath $output -Force).Count -gt 0) {
    throw "Output directory '$output' is not empty; stale packages could be uploaded."
}

$packArguments = @(
    'pack', '--packId', $PackId, '--packVersion', $Version,
    '--packDir', $publish, '--mainExe', 'DrawEM.App.exe', '--packTitle', 'drawEM',
    '--icon', $iconPath,
    '--runtime', 'win-x64', '--channel', 'win', '--noPortable', '--outputDir', $output
)
if ($PackCommand -eq '') {
    dotnet tool restore | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "dotnet tool restore failed with exit code $LASTEXITCODE." }
    dotnet vpk @packArguments | Out-Host
}
else {
    & $PackCommand @packArguments | Out-Host
}
if ($LASTEXITCODE -ne 0) {
    throw "vpk pack failed with exit code $LASTEXITCODE."
}

$setup = Join-Path $output "$PackId-win-Setup.exe"
$nupkg = Join-Path $output "$PackId-$Version-full.nupkg"
$feed = Join-Path $output 'releases.win.json'
$missing = @($setup, $nupkg, $feed | Where-Object { -not (Test-Path -LiteralPath $_ -PathType Leaf) } |
    ForEach-Object { [IO.Path]::GetFileName($_) })
if ($missing.Count -gt 0) {
    throw "vpk pack did not write: $($missing -join ', ')."
}

$installer = Join-Path $output "drawEM-$Version-win-x64-Setup.exe"
Move-Item -LiteralPath $setup -Destination $installer

$installer
$nupkg
$feed
