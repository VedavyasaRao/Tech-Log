@echo off
chcp 65001
setlocal enabledelayedexpansion

set counter=1
set cpfile=%2
set logfile=%4

set A=
set B=
set C=

del /q /f !logfile!

if %1 equ "prepare" (
if exist %3 (
echo creating folders...
echo md %2 >>!logfile!
md %2 >>!logfile!  2>>&1
echo robocopy %3 %2  /e /xf * >>!logfile!
robocopy %3 %2  /e /xf * >>!logfile!  2>>&1
)
goto :eof
)


if %1 equ "move" (
for /f "usebackq  tokens=1-3 delims=|" %%A in (%cpfile%) do (
set A=%%A
set B=%%B
echo processing !A!
echo !counter! / %3
set /a counter=counter+1
echo move  !A! !B! >>!logfile!
move  !A! !B! >>!logfile!  2>>&1 
)
goto :eof
)


if %1 equ "dupcopy" (
for /f "usebackq tokens=1-3 delims=|" %%A in (%cpfile%) do (
set A=%%A
set B=%%B
set C=%%C 
set C=!C: =!
echo !counter! / %3
echo processing !A!
if !C! equ "1" (
   echo deleting...
   echo del /q /f !A! >>!logfile!
   del /q /f !A! >>!logfile!  2>>&1
   set /a counter=counter+1
   ) else (
echo copying...
echo xcopy /y !A! !B! >>!logfile!
echo f|xcopy /y !A! !B! >>!logfile!  2>>&1
if not exist !B! (
for %%A in (!B!) do (
set dstdir="%%~dpA"
if  not exist !dstdir! (
 echo md !dstdir! >>!logfile!
 md !dstdir!  >>!logfile!  2>>&1
 )
)
echo copy /y !A! !B! >>!logfile!
copy /y !A! !B! >>!logfile!  2>>&1
)
)
)
goto :eof
)

if %1 equ "copy" (
for /f "usebackq tokens=1-3 delims=|" %%A in (%cpfile%) do (
set A=%%A
set B=%%B
set C=%%C 
set C=!C: =!

echo !counter! / %3
echo processing !A!
echo copying...
set /a counter=counter+1
echo xcopy /y !A! !B! >>!logfile!
echo f|xcopy /y !A! !B! >>!logfile!  2>>&1
if not exist !B! (
for %%A in (!B!) do (
set dstdir="%%~dpA"
if  not exist !dstdir! (
echo md !dstdir! >>!logfile!
md !dstdir! >>!logfile!  2>>&1
)
)
echo copy /y !A! !B!>>!logfile!
copy /y !A! !B! >>!logfile!  2>>&1
)
)
goto :eof
)

if %1 equ "makelink" (
for /f "usebackq tokens=1-2 delims=|" %%A in (%cpfile%) do (
set A=%%A
set B=%%B
 echo makelinking %%A 
 echo mklink /J %%A %%B >>!logfile!
 mklink /J %%A %%B >>!logfile!  2>>&1
 )
goto :eof
)



