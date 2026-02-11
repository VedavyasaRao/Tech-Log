cd /d "%~dp0"
if not exist ..\Rvvpm\cef (
7z x -o"..\Rvvpm" "..\Rvvpm\cef.part1.rar"
)
if not exist ..\packages (
7z x -o".." "..\Packages.part01.rar"
)
set errorlevel=0

