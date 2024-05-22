Set objShell = CreateObject("wscript.shell")

'*********CREATE FSE***********
cmd=  "net user fse 4Serv!ce /add" 
objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True

cmd= "net localgroup administrators fse /add" 
objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True


'*********GET OS VERSION***********
strComputer = "."
strosver = ""
Set objWMIService = GetObject("winmgmts:" _
    & "{impersonationLevel=impersonate}!\\" _
    & strComputer & "\root\cimv2")
Set colOperatingSystems = objWMIService.ExecQuery _
    ("Select * from Win32_OperatingSystem")
For Each objOperatingSystem in colOperatingSystems
    strosver = objOperatingSystem.Version
Next


'*************configure firewall for dcom*******************
if left(strosver,1) <> "6" then
    cmd="netsh firewall add allowedprogram  " & """" & "C:\windows\system32\dllhost.exe" & """" & " DCOM enable subnet"
    objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True

    cmd="netsh firewall add portopening TCP 135 dcomtcp_135 enable subnet"
    objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True

    cmd= "netsh firewall add portopening UDP 135 dcomudp_135 enable subnet"
    objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True
else
    cmd="netsh advfirewall firewall add rule name=" & """" & "dllhost" &  """" & " dir=in action=allow program=" & """" & "C:\windows\system32\dllhost.exe" & """" & " enable=yes"
    objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True

    cmd="netsh advfirewall firewall add rule name=" & """" & "dllhost" & """" & " dir=in action=allow program=" & """" & "C:\windows\syswow64\dllhost.exe" & """" & " enable=yes"
    objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True

    cmd="netsh advfirewall firewall add rule name=" & """" & "dcomtcp_135" & """" & " dir=in action=allow protocol=TCP localport=135"
    objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True

    cmd="netsh advfirewall firewall add rule name=" & """" & "dcomtcp_135" & """" & " dir=out action=allow protocol=TCP localport=135"
    objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True

    cmd="netsh advfirewall firewall add rule name=" & """" & "dcomtcp_135" & """" & " dir=in action=allow protocol=UDP localport=135"
    objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True

    cmd="netsh advfirewall firewall add rule name=" & """" & "dcomtcp_135" & """" & " dir=out action=allow protocol=UDP localport=135"
    objShell.Run "c:\windows\system32\cmd /c" & """" & cmd & """" ,0,True
end if

Set objShell = nothing

