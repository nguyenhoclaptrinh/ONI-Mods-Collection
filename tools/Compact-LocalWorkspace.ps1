[CmdletBinding()]
param(
    [switch]$Apply,
    [switch]$RestoreFullSource,
    [switch]$SkipHardlinks,
    [switch]$SkipSparseCheckout
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$Workspace = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path.TrimEnd("\")
$GitDirectory = Join-Path $Workspace ".git"
$SourceDirectory = Join-Path $Workspace "_source"
$TempRoot = $null
$PreservedDirectoryPaths = @()

$MaintainedSourcePaths = @(
    "_source/AzeTheGreat_source/src/BetterInfoCards",
    "_source/AzeTheGreat_source/src/BetterLogicOverlay",
    "_source/AzeTheGreat_source/src/BuildMenuSearchHotkey",
    "_source/AzeTheGreat_source/src/PriorityZero",
    "_source/beatlepie_source/ChooseNeuralVacillator/ChooseNeuralVacillator",
    "_source/cairath_source/src/AchievementProgress",
    "_source/cairath_source/src/BiggerBuildingMenu",
    "_source/cairath_source/src/BiggerCameraZoomOut",
    "_source/cairath_source/src/GeyserCalculatedAvgOutputTooltip",
    "_source/CritterAIArchitect",
    "_source/customizable_speed_source",
    "_source/doctorfeelgood_source/source/MoveThisHere",
    "_source/glampi_source/AutoDropBottlers",
    "_source/glampi_source/ChainErrand",
    "_source/MoveGeyserInstant",
    "_source/ONI-Mods/src/CustomizableSpeed",
    "_source/peterhan_source/AIImprovements",
    "_source/peterhan_source/AutoEject",
    "_source/peterhan_source/BuildStraightUp",
    "_source/peterhan_source/BulkSettingsChange",
    "_source/peterhan_source/CleanDrop",
    "_source/peterhan_source/CritterInventory",
    "_source/peterhan_source/DeselectNewMaterials",
    "_source/peterhan_source/EfficientFetch",
    "_source/peterhan_source/FallingSand",
    "_source/peterhan_source/FastSave",
    "_source/peterhan_source/FastTrack",
    "_source/peterhan_source/FinishTasks",
    "_source/peterhan_source/FoodTooltip",
    "_source/peterhan_source/ForbidItems",
    "_source/peterhan_source/MismatchedFinder",
    "_source/peterhan_source/ModUpdateDate",
    "_source/peterhan_source/NoSplashScreen",
    "_source/peterhan_source/NoWasteWant",
    "_source/peterhan_source/PipPlantOverlay",
    "_source/peterhan_source/QueueForSink",
    "_source/peterhan_source/ResearchQueue",
    "_source/peterhan_source/SandboxTools",
    "_source/peterhan_source/ShowRange",
    "_source/peterhan_source/SmartPumps",
    "_source/peterhan_source/SweepByType",
    "_source/peterhan_source/ThermalTooltips",
    "_source/peterhan_source/ToastControl",
    "_source/sanchozz_source/src/CarouselCentrifuge",
    "_source/sanchozz_source/src/NoManualDelivery",
    "_source/sanchozz_source/src/SupplyToClosest",
    "_source/sgt_imalas_source/BlueprintsV2",
    "_source/sgt_imalas_source/ClusterTraitGenerationManager",
    "_source/sgt_imalas_source/DupeTransportViaNetwork",
    "_source/sgt_imalas_source/LightBridge",
    "_source/sgt_imalas_source/MassMoveTo",
    "_source/sgt_imalas_source/RebuildPreserve",
    "_source/sgt_imalas_source/RonivansLegacy_ChemicalProcessing",
    "_source/sgt_imalas_source/SetStartDupes"
)

# AutoDropBottlers references these Release outputs directly. Keep their small
# output trees ready while removing other generated directories.
$PreservedOutputPaths = @(
    "_source/peterhan_source/PLibCore/bin/Release/netstandard2.1",
    "_source/peterhan_source/PLibOptions/bin/Release/netstandard2.1",
    "_source/peterhan_source/PLibUI/bin/Release/netstandard2.1"
)

function Invoke-Git {
    param([Parameter(Mandatory = $true)][string[]]$Arguments)

    $output = @(& git -C $Workspace @Arguments 2>&1)
    if ($LASTEXITCODE -ne 0) {
        throw "git $($Arguments -join ' ') failed:`n$($output -join [Environment]::NewLine)"
    }

    return $output
}

function Test-IsUnderPath {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)][string]$Parent
    )

    $absolutePath = [IO.Path]::GetFullPath($Path).TrimEnd("\")
    $absoluteParent = [IO.Path]::GetFullPath($Parent).TrimEnd("\")
    return $absolutePath.StartsWith($absoluteParent + "\", [StringComparison]::OrdinalIgnoreCase)
}

function Assert-WorkspaceDeletePath {
    param([Parameter(Mandatory = $true)][string]$Path)

    $absolutePath = [IO.Path]::GetFullPath($Path).TrimEnd("\")
    if (-not (Test-IsUnderPath $absolutePath $Workspace)) {
        throw "Refusing to delete a path outside the workspace: $absolutePath"
    }

    if ($absolutePath.Equals($Workspace, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to delete the workspace root."
    }

    if ($absolutePath.Equals($GitDirectory, [StringComparison]::OrdinalIgnoreCase) -or
        (Test-IsUnderPath $absolutePath $GitDirectory)) {
        throw "Refusing to delete a Git metadata path: $absolutePath"
    }
}

function Get-ByteTotal {
    param([object[]]$Items)

    $sum = [int64]0
    foreach ($item in @($Items)) {
        if ($null -ne $item) {
            $sum += [int64]$item.Length
        }
    }

    return $sum
}

function Get-PreservedOutputFiles {
    $files = [Collections.Generic.List[IO.FileInfo]]::new()
    foreach ($relativePath in $PreservedOutputPaths) {
        $path = Join-Path $Workspace ($relativePath -replace "/", "\")
        if (Test-Path -LiteralPath $path -PathType Container) {
            foreach ($file in @(Get-ChildItem -LiteralPath $path -File -Force -Recurse -ErrorAction SilentlyContinue)) {
                $files.Add($file)
            }
        }
    }

    return @($files)
}

function Format-MiB {
    param([int64]$Bytes)
    return "{0:N2} MiB" -f ($Bytes / 1MB)
}

function Assert-CleanWorktree {
    $status = @(Invoke-Git @("status", "--porcelain=v1"))
    if ($status.Count -gt 0) {
        throw "The Git worktree must be clean before applying workspace maintenance."
    }
}

function Get-GeneratedDeleteRoots {
    $directories = @(Get-ChildItem -LiteralPath $Workspace -Directory -Force -Recurse -ErrorAction SilentlyContinue |
        Where-Object {
            $_.Name -in @("bin", "obj", ".vs") -and
            -not (Test-IsUnderPath $_.FullName $GitDirectory)
        } |
        Sort-Object { $_.FullName.Length })

    $selected = [Collections.Generic.List[IO.DirectoryInfo]]::new()
    foreach ($directory in $directories) {
        $covered = $false
        foreach ($parent in $selected) {
            if (Test-IsUnderPath $directory.FullName $parent.FullName) {
                $covered = $true
                break
            }
        }

        if (-not $covered) {
            $selected.Add($directory)
        }
    }

    return @($selected)
}

function Get-RuntimeDisposableFiles {
    $items = [Collections.Generic.List[IO.FileInfo]]::new()
    $files = @(Get-ChildItem -LiteralPath $Workspace -File -Force -Recurse -ErrorAction SilentlyContinue)

    foreach ($file in $files) {
        if ((Test-IsUnderPath $file.FullName $GitDirectory) -or
            (Test-IsUnderPath $file.FullName $SourceDirectory) -or
            $file.FullName -match "[\\/](archived_versions)[\\/]") {
            continue
        }

        if ($file.Extension -ieq ".pdb" -or
            $file.FullName.Equals((Join-Path $Workspace "TiengViet\strings.po.bak"),
                [StringComparison]::OrdinalIgnoreCase)) {
            $items.Add($file)
        }
    }

    $playerLog = Join-Path $Workspace "Player.log"
    if (Test-Path -LiteralPath $playerLog -PathType Leaf) {
        $items.Add((Get-Item -LiteralPath $playerLog -Force))
    }

    return @($items | Sort-Object FullName -Unique)
}

function Get-WorkspaceRelativePath {
    param([Parameter(Mandatory = $true)][string]$Path)

    $absolutePath = [IO.Path]::GetFullPath($Path).TrimEnd("\")
    Assert-WorkspaceDeletePath $absolutePath
    return $absolutePath.Substring($Workspace.Length + 1)
}

function Get-PreservedCompatibilityFiles {
    return @(Get-ChildItem -LiteralPath $Workspace -File -Force -Recurse -ErrorAction SilentlyContinue |
        Where-Object {
            -not (Test-IsUnderPath $_.FullName $GitDirectory) -and
            ($_.Extension -ieq ".mo" -or
                $_.Name -ieq "Directory.Build.props.user" -or
                $_.FullName -match "[\\/]archived_versions[\\/]")
        })
}

function Get-PreservedWorkspaceFiles {
    return @(
        @(Get-PreservedOutputFiles) +
        @(Get-IgnoredDependencyFiles) +
        @(Get-PreservedCompatibilityFiles) |
            Sort-Object FullName -Unique
    )
}

function Backup-PreservedWorkspaceItems {
    $script:TempRoot = Join-Path ([IO.Path]::GetTempPath()) ("Compact-LocalWorkspace-" + [Guid]::NewGuid())
    New-Item -ItemType Directory -Path $script:TempRoot | Out-Null

    $script:PreservedDirectoryPaths = @(Get-ChildItem -LiteralPath $Workspace -Directory -Force -Recurse -ErrorAction SilentlyContinue |
        Where-Object {
            -not (Test-IsUnderPath $_.FullName $GitDirectory) -and
            ($_.Name -ieq "archived_versions" -or $_.FullName -match "[\\/]archived_versions[\\/]")
        } |
        ForEach-Object { Get-WorkspaceRelativePath $_.FullName })

    foreach ($file in @(Get-PreservedWorkspaceFiles)) {
        $relativePath = Get-WorkspaceRelativePath $file.FullName
        $destination = Join-Path $script:TempRoot $relativePath
        New-Item -ItemType Directory -Path ([IO.Path]::GetDirectoryName($destination)) -Force | Out-Null
        Copy-Item -LiteralPath $file.FullName -Destination $destination -Force
    }
}

function Restore-PreservedWorkspaceItems {
    if ($null -eq $script:TempRoot -or -not (Test-Path -LiteralPath $script:TempRoot)) {
        return
    }

    foreach ($relativePath in $script:PreservedDirectoryPaths) {
        $destination = Join-Path $Workspace $relativePath
        Assert-WorkspaceDeletePath $destination
        New-Item -ItemType Directory -Path $destination -Force | Out-Null
    }

    foreach ($file in @(Get-ChildItem -LiteralPath $script:TempRoot -File -Force -Recurse -ErrorAction SilentlyContinue)) {
        $relativePath = $file.FullName.Substring($script:TempRoot.Length + 1)
        $destination = Join-Path $Workspace $relativePath
        Assert-WorkspaceDeletePath $destination
        New-Item -ItemType Directory -Path ([IO.Path]::GetDirectoryName($destination)) -Force | Out-Null
        Copy-Item -LiteralPath $file.FullName -Destination $destination -Force
    }
}

function Remove-TemporaryBackup {
    if ($null -eq $script:TempRoot -or -not (Test-Path -LiteralPath $script:TempRoot)) {
        return
    }

    $systemTemp = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd("\")
    $backup = [IO.Path]::GetFullPath($script:TempRoot).TrimEnd("\")
    if (-not (Test-IsUnderPath $backup $systemTemp) -or
        -not ([IO.Path]::GetFileName($backup)).StartsWith("Compact-LocalWorkspace-",
            [StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to delete an unexpected temporary path: $backup"
    }

    Remove-Item -LiteralPath $backup -Recurse -Force
    $script:TempRoot = $null
}

function Remove-SafeWorkspaceItem {
    param([Parameter(Mandatory = $true)][string]$Path)

    if (-not (Test-Path -LiteralPath $Path)) {
        return
    }

    Assert-WorkspaceDeletePath $Path
    Remove-Item -LiteralPath $Path -Recurse -Force
}

function Get-SparseCheckoutPaths {
    $topLevelDirectories = @(Invoke-Git @("ls-tree", "-d", "--name-only", "HEAD") |
        Where-Object { $_ -ne "_source" })

    return @($topLevelDirectories + $MaintainedSourcePaths | Sort-Object -Unique)
}

function Set-MaintainedSparseCheckout {
    $paths = @(Get-SparseCheckoutPaths)
    Invoke-Git @("sparse-checkout", "init", "--cone") | Out-Null
    Invoke-Git (@("sparse-checkout", "set", "--cone", "--") + $paths) | Out-Null
}

function Get-IgnoredDependencyFiles {
    $ignored = @(Invoke-Git @("ls-files", "--others", "--ignored", "--exclude-standard", "--", "_source"))
    $files = [Collections.Generic.List[IO.FileInfo]]::new()

    foreach ($relativePath in $ignored) {
        if ($relativePath -notmatch "(?i)(^|/)lib/" -or
            $relativePath -match "(?i)(^|/)(bin|obj|\.vs)/") {
            continue
        }

        $absolutePath = Join-Path $Workspace ($relativePath -replace "/", "\")
        if (Test-Path -LiteralPath $absolutePath -PathType Leaf) {
            $files.Add((Get-Item -LiteralPath $absolutePath -Force))
        }
    }

    return @($files)
}

function Get-CanonicalDependencyScore {
    param([Parameter(Mandatory = $true)][IO.FileInfo]$File)

    if ($File.FullName -match "(?i)[\\/]peterhan_source[\\/]lib[\\/]") {
        return 0
    }
    if ($File.FullName -match "(?i)[\\/]sgt_imalas_source[\\/]lib[\\/]") {
        return 1
    }
    if ($File.FullName -match "(?i)[\\/]beatlepie_source[\\/].*[\\/]lib[\\/]") {
        return 2
    }

    return 10
}

function Get-FileId {
    param([Parameter(Mandatory = $true)][IO.FileInfo]$File)

    $output = @(& fsutil file queryfileid $File.FullName 2>$null)
    if ($LASTEXITCODE -ne 0) {
        return $null
    }

    return ($output -join "`n").Trim()
}

function Test-FilesShareFileId {
    param(
        [Parameter(Mandatory = $true)][IO.FileInfo]$First,
        [Parameter(Mandatory = $true)][IO.FileInfo]$Second
    )

    $firstId = Get-FileId $First
    if ([string]::IsNullOrWhiteSpace($firstId)) {
        return $false
    }

    $secondId = Get-FileId $Second
    return -not [string]::IsNullOrWhiteSpace($secondId) -and $firstId -eq $secondId
}

function Get-DuplicateDependencyGroups {
    $groups = [Collections.Generic.List[object]]::new()
    $files = @(Get-IgnoredDependencyFiles)

    foreach ($sizeGroup in @($files | Group-Object Length | Where-Object { $_.Count -gt 1 })) {
        $hashRows = @($sizeGroup.Group | ForEach-Object {
            [pscustomobject]@{
                File = $_
                Hash = (Get-FileHash -Algorithm SHA256 -LiteralPath $_.FullName).Hash
            }
        })

        foreach ($hashGroup in @($hashRows | Group-Object Hash | Where-Object { $_.Count -gt 1 })) {
            $members = @($hashGroup.Group.File | Sort-Object `
                @{ Expression = { Get-CanonicalDependencyScore $_ }; Ascending = $true },
                @{ Expression = { $_.FullName }; Ascending = $true })
            $groups.Add([pscustomobject]@{
                Canonical = $members[0]
                Duplicates = @($members | Select-Object -Skip 1)
                Hash = $hashGroup.Name
            })
        }
    }

    return @($groups)
}

function Set-DependencyHardlinks {
    $groups = @(Get-DuplicateDependencyGroups)
    $linked = 0
    $savedBytes = [int64]0

    foreach ($group in $groups) {
        foreach ($duplicate in $group.Duplicates) {
            Assert-WorkspaceDeletePath $duplicate.FullName
            if (Test-FilesShareFileId $group.Canonical $duplicate) {
                continue
            }

            $savedBytes += $duplicate.Length

            if (-not $Apply) {
                continue
            }

            Remove-Item -LiteralPath $duplicate.FullName -Force
            try {
                New-Item -ItemType HardLink -Path $duplicate.FullName -Target $group.Canonical.FullName | Out-Null
                $linked++
            }
            catch {
                Copy-Item -LiteralPath $group.Canonical.FullName -Destination $duplicate.FullName -Force
                throw "Failed to create hardlink $($duplicate.FullName): $($_.Exception.Message)"
            }
        }
    }

    return [pscustomobject]@{
        Groups = $groups.Count
        LinkedFiles = $linked
        PotentialSavedBytes = $savedBytes
    }
}

function Get-DriveFreeBytes {
    $root = [IO.Path]::GetPathRoot($Workspace)
    return ([IO.DriveInfo]::new($root)).AvailableFreeSpace
}

function Test-WorkspaceIsNtfs {
    $root = [IO.Path]::GetPathRoot($Workspace)
    return ([IO.DriveInfo]::new($root)).DriveFormat -ieq "NTFS"
}

$gitRoot = (Invoke-Git @("rev-parse", "--show-toplevel") | Select-Object -First 1).Replace("/", "\")
if (-not $gitRoot.Equals($Workspace, [StringComparison]::OrdinalIgnoreCase)) {
    throw "Run this script from its Local workspace. Expected $Workspace, got $gitRoot."
}

if ($Apply) {
    Assert-CleanWorktree
}

if ($RestoreFullSource) {
    Write-Host "Restore full tracked source checkout"
    if (-not $Apply) {
        Write-Host "Dry-run only. Re-run with -Apply to disable sparse checkout."
        exit 0
    }

    try {
        Backup-PreservedWorkspaceItems
        Invoke-Git @("sparse-checkout", "disable") | Out-Null
    }
    finally {
        Restore-PreservedWorkspaceItems
        Remove-TemporaryBackup
    }
    Write-Host "Full tracked source checkout restored. Ignored dependency caches are unchanged."
    exit 0
}

$freeBefore = Get-DriveFreeBytes
$generatedRoots = @(Get-GeneratedDeleteRoots)
$generatedFiles = @($generatedRoots | ForEach-Object {
    Get-ChildItem -LiteralPath $_.FullName -File -Force -Recurse -ErrorAction SilentlyContinue
})
$preservedFiles = @(Get-PreservedOutputFiles)
$generatedReclaimableBytes = [Math]::Max(
    [int64]0,
    (Get-ByteTotal $generatedFiles) - (Get-ByteTotal $preservedFiles)
)
$runtimeFiles = @(Get-RuntimeDisposableFiles)
$scratch = Join-Path $Workspace "scratch"
$scratchFiles = @()
if (Test-Path -LiteralPath $scratch -PathType Container) {
    $scratchFiles = @(Get-ChildItem -LiteralPath $scratch -File -Force -Recurse -ErrorAction SilentlyContinue)
}

Write-Host "Workspace: $Workspace"
Write-Host "Mode: $(if ($Apply) { 'apply' } else { 'dry-run' })"
Write-Host "Generated roots: $($generatedRoots.Count), net reclaimable after PLib restore: $(Format-MiB $generatedReclaimableBytes)"
Write-Host "Runtime disposable files: $($runtimeFiles.Count), reclaimable: $(Format-MiB (Get-ByteTotal $runtimeFiles))"
Write-Host "Top-level scratch files: $($scratchFiles.Count), reclaimable: $(Format-MiB (Get-ByteTotal $scratchFiles))"

if ($Apply) {
    try {
        Backup-PreservedWorkspaceItems
        foreach ($root in $generatedRoots) {
            Remove-SafeWorkspaceItem $root.FullName
        }

        foreach ($file in $runtimeFiles) {
            if (Test-Path -LiteralPath $file.FullName -PathType Leaf) {
                Assert-WorkspaceDeletePath $file.FullName
                Remove-Item -LiteralPath $file.FullName -Force
            }
        }

        if (Test-Path -LiteralPath $scratch -PathType Container) {
            Remove-SafeWorkspaceItem $scratch
        }

        if (-not $SkipSparseCheckout) {
            Set-MaintainedSparseCheckout
        }
    }
    finally {
        Restore-PreservedWorkspaceItems
        Remove-TemporaryBackup
    }
}

if (-not $SkipHardlinks -and (Test-WorkspaceIsNtfs)) {
    $hardlinks = Set-DependencyHardlinks
    Write-Host "Dependency duplicate groups: $($hardlinks.Groups)"
    Write-Host "Dependency hardlink potential: $(Format-MiB $hardlinks.PotentialSavedBytes)"
    if ($Apply) {
        Write-Host "Dependency files hardlinked: $($hardlinks.LinkedFiles)"
    }
}
elseif (-not $SkipHardlinks) {
    Write-Host "Dependency hardlinks skipped: workspace volume is not NTFS."
}

if ($Apply) {
    $freeAfter = Get-DriveFreeBytes
    Write-Host "Drive free-space increase: $(Format-MiB ($freeAfter - $freeBefore))"
    Write-Host "Workspace maintenance complete."
}
else {
    Write-Host "Dry-run only. Re-run with -Apply to compact the workspace."
}
