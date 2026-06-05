# Kiáº¿n TrÃºc Quáº£n LÃ½ Mod Trong Oxygen Not Included (Báº£n Crack & DLC)

- **MÃ£ tÃ i liá»‡u**: `docs/020-architecture/002-ONI-Mod-Management-Architecture.md`
- **TÃ¡c giáº£**: Antigravity (AI Coding Assistant)
- **NgÃ y táº¡o**: 2026-05-31
- **Tráº¡ng thÃ¡i**: ÄÃ£ phÃª duyá»‡t & LÆ°u trá»¯ Workspace

---

## 1. Vá»‹ TrÃ­ Cáº¥u HÃ¬nh Mods (Mod Configuration Path)
Game *Oxygen Not Included* (ká»ƒ cáº£ báº£n Steam chÃ­nh thá»©c hay báº£n báº» khÃ³a/crack sá»­ dá»¥ng Steam Emulator nhÆ° Goldberg, SmartSteamEmu...) Ä‘á»u sá»­ dá»¥ng cÆ¡ cháº¿ gá»i API há»‡ thá»‘ng cá»§a Windows Ä‘á»ƒ truy xuáº¥t thÆ° má»¥c lÆ°u trá»¯ cÃ¡ nhÃ¢n.

TrÃªn mÃ¡y cá»§a ngÆ°á»i dÃ¹ng hiá»‡n táº¡i, thÆ° má»¥c `Documents` Ä‘Ã£ Ä‘Æ°á»£c há»‡ Ä‘iá»u hÃ nh chuyá»ƒn hÆ°á»›ng sang á»• `D:\`. Do Ä‘Ã³, Ä‘Æ°á»ng dáº«n cáº¥u hÃ¬nh mod duy nháº¥t hoáº¡t Ä‘á»™ng lÃ :
ðŸ‘‰ **`D:\Documents\Klei\OxygenNotIncluded\mods\mods.json`**

*(KhÃ´ng há» cÃ³ file cáº¥u hÃ¬nh trÃ¹ng láº·p trong thÆ° má»¥c áº©n `%USERPROFILE%\AppData\Local` hay `Roaming`)*.

---

## 2. CÆ¡ Cháº¿ LÆ°u Tráº¡ng ThÃ¡i Báº­t/Táº¯t Mod (Mod Activation Mechanism)
ÄÃ¢y lÃ  kiáº¿n thá»©c ká»¹ thuáº­t quan trá»ng nháº¥t vá» cáº¥u trÃºc dá»¯ liá»‡u JSON cá»§a game trong ká»· nguyÃªn há»— trá»£ DLC (*Spaced Out!* vÃ  *Frosty Planet Pack*):

### A. Thuá»™c tÃ­nh `"enabled"` (Boolean: `true` / `false`)
- **TÃ¡c dá»¥ng**: Chá»‰ Ä‘Æ°á»£c game sá»­ dá»¥ng khi cháº¡y á»Ÿ phiÃªn báº£n **Vanilla gá»‘c** (khÃ´ng kÃ­ch hoáº¡t báº¥t ká»³ DLC nÃ o).
- **Tráº¡ng thÃ¡i hiá»‡n táº¡i**: Khi cháº¡y á»Ÿ cháº¿ Ä‘á»™ DLC, game sáº½ bá» qua giÃ¡ trá»‹ nÃ y vÃ  máº·c Ä‘á»‹nh gÃ¡n lÃ  `false` cho toÃ n bá»™ cÃ¡c mod trong tá»‡p `mods.json`.

### B. Máº£ng `"enabledForDlc"` (Array of Strings)
- **TÃ¡c dá»¥ng**: LÆ°u giá»¯ tráº¡ng thÃ¡i kÃ­ch hoáº¡t cá»§a mod Ä‘á»‘i vá»›i tá»«ng DLC cá»¥ thá»ƒ.
- **MÃ£ ID cá»§a DLC**:
  - **`EXPANSION1_ID`**: DLC *Spaced Out!* (Máº£nh vá»¡ khÃ´ng gian).
  - **`DLC2_ID`**: DLC *The Frosty Planet Pack* (HÃ nh tinh bÄƒng giÃ¡).
- **Quy táº¯c kÃ­ch hoáº¡t**:
  - **Báº¬T MOD**: Náº¿u máº£ng chá»©a pháº§n tá»­ ID cá»§a DLC Ä‘Ã³. VÃ­ dá»¥: `"enabledForDlc": ["EXPANSION1_ID"]`.
  - **Táº®T MOD**: Náº¿u máº£ng trá»‘ng rá»—ng `"enabledForDlc": []`.

---

## 3. Cáº¥u TrÃºc File `mods.json` Thá»±c Táº¿
VÃ­ dá»¥ vá» má»™t pháº§n tá»­ mod Ä‘ang Ä‘Æ°á»£c **Báº­t** cho DLC *Spaced Out!*:

```json
{
  "label": {
    "distribution_platform": 0,
    "id": "FastTrack",
    "title": "Fast Track",
    "version": 123456789
  },
  "status": 1,
  "enabled": false,               // Bá»‹ bá» qua á»Ÿ cháº¿ Ä‘á»™ DLC
  "enabledForDlc": [
    "EXPANSION1_ID"               // MOD ÄANG ÄÆ¯á»¢C Báº¬T CHO DLC SPACED OUT!
  ],
  "crash_count": 0,
  "reinstall_path": null,
  "staticID": "PeterHan.FastTrack"
}
```

---

## 4. CÃ¡ch Báº­t/Táº¯t Mod Thá»§ CÃ´ng Báº±ng Code (Bypass Game UI)
Náº¿u muá»‘n kÃ­ch hoáº¡t hoáº·c vÃ´ hiá»‡u hÃ³a nhanh má»™t báº£n mod (vÃ­ dá»¥: kÃ­ch hoáº¡t mod vá»«a cÃ i Ä‘áº·t `ChooseNeuralVacillator` hoáº·c `BiggerCameraZoomOut`) mÃ  khÃ´ng cáº§n má»Ÿ giao diá»‡n game, nhÃ  phÃ¡t triá»ƒn hoáº·c AI Agent cÃ³ thá»ƒ cháº¡y Ä‘oáº¡n mÃ£ PowerShell sau Ä‘á»ƒ cáº­p nháº­t trá»±c tiáº¿p tá»‡p `mods.json`:

### Lá»‡nh PowerShell báº­t mod `ChooseNeuralVacillator` cho DLC Spaced Out!:
```powershell
$path = "D:\Documents\Klei\OxygenNotIncluded\mods\mods.json"
$json = Get-Content $path -Raw | ConvertFrom-Json
$targetMod = $json.mods | Where-Object { $_.label.id -eq "ChooseNeuralVacillator" }
if ($targetMod) {
    $targetMod.enabledForDlc = @("EXPANSION1_ID")
    $json | ConvertTo-Json -Depth 100 | Set-Content $path
    Write-Host "KÃ­ch hoáº¡t thÃ nh cÃ´ng ChooseNeuralVacillator!"
}
```

