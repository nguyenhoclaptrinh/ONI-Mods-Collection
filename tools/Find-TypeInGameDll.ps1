param(
    [string]$SearchName = "FewOption"
)

$gameManagedDir = "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed"
$dllPath = Join-Path $gameManagedDir "Assembly-CSharp.dll"

if (-not (Test-Path $dllPath)) {
    Write-Error "Assembly-CSharp.dll not found at $dllPath"
    return
}

# Tuân thủ đăng ký AssemblyResolve để tự động load dependency từ thư mục game
$resolveHandler = [System.ResolveEventHandler] {
    param($sender, $args)
    $name = $args.Name.Split(',')[0]
    $depPath = Join-Path $gameManagedDir "$name.dll"
    if (Test-Path $depPath) {
        return [System.Reflection.Assembly]::LoadFile($depPath)
    }
    return $null
}

[System.AppDomain]::CurrentDomain.add_AssemblyResolve($resolveHandler)

try {
    $assembly = [System.Reflection.Assembly]::LoadFile($dllPath)
    $types = $assembly.GetTypes()
} catch [System.Reflection.ReflectionTypeLoadException] {
    Write-Warning "Some types failed to load. Searching successfully loaded types..."
    $types = $_.Exception.Types | Where-Object { $null -ne $psitem }
} catch {
    Write-Error "Failed to load assembly: $error"
    return
} finally {
    # Hủy đăng ký handler để tránh leak
    [System.AppDomain]::CurrentDomain.remove_AssemblyResolve($resolveHandler)
}

$results = $types | Where-Object { $psitem.Name -like "*$SearchName*" }
if ($results) {
    foreach ($type in $results) {
        Write-Host $type.FullName
    }
} else {
    Write-Host "No types found containing '$SearchName'"
}
