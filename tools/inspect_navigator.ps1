$managedDir = "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed"

# Load dependencies
$dlls = @(
    "UnityEngine.dll",
    "UnityEngine.CoreModule.dll",
    "UnityEngine.PhysicsModule.dll",
    "UnityEngine.Physics2DModule.dll",
    "Assembly-CSharp-firstpass.dll",
    "Assembly-CSharp.dll"
)

foreach ($dll in $dlls) {
    $path = Join-Path $managedDir $dll
    [System.Reflection.Assembly]::LoadFrom($path) | Out-Null
}

$assembly = [AppDomain]::CurrentDomain.GetAssemblies() | Where-Object { $_.GetName().Name -eq "Assembly-CSharp" } | Select-Object -First 1

if ($assembly -ne $null) {
    $type = $assembly.GetType("Navigator")
    if ($type -ne $null) {
        Write-Output "--- Fields ---"
        $type.GetFields([System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance) | 
            ForEach-Object { "$($_.FieldType.FullName) $($_.Name)" }
            
        Write-Output "--- Properties ---"
        $type.GetProperties([System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance) | 
            ForEach-Object { "$($_.PropertyType.FullName) $($_.Name)" }
    } else {
        Write-Output "Navigator type not found in Assembly-CSharp"
    }
} else {
    Write-Output "Assembly-CSharp not found"
}
