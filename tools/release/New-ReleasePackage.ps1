<#
.SYNOPSIS
Checks a Windows x64 publish directory and zips it as drawEM-<version>-win-x64.zip.

.DESCRIPTION
Fails without writing a ZIP when the publish is not self-contained, is a
single-file bundle, or its executable carries a version other than -Version.
Writes the full path of the created ZIP.

.EXAMPLE
./tools/release/New-ReleasePackage.ps1 -PublishDirectory src/DrawEM.App/bin/Release/net8.0-windows/win-x64/publish -Version 1.2.3 -OutputDirectory artifacts
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string] $PublishDirectory,

    [Parameter(Mandatory)]
    [string] $Version,

    [Parameter(Mandatory)]
    [string] $OutputDirectory
)

$ErrorActionPreference = 'Stop'

if ($Version -cnotmatch '^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\z') {
    throw "Version '$Version' is not of the form X.Y.Z."
}

$publish = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($PublishDirectory)
$output = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($OutputDirectory)
if (-not (Test-Path -LiteralPath $publish -PathType Container)) {
    throw "Publish directory '$publish' does not exist."
}

# A self-contained publish carries the runtime (coreclr.dll) and host resolver (hostfxr.dll);
# a single-file publish bundles DrawEM.App.dll into the executable.
$required = [ordered]@{
    'DrawEM.App.exe' = 'application executable'
    'DrawEM.App.dll' = 'application assembly; single-file publish is not allowed'
    'coreclr.dll'    = '.NET runtime; publish must be self-contained'
    'hostfxr.dll'    = '.NET host resolver; publish must be self-contained'
}
$missing = @($required.Keys | Where-Object { -not (Test-Path -LiteralPath (Join-Path $publish $_) -PathType Leaf) })
if ($missing.Count -gt 0) {
    $details = ($missing | ForEach-Object { "$_ ($($required[$_]))" }) -join ', '
    throw "Publish directory '$publish' is missing: $details."
}

$info = [System.Diagnostics.FileVersionInfo]::GetVersionInfo((Join-Path $publish 'DrawEM.App.exe'))
$fileVersion = '{0}.{1}.{2}' -f $info.FileMajorPart, $info.FileMinorPart, $info.FileBuildPart
# The SDK appends +<commit> to the product version; the part before it must still match the tag.
$productVersion = ($info.ProductVersion -split '\+', 2)[0]
if ($fileVersion -ne $Version -or $productVersion -ne $Version) {
    throw "DrawEM.App.exe has file version '$($info.FileVersion)' and product version '$($info.ProductVersion)'; expected version $Version from the release tag."
}

$zipPath = Join-Path $output "drawEM-$Version-win-x64.zip"
if (Test-Path -LiteralPath $zipPath) {
    throw "Release package '$zipPath' already exists."
}
New-Item -ItemType Directory -Path $output -Force | Out-Null

Add-Type -AssemblyName System.IO.Compression, System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory($publish, $zipPath, [System.IO.Compression.CompressionLevel]::Optimal, $false)

$expected = @(Get-ChildItem -LiteralPath $publish -Recurse -File -Force |
    ForEach-Object { $_.FullName.Substring($publish.TrimEnd('\', '/').Length + 1).Replace('\', '/') } |
    Sort-Object)
$zip = [System.IO.Compression.ZipFile]::OpenRead($zipPath)
try {
    $actual = @($zip.Entries | Where-Object { $_.Name -ne '' } | ForEach-Object { $_.FullName.Replace('\', '/') } | Sort-Object)
}
finally {
    $zip.Dispose()
}
if (Compare-Object $expected $actual) {
    Remove-Item -LiteralPath $zipPath
    throw "Release package '$zipPath' does not contain exactly the files of '$publish'."
}

$zipPath
