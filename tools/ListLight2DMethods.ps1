$gameManagedDir = "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed"

# Tải trước các Assembly quan trọng
$preloads = @(
    "UnityEngine.dll",
    "UnityEngine.CoreModule.dll",
    "Assembly-CSharp-firstpass.dll"
)

foreach ($dll in $preloads) {
    $path = Join-Path $gameManagedDir $dll
    if (Test-Path $path) {
        [System.Reflection.Assembly]::LoadFile($path) | Out-Null
    }
}

$dllPath = Join-Path $gameManagedDir "Assembly-CSharp.dll"

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
    $type = $assembly.GetType("Light2D")
    if ($null -eq $type) {
        Write-Error "Type Light2D not found"
        return
    }
    
    $methods = $type.GetMethods([System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance -bor [System.Reflection.BindingFlags]::Static)
    Write-Host "Found $($methods.Length) methods in Light2D:"
    foreach ($m in $methods | Sort-Object Name) {
        $params = ($m.GetParameters() | ForEach-Object { "$($_.ParameterType.Name) $($_.Name)" }) -join ", "
        Write-Host "  $($m.ReturnType.Name) $($m.Name)($params)"
    }
} catch {
    Write-Error "Error: $_"
} finally {
    [System.AppDomain]::CurrentDomain.remove_AssemblyResolve($resolveHandler)
}
