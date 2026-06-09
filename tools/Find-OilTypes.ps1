$gameManagedDir = "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed"
$dllPath = "d:\Documents\Klei\OxygenNotIncluded\mods\Local\_source\sgt_imalas_source\PublicisedAssembly\Assembly-CSharp_public.dll"

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
    $assembly = [System.Reflection.Assembly]::LoadFrom($dllPath)
    $types = $assembly.GetTypes()
} catch [System.Reflection.ReflectionTypeLoadException] {
    Write-Warning "Type load exception occurred. Loader exceptions:"
    $_.Exception.LoaderExceptions | ForEach-Object { Write-Warning $_.Message }
    $types = $_.Exception.Types | Where-Object { $null -ne $_ }
} catch {
    Write-Error $_
    return
} finally {
    [System.AppDomain]::CurrentDomain.remove_AssemblyResolve($resolveHandler)
}

$results = $types | Where-Object { $_.Name -like "*Oil*" -or $_.Name -like "*Reservoir*" -or $_.Name -like "*Well*" }
foreach ($t in $results) {
    $baseName = if ($t.BaseType) { $t.BaseType.FullName } else { "None" }
    Write-Host "$($t.FullName) (Base: $baseName)"
}
