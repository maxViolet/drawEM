<#
.SYNOPSIS
Generates the S5-01 MP4 fixture set and its SHA256SUMS.txt.

.DESCRIPTION
Uses one pinned FFmpeg build: gyan.dev 7.1.1 essentials (win64, static). Without
-FfmpegPath the script downloads that archive once into
%LOCALAPPDATA%\drawEM-ffmpeg-cache, checks its SHA-256, and extracts it there.
Every encode is single-threaded and bit-exact, so the same FFmpeg build
reproduces the checked-in bytes. Tests read the checked-in files and never run
FFmpeg.

Each fixture is 2 s of the FFmpeg testsrc2 pattern at 320x180, 25 fps, with a
440 Hz sine tone when it carries audio.

-Verify generates into a temporary directory and fails unless every hash
matches the SHA256SUMS.txt in -OutputDirectory; nothing there is changed.

.EXAMPLE
./tools/video-fixtures/New-VideoFixtures.ps1
./tools/video-fixtures/New-VideoFixtures.ps1 -Verify
#>
[CmdletBinding()]
param(
    # Default: tests/DrawEM.Tests/Fixtures/Video.
    [string] $OutputDirectory,

    # Directory that holds ffmpeg.exe. Must be the pinned build; the version line is checked.
    [string] $FfmpegPath,

    [switch] $Verify
)

$ErrorActionPreference = 'Stop'
if (-not $OutputDirectory) {
    $OutputDirectory = Join-Path $PSScriptRoot '..\..\tests\DrawEM.Tests\Fixtures\Video'
}

$FfmpegVersion = '7.1.1-essentials_build-www.gyan.dev'
$FfmpegUrl = 'https://github.com/GyanD/codexffmpeg/releases/download/7.1.1/ffmpeg-7.1.1-essentials_build.zip'
$FfmpegZipSha256 = '04861d3339c5ebe38b56c19a15cf2c0cc97f5de4fa8910e4d47e5e6404e4a2d4'

function Get-PinnedFfmpeg {
    $cache = Join-Path $env:LOCALAPPDATA 'drawEM-ffmpeg-cache'
    $bin = Join-Path $cache 'ffmpeg-7.1.1-essentials_build\bin'
    if (Test-Path -LiteralPath (Join-Path $bin 'ffmpeg.exe')) {
        return $bin
    }

    New-Item -ItemType Directory -Path $cache -Force | Out-Null
    $zip = Join-Path $cache 'ffmpeg-7.1.1-essentials_build.zip'
    if (-not (Test-Path -LiteralPath $zip)) {
        Invoke-WebRequest -Uri $FfmpegUrl -OutFile $zip -UseBasicParsing
    }
    $actual = (Get-FileHash -LiteralPath $zip -Algorithm SHA256).Hash.ToLowerInvariant()
    if ($actual -ne $FfmpegZipSha256) {
        throw "FFmpeg archive hash is $actual; expected $FfmpegZipSha256. Delete '$zip' and retry."
    }
    Expand-Archive -LiteralPath $zip -DestinationPath $cache -Force
    return $bin
}

if (-not $FfmpegPath) {
    $FfmpegPath = Get-PinnedFfmpeg
}
$ffmpeg = Join-Path $FfmpegPath 'ffmpeg.exe'
$versionLine = (& $ffmpeg -hide_banner -version | Select-Object -First 1)
if ($versionLine -notmatch [regex]::Escape("ffmpeg version $FfmpegVersion ")) {
    throw "Expected ffmpeg version $FfmpegVersion; '$ffmpeg' reports: $versionLine"
}

$video = @('-f', 'lavfi', '-i', 'testsrc2=size=320x180:rate=25:duration=2')
$video1080p60 = @('-f', 'lavfi', '-i', 'testsrc2=size=1920x1080:rate=60:duration=1')
$audio = @('-f', 'lavfi', '-i', 'sine=frequency=440:sample_rate=48000:duration=2')
$audio1s = @('-f', 'lavfi', '-i', 'sine=frequency=440:sample_rate=48000:duration=1')
$exact = @('-map_metadata', '-1', '-fflags', '+bitexact', '-flags:v', '+bitexact', '-flags:a', '+bitexact', '-threads', '1')
$x264 = @('-c:v', 'libx264', '-preset', 'veryfast', '-crf', '30', '-g', '25')
$aac = @('-c:a', 'aac', '-b:a', '64k', '-ac', '2')
$aacMono44k = @('-c:a', 'aac', '-b:a', '64k', '-ac', '1', '-ar', '44100')

# Name -> FFmpeg arguments. Expected validation and playback results are in Fixtures/Video/README.md.
$encodes = [ordered]@{
    'h264-high-aac.mp4'        = @($video + $audio + $exact + $x264 + @('-profile:v', 'high', '-pix_fmt', 'yuv420p') + $aac)
    'h264-main-aac-mono44k.mp4' = @($video + $audio + $exact + $x264 + @('-profile:v', 'main', '-pix_fmt', 'yuv420p') + $aacMono44k)
    'h264-baseline-aac.mp4'    = @($video + $audio + $exact + $x264 + @('-profile:v', 'baseline', '-pix_fmt', 'yuv420p') + $aac)
    'h264-high-silent.mp4'     = @($video + $exact + $x264 + @('-profile:v', 'high', '-pix_fmt', 'yuv420p'))
    'h264-high-1080p60-aac.mp4' = @($video1080p60 + $audio1s + $exact + @('-c:v', 'libx264', '-preset', 'veryfast', '-crf', '38', '-g', '60',
                                     '-profile:v', 'high', '-pix_fmt', 'yuv420p') + $aac)
    'h264-high10-aac.mp4'      = @($video + $audio + $exact + $x264 + @('-profile:v', 'high10', '-pix_fmt', 'yuv420p10le') + $aac)
    'h264-high-opus.mp4'       = @($video + $audio + $exact + $x264 + @('-profile:v', 'high', '-pix_fmt', 'yuv420p', '-c:a', 'libopus', '-b:a', '64k'))
    'hevc-main-aac.mp4'        = @($video + $audio + $exact + @('-c:v', 'libx265', '-preset', 'veryfast', '-crf', '30', '-tag:v', 'hvc1',
                                     '-x265-params', 'pools=none:frame-threads=1:log-level=error', '-pix_fmt', 'yuv420p') + $aac)
    'aac-audio-only.mp4'       = @($audio + $exact + $aac)
    'h264-high-aac-fragmented.mp4' = @($video + $audio + $exact + $x264 + @('-profile:v', 'high', '-pix_fmt', 'yuv420p') + $aac +
                                     @('-movflags', 'frag_keyframe+empty_moov'))
}

$target = if ($Verify) { Join-Path ([IO.Path]::GetTempPath()) ("drawEM-video-fixtures-" + [guid]::NewGuid().ToString('N')) } else { $OutputDirectory }
New-Item -ItemType Directory -Path $target -Force | Out-Null
$target = (Resolve-Path -LiteralPath $target).Path

foreach ($name in $encodes.Keys) {
    $file = Join-Path $target $name
    & $ffmpeg -hide_banner -loglevel error -y @($encodes[$name]) $file
    if ($LASTEXITCODE -ne 0) {
        throw "ffmpeg failed for $name (exit $LASTEXITCODE)."
    }
}

# truncated-moov.mp4: the first half of h264-high-aac.mp4. FFmpeg writes moov after mdat, so the cut
# leaves a partial mdat and no movie header.
$whole = [IO.File]::ReadAllBytes((Join-Path $target 'h264-high-aac.mp4'))
[IO.File]::WriteAllBytes((Join-Path $target 'truncated-moov.mp4'), $whole[0..([int]($whole.Length / 2) - 1)])

# not-mp4.mp4: plain text with an .mp4 name.
[IO.File]::WriteAllText((Join-Path $target 'not-mp4.mp4'), "This is not an MP4 file.`n", [Text.UTF8Encoding]::new($false))

# h264-high10-labeled-high.mp4: h264-high10-aac.mp4 with the avcC profile byte changed from 110 (High 10)
# to 100 (High). The SPS still says High 10, so the header passes validation and the Windows H.264
# decoder rejects the stream when playback opens it.
$high10 = [IO.File]::ReadAllBytes((Join-Path $target 'h264-high10-aac.mp4'))
$avcC = [Text.Encoding]::ASCII.GetString($high10).IndexOf('avcC', [StringComparison]::Ordinal)
if ($avcC -lt 0 -or $high10[$avcC + 5] -ne 110) {
    throw 'h264-high10-aac.mp4 has no avcC box with profile 110.'
}
$high10[$avcC + 5] = 100
[IO.File]::WriteAllBytes((Join-Path $target 'h264-high10-labeled-high.mp4'), $high10)

$files = @(Get-ChildItem -LiteralPath $target -Filter '*.mp4' | ForEach-Object FullName)
$sums = Join-Path $target 'SHA256SUMS.txt'
& (Join-Path $PSScriptRoot '..\release\Write-Sha256Sums.ps1') -Path $files -OutputPath $sums

if ($Verify) {
    $expected = [IO.File]::ReadAllText((Join-Path $OutputDirectory 'SHA256SUMS.txt'))
    $actual = [IO.File]::ReadAllText($sums)
    Remove-Item -LiteralPath $target -Recurse -Force
    if ($expected -ne $actual) {
        throw "Regenerated fixtures differ from the checked-in set.`nExpected:`n$expected`nActual:`n$actual"
    }
    Write-Output "Regenerated fixtures match $(Join-Path $OutputDirectory 'SHA256SUMS.txt')."
}
else {
    Get-Content -LiteralPath $sums
}
