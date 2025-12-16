@echo off
setlocal enabledelayedexpansion
chcp 65001
set exe7z="%~dp0\7z"
if "%1" equ "zip" (
	cd /d %2
	echo  "zipping"
	call  %exe7z% u -mhe=on -p%5 -v%6m -ms=%7 -spf -mx9 %3 @%4
)

if "%1" equ "unzipdiff" (
	call  %exe7z%   x  -y  -p%5 -o%2 %3  @%4
)

if "%1" equ "unzipsame" (
	call  %exe7z%   x  -y -spf -p%5  %3  @%4
)
