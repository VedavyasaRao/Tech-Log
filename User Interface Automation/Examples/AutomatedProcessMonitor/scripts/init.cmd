@echo on
rd /s /q c:\temp\procmon
md c:\temp\procmon\capture
md c:\temp\procmon\pmlogs
schtasks /create  /f /sc onlogon /tn startpm /it /ru rvvya /tr "%~dp0\savepmlog.cmd"
schtasks /create  /f /sc minute /mo 30  /tn savepmlog /it /ru rvvya /tr "%~dp0\savepmlog.cmd"
start "" "%~dp0\..\bin\Procmon64.exe" /accepteula /terminate
start "" "%~dp0\..\bin\Procmon64.exe" /quiet /loadconfig "%~dp0\ProcmonConfiguration.pmc"


