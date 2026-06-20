param(
    [string]$AsmPath = "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll",
    [string[]]$TypeNames = @("SuitLocker","CopyBuildingSettings","SuitMarker","JetSuitLocker","LeadSuitLocker")
)

$asm = [System.Reflection.Assembly]::LoadFrom($AsmPath)
$flags = [System.Reflection.BindingFlags]::Public -bor
         [System.Reflection.BindingFlags]::NonPublic -bor
         [System.Reflection.BindingFlags]::Instance -bor
         [System.Reflection.BindingFlags]::Static -bor
         [System.Reflection.BindingFlags]::DeclaredOnly

foreach ($name in $TypeNames) {
    $type = $asm.GetType($name)
    if (-not $type) {
        Write-Host "=== $name : NOT FOUND ===" -ForegroundColor Red
        continue
    }
    Write-Host ""
    Write-Host "============================================================" -ForegroundColor Cyan
    Write-Host "TYPE: $name" -ForegroundColor Cyan
    Write-Host "  BaseType: $($type.BaseType.FullName)"
    Write-Host "  Interfaces: $(($type.GetInterfaces() | ForEach-Object { $_.Name }) -join ', ')"
    Write-Host ""

    Write-Host "--- FIELDS ---"
    foreach ($f in $type.GetFields($flags)) {
        $smod = if ($f.IsStatic) { 'static' } else { 'inst  ' }
        Write-Host "  [$smod] $($f.FieldType.Name) $($f.Name)"
    }

    Write-Host "--- PROPERTIES ---"
    foreach ($p in $type.GetProperties($flags)) {
        Write-Host "  $($p.PropertyType.Name) $($p.Name)"
    }

    Write-Host "--- METHODS ---"
    foreach ($m in $type.GetMethods($flags)) {
        $paramStr = ($m.GetParameters() | ForEach-Object { $_.ParameterType.Name + " " + $_.Name }) -join ", "
        Write-Host "  $($m.ReturnType.Name) $($m.Name)($paramStr)"
    }
}
