[CmdletBinding()]
param(
    [string]$Destination = $null,
    [string]$Source = $null
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$workspace = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path

if ([string]::IsNullOrWhiteSpace($Destination)) {
    $Destination = Join-Path $workspace "Player.log"
}

if ([string]::IsNullOrWhiteSpace($Source)) {
    if ([string]::IsNullOrWhiteSpace($env:USERPROFILE)) {
        throw "USERPROFILE is not set; pass -Source explicitly."
    }

    $Source = Join-Path $env:USERPROFILE "AppData\LocalLow\Klei\Oxygen Not Included\Player.log"
}

$sourcePath = [IO.Path]::GetFullPath($Source)
$destinationPath = [IO.Path]::GetFullPath($Destination)
$workspaceRoot = [IO.Path]::GetFullPath($workspace).TrimEnd([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar)

if (-not $destinationPath.StartsWith($workspaceRoot + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase) -and
    -not $destinationPath.Equals($workspaceRoot, [StringComparison]::OrdinalIgnoreCase)) {
    throw "Destination must be inside the project workspace."
}

if (-not (Test-Path -LiteralPath $sourcePath -PathType Leaf)) {
    throw "Player.log was not found. Start the game once or pass -Source with the log path."
}

$destinationDirectory = [IO.Path]::GetDirectoryName($destinationPath)
if (-not [string]::IsNullOrWhiteSpace($destinationDirectory)) {
    New-Item -ItemType Directory -Path $destinationDirectory -Force | Out-Null
}

Copy-Item -LiteralPath $sourcePath -Destination $destinationPath -Force

$relativeDestination = $destinationPath.Substring($workspaceRoot.Length).TrimStart([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar)
Write-Host "Copied Player.log to $relativeDestination"
