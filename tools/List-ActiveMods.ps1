[CmdletBinding()]
param(
    [string]$Path = $null
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# Thư mục chứa script
$scriptRoot = $PSScriptRoot
if (-not $scriptRoot) {
    $scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition
}

# Tự động tìm mods.json nếu không truyền tham số
if ([string]::IsNullOrWhiteSpace($Path)) {
    $defaultPaths = @(
        "D:\Documents\Klei\OxygenNotIncluded\mods\mods.json",
        (Join-Path $env:USERPROFILE "Documents\Klei\OxygenNotIncluded\mods\mods.json"),
        (Join-Path $scriptRoot "..\..\mods.json"),
        "C:\Users\thain\Documents\Klei\OxygenNotIncluded\mods\mods.json"
    )

    foreach ($p in $defaultPaths) {
        if (Test-Path -LiteralPath $p -PathType Leaf) {
            $Path = $p
            break
        }
    }
}

if ([string]::IsNullOrWhiteSpace($Path) -or -not (Test-Path -LiteralPath $Path -PathType Leaf)) {
    throw "Không tìm thấy file mods.json của game. Vui lòng truyền đường dẫn chính xác qua tham số -Path."
}

$resolvedPath = (Resolve-Path -LiteralPath $Path).Path
Write-Host "Đang đọc danh sách mod từ: $resolvedPath`n" -ForegroundColor Cyan

# Đọc và Parse JSON
$content = Get-Content -LiteralPath $resolvedPath -Raw | ConvertFrom-Json
if (-not $content -or -not $content.mods) {
    Write-Host "File mods.json rỗng hoặc không đúng định dạng." -ForegroundColor Red
    return
}

# Lọc các mod được kích hoạt (enabled hoặc có trong danh sách enabledForDlc)
$activeMods = $content.mods | Where-Object {
    $_.enabled -eq $true -or ($_.enabledForDlc -ne $null -and $_.enabledForDlc.Count -gt 0)
}

if ($activeMods.Count -eq 0) {
    Write-Host "Không có mod nào đang được kích hoạt." -ForegroundColor Yellow
    return
}

# Hiển thị bảng
$activeMods | Select-Object -Property `
    @{Name="Tên Mod (Title)"; Expression={$_.label.title}}, `
    @{Name="ID Tĩnh (StaticID)"; Expression={$_.staticID}}, `
    @{Name="DLC"; Expression={ if ($_.enabledForDlc -ne $null) { $_.enabledForDlc -join ", " } else { "" } }} | `
    Format-Table -AutoSize
