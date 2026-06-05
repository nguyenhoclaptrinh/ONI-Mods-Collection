# Cấu hình Môi trường Phát triển Cục bộ (Local Developer Environment)

> [!IMPORTANT]
> Tài liệu này chứa các thông tin cấu hình môi trường cục bộ dành cho các AI Agent.
> **Tệp này đã được cấu hình trong `.gitignore` để tránh bị commit lên Git.**

## 1. Thông tin Game (Oxygen Not Included)
- **Đường dẫn cài đặt game**: `d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793`
- **Đường dẫn Game Assemblies (Managed DLLs)**: `d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed`
- **Phiên bản build game hiện tại**: `U56-706793`

## 2. Thông tin .NET SDK & Môi trường Biên dịch
- **Phiên bản dotnet CLI**: `10.0.204`
- **Đường dẫn cài đặt dotnet**: `C:\Program Files\dotnet`
- **Phiên bản Runtime khả dụng**:
  - `Microsoft.NETCore.App` version `10.0.8` (tại `C:\Program Files\dotnet\shared\Microsoft.NETCore.App`)

## 3. Cấu hình Workspace & Deployment
- **Đường dẫn Mod Local Workspace (Source & Deploy)**: `d:\Documents\Klei\OxygenNotIncluded\mods\Local`
- **Cấu hình MSBuild `.csproj.user` khuyên dùng cho các mod C#**:
  ```xml
  <Project>
    <PropertyGroup>
      <GameLibDir>d:\Games\OxygenNotIncludedCrack\oxygen.not.included.v706793\OxygenNotIncluded_Data\Managed</GameLibDir>
    </PropertyGroup>
  </Project>
  ```
- **Thư mục output deploy mod mặc định**: `d:\Documents\Klei\OxygenNotIncluded\mods\Local\[ModName]`
