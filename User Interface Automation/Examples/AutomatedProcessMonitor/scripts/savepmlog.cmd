@echo on
setlocal enabledelayedexpansion
del C:\temp\procmon\capture\snapshot.bmp>nul
del C:\temp\procmon\capture\logfile.csv>nul

tasklist /fi "imagename eq Procmon64.exe" | find /i "Procmon64.exe">nul
if !errorlevel! equ 0 start "" /wait "%~dp0\..\bin\Procmon64.exe" /Terminate
start "" "%~dp0\..\bin\Procmon64.exe" /quiet /openlog C:\temp\procmon\pmlogs\uiatest.PML
echo saving....
start "" /min /wait "%~dp0\..\bin\savelog.exe"
call :rename_logfile
goto :eof

:rename_logfile
set logfile=logfile_%date%_%time%
set logfile=%logfile::=_%
set logfile=%logfile:/=_%
find /c /v ""  C:\temp\procmon\capture\logfile.CSV | find /i ".CSV: 1">nul
if !errorlevel! equ 0 (
del C:\temp\procmon\capture\snapshot.bmp>nul
del C:\temp\procmon\capture\logfile.csv>nul
echo %date% %time% not found >> C:\temp\procmon\capture\results.log
) else (
move C:\temp\procmon\capture\snapshot.bmp "C:\temp\procmon\capture\%logfile%".bmp
move C:\temp\procmon\capture\logfile.csv "C:\temp\procmon\capture\%logfile%".csv
echo %date% %time% found >> C:\temp\procmon\capture\results.log
)
exit /b
