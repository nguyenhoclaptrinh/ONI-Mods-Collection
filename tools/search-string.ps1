param (
    [string]$searchString = "wasn't a FetchChore",
    [string]$folderPath = "d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed"
)

# Thử cả UTF-8, ASCII và Unicode (UTF-16LE)
Get-ChildItem -Path $folderPath -Filter "*.dll" -Recurse | ForEach-Object {
    try {
        $bytes = [System.IO.File]::ReadAllBytes($_.FullName)
        
        # 1. Thử Unicode (UTF-16 LE)
        $txtUnicode = [System.Text.Encoding]::Unicode.GetString($bytes)
        if ($txtUnicode.Contains($searchString)) {
            Write-Host "Found '$searchString' (Unicode) in: $($_.FullName)"
            return
        }

        # 2. Thử UTF-8
        $txtUTF8 = [System.Text.Encoding]::UTF8.GetString($bytes)
        if ($txtUTF8.Contains($searchString)) {
            Write-Host "Found '$searchString' (UTF8) in: $($_.FullName)"
            return
        }
    } catch {
        # Silent fail for locked files
    }
}
