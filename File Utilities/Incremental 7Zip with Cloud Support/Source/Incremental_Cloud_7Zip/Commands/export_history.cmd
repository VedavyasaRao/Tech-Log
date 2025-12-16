@echo off
echo generating....
setlocal enabledelayedexpansion
set abc=%1 %2 %3 %4 %5
!abc! > %7
del /q %6

for /f "tokens=5* delims= " %%A in ('type %7') do (
echo processing %%A
echo "%%A" | find /i "\" >nul
if !errorlevel! equ 0 (
@echo %%A%%B >> %6
 ) else ( 
@echo %%B >> %6
))

