@echo off
setlocal enabledelayedexpansion
pushd %~dp0
echo[
call 7z a -mhe=on -p%3 -v4g -spf -mx9 %1 @%2
popd
