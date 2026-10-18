<#
.SYNOPSIS
Writes SHA256SUMS.txt for an explicit list of release files.

.DESCRIPTION
One line per file, "<lowercase sha256>  <file name>", sorted by name (ordinal),
LF endings, UTF-8 without BOM: the sha256sum -c format. File names carry no
directory, so two inputs with the same name fail. The manifest never lists
itself. Nothing is written when any check fails.

-Path takes an array, or one comma-separated string when the script runs
through powershell -File.

.EXAMPLE
./tools/release/Write-Sha256Sums.ps1 -Path artifacts/drawEM-1.2.3-win-x64.zip -OutputPath artifacts/SHA256SUMS.txt
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string[]] $Path,

    [Parameter(Mandatory)]
    [string] $OutputPath
)

$ErrorActionPreference = 'Stop'

$output = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($OutputPath)
$files = @($Path | ForEach-Object { $_ -split ',' } | Where-Object { $_ -ne '' } |
    ForEach-Object { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($_) })
if ($files.Count -eq 0) {
    throw 'No input files were given.'
}

$names = @{}
foreach ($file in $files) {
    if (-not (Test-Path -LiteralPath $file -PathType Leaf)) {
        throw "Input file '$file' does not exist."
    }
    if ([string]::Equals($file, $output, [StringComparison]::OrdinalIgnoreCase)) {
        throw "The manifest '$output' cannot list itself."
    }
    $name = [IO.Path]::GetFileName($file)
    if ($names.ContainsKey($name)) {
        throw "Two inputs have the file name '$name': '$($names[$name])' and '$file'."
    }
    $names[$name] = $file
}

$sortedNames = [string[]] $names.Keys
[Array]::Sort($sortedNames, [StringComparer]::Ordinal)
$lines = foreach ($name in $sortedNames) {
    $hash = (Get-FileHash -LiteralPath $names[$name] -Algorithm SHA256).Hash.ToLowerInvariant()
    "$hash  $name`n"
}

New-Item -ItemType Directory -Path ([IO.Path]::GetDirectoryName($output)) -Force | Out-Null
[IO.File]::WriteAllText($output, (-join $lines), [Text.UTF8Encoding]::new($false))
