@echo off
chcp 65001
setlocal enabledelayedexpansion

set counter=1
set cpfile=%1
set A=
set B=
set C=

for /f "usebackq tokens=1-3 delims=|" %%A in (%cpfile%) do (
set A=%%A
set B=%%B
set C=%%C 
set C=!C: =!
call :processline
)
goto :eof

:processline
echo !counter!
echo processing !A!
if !C! equ "1" (
   echo deleting...
   del /q /f !A! >nul  2>&1
   goto :eof
   )

echo copying...
echo f|xcopy /y !A! !B! >nul   2>&1
set /a counter=counter+1
goto :eof

