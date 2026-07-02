param(
    [string]$TypeName = "KButtonMenu"
)

$gameManagedDir = "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed"
$dllPaths = @(
    (Join-Path $gameManagedDir "Assembly-CSharp.dll"),
    (Join-Path $gameManagedDir "Assembly-CSharp-firstpass.dll")
)

$resolveHandler = [System.ResolveEventHandler] {
    param($sender, $args)
    $name = $args.Name.Split(',')[0]
    $depPath = Join-Path $gameManagedDir "$name.dll"
    if (Test-Path $depPath) {
        return [System.Reflection.Assembly]::LoadFrom($depPath)
    }
    return $null
}

[System.AppDomain]::CurrentDomain.add_AssemblyResolve($resolveHandler)

try {
    # Load all assemblies first so they can resolve each other
    $assemblies = @()
    foreach ($path in $dllPaths) {
        if (Test-Path $path) {
            $assemblies += [System.Reflection.Assembly]::LoadFrom($path)
        }
    }

    $type = $null
    foreach ($asm in $assemblies) {
        $type = $asm.GetType($TypeName)
        if ($null -eq $type) {
            try {
                $types = $asm.GetTypes()
            } catch {
                $types = $_.Exception.Types | Where-Object { $null -ne $_ }
            }
            $type = $types | Where-Object { $_.Name -eq $TypeName -or $_.FullName -eq $TypeName } | Select-Object -First 1
        }
        if ($null -ne $type) {
            break
        }
    }

    if ($type) {
        Write-Host "Type: $($type.FullName) (from $($type.Assembly.GetName().Name))"
        Write-Host "Fields:"
        $fields = $type.GetFields([System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance -bor [System.Reflection.BindingFlags]::Static)
        if ($fields) {
            foreach ($field in $fields) {
                if ($null -ne $field) {
                    $typeNameStr = "UnknownType"
                    if ($null -ne $field.FieldType) {
                        $typeNameStr = if ($field.FieldType.FullName) { $field.FieldType.FullName } else { $field.FieldType.Name }
                    }
                    Write-Host "  $typeNameStr $($field.Name)"
                }
            }
        }
        Write-Host "Properties:"
        $props = $type.GetProperties([System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance -bor [System.Reflection.BindingFlags]::Static)
        if ($props) {
            foreach ($prop in $props) {
                if ($null -ne $prop) {
                    $typeNameStr = "UnknownType"
                    if ($null -ne $prop.PropertyType) {
                        $typeNameStr = if ($prop.PropertyType.FullName) { $prop.PropertyType.FullName } else { $prop.PropertyType.Name }
                    }
                    Write-Host "  $typeNameStr $($prop.Name)"
                }
            }
        }
    } else {
        Write-Host "Type '$TypeName' not found in loaded assemblies."
    }
} catch {
    Write-Error "Failed to reflect type: $_"
} finally {
    [System.AppDomain]::CurrentDomain.remove_AssemblyResolve($resolveHandler)
}
