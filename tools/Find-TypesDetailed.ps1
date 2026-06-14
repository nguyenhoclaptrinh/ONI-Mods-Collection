param(
    [string]$SearchName = ""
)

$gameManagedDir = "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed"
$dllPath = Join-Path $gameManagedDir "Assembly-CSharp.dll"

if (-not (Test-Path $dllPath)) {
    Write-Error "Assembly-CSharp.dll not found at $dllPath"
    return
}

# Resolve Handler để load tất cả DLL từ gameManagedDir
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
    Write-Host "Successfully loaded all types: $($types.Length)"
} catch [System.Reflection.ReflectionTypeLoadException] {
    Write-Warning "LoaderExceptions:"
    foreach ($ex in $_.Exception.LoaderExceptions) {
        Write-Warning " - $($ex.Message)"
    }
    $types = $_.Exception.Types | Where-Object { $null -ne $psitem }
    Write-Host "Loaded partial types: $($types.Length)"
} catch {
    Write-Error "Failed to load assembly: $_"
    return
} finally {
    [System.AppDomain]::CurrentDomain.remove_AssemblyResolve($resolveHandler)
}

if ($SearchName) {
    $results = $types | Where-Object { $psitem.FullName -like "*$SearchName*" -or $psitem.Name -like "*$SearchName*" }
    if ($results) {
        Write-Host "Found $($results.Length) matches:"
        foreach ($type in $results) {
            Write-Host "  $($type.FullName) (Assembly: $($type.Assembly.GetName().Name))"
        }
    } else {
        Write-Host "No types found matching '$SearchName'"
    }
}
