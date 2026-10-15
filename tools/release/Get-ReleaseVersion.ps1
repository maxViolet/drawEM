<#
.SYNOPSIS
Returns the release version for a vX.Y.Z tag and fails for any other tag.

.DESCRIPTION
The release tag is the only version source. Only plain vMAJOR.MINOR.PATCH tags
without leading zeros, pre-release, or build suffixes are accepted.

.EXAMPLE
./tools/release/Get-ReleaseVersion.ps1 -Tag v1.2.3   # writes 1.2.3
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [AllowEmptyString()]
    [string] $Tag
)

$ErrorActionPreference = 'Stop'

# \z instead of $: $ also matches before a trailing newline.
if ($Tag -cnotmatch '^v((0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*))\z') {
    throw "Release tag '$Tag' is not of the form vX.Y.Z (for example v1.2.3)."
}

$Matches[1]
