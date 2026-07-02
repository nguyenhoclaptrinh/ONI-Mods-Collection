param (
    [string]$assemblyPath = "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll",
    [string]$targetTypeName = "Storage"
)

# Setup Assembly Resolver to find dependencies in the same folder
$managedDir = Split-Path $assemblyPath
[System.AppDomain]::CurrentDomain.add_AssemblyResolve({
    param($sender, $args)
    $name = New-Object System.Reflection.AssemblyName($args.Name)
    $assemblyFile = Join-Path $managedDir "$($name.Name).dll"
    if (Test-Path $assemblyFile) {
        return [System.Reflection.Assembly]::LoadFrom($assemblyFile)
    }
    return $null
})

# Load Assembly
try {
    $asm = [System.Reflection.Assembly]::LoadFrom($assemblyPath)
} catch {
    Write-Error "Failed to load assembly: $_"
    if ($_.Exception.LoaderExceptions) {
        Write-Error "Loader exceptions:"
        foreach ($le in $_.Exception.LoaderExceptions) {
            Write-Error "  $le"
        }
    }
    exit 1
}

$type = $asm.GetType($targetTypeName)
if ($type -eq $null) {
    Write-Host "Type $targetTypeName not found."
    exit 1
}

Write-Host "=== Members of $targetTypeName ==="
Write-Host "Fields:"
$type.GetFields([System.Reflection.BindingFlags]::Public -or [System.Reflection.BindingFlags]::NonPublic -or [System.Reflection.BindingFlags]::Instance -or [System.Reflection.BindingFlags]::Static) | 
    Sort-Object Name | 
    ForEach-Object {
        Write-Host "  $($_.Attributes) $($_.FieldType.Name) $($_.Name)"
    }

Write-Host "`nProperties:"
$type.GetProperties([System.Reflection.BindingFlags]::Public -or [System.Reflection.BindingFlags]::NonPublic -or [System.Reflection.BindingFlags]::Instance -or [System.Reflection.BindingFlags]::Static) | 
    Sort-Object Name | 
    ForEach-Object {
        $getStr = ""
        if ($_.GetGetMethod($true) -ne $null) { $getStr = "get;" }
        $setStr = ""
        if ($_.GetSetMethod($true) -ne $null) { $setStr = "set;" }
        Write-Host "  $($_.PropertyType.Name) $($_.Name) { $getStr $setStr }"
    }
