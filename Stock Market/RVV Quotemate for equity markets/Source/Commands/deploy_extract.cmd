if not exist "..\..\Deploy\YahooFinance\libcef.dll" (
7z e -o"..\..\Deploy\YahooFinance" "..\..\Deploy\YahooFinance\libcef.rar"
)
if not exist "..\..\Deploy\GoogleNews\libcef.dll" (
7z e -o"..\..\Deploy\GoogleNews" "..\..\Deploy\GoogleNews\libcef.rar"
)
if not exist "..\..\Deploy\RVVPM\libcef.dll" (
7z e -o"..\..\Deploy\RVVPM" "..\..\Deploy\RVVPM\libcef.rar"
)

pause