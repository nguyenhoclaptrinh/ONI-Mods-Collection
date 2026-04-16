@echo off
setlocal

set "destination=..\Local\TiengViet"

if not exist "%destination%" mkdir "%destination%"

copy "%~dp0*.*" "%destination%"

echo Viet hoa thanh cong
endlocal
