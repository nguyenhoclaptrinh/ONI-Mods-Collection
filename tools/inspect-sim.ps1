param (
    [string]$assemblyPath = "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll",
    [string]$typeName = "SolidTransferArm",
    [string]$methodName = "Sim"
)

$managedDir = Split-Path $assemblyPath

# Load all DLLs in Managed folder first
Get-ChildItem -Path $managedDir -Filter "*.dll" | ForEach-Object {
    try {
        [System.Reflection.Assembly]::LoadFrom($_.FullName) > $null
    } catch {
        # Ignore load errors for some system assemblies
    }
}

try {
    $asm = [System.Reflection.Assembly]::LoadFrom($assemblyPath)
    $type = $asm.GetType($typeName)
    if ($type -eq $null) {
        Write-Host "Type $typeName not found."
        exit 1
    }

    $flags = [System.Reflection.BindingFlags]::Public -or [System.Reflection.BindingFlags]::NonPublic -or [System.Reflection.BindingFlags]::Instance -or [System.Reflection.BindingFlags]::Static
    $method = $type.GetMethod($methodName, $flags)
    if ($method -eq $null) {
        Write-Host "Method $methodName not found in $typeName."
        exit 1
    }

    Write-Host "=== Inspecting $typeName.$methodName ==="
    $body = $method.GetMethodBody()
    if ($body -eq $null) {
        Write-Host "Method body is null (maybe it is extern or pinvoke)."
        exit 1
    }

    $il = $body.GetILAsByteArray()
    Write-Host "IL Byte length: $($il.Length)"
    
    # In ra dạng Hex
    $hex = [System.BitConverter]::ToString($il)
    Write-Host "IL Hex bytes:"
    Write-Host $hex
} catch {
    Write-Error "Error: $_"
}
