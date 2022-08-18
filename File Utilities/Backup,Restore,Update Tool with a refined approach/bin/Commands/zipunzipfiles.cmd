@echo off
setlocal enabledelayedexpansion
set exe7z="%~dp0\..\7z"

if "%1" equ "zip" (
	cd /d %2
	call  %exe7z% a -mhe=on -p%5 -v4g -spf -mx9 %3 @%4
)

if "%1" equ "unzipdiff" (
	call  %exe7z%   x  -y  -p%5 -o%2 %3  @%4
)

if "%1" equ "unzipsame" (
	call  %exe7z%   x  -y -spf -p%5  %3  @%4
)
