Set Shell = CreateObject("WScript.Shell")
Set Ag=Wscript.Arguments 

Lnkname=mid(Ag(1), InStrRev (Ag(1),"\")+1)
Lnkname=mid(Lnkname,1,InStrRev (Lnkname,".")-1)
DesktopPath = Shell.SpecialFolders("Desktop") & "\" & LnkName & ".Lnk"
if Ag(0)="-i" Then
    TPath=mid(Ag(1),1,InStrRev (Ag(1),"\")-1)
    Set link = Shell.CreateShortcut(DesktopPath)
    link.Arguments = ""
    link.Description = Lnkname
    link.IconLocation = Ag(1) & ",0"
    link.TargetPath = Ag(1)
    link.WindowStyle = 0
    link.WorkingDirectory = TPath
    link.Save
end if

if Ag(0)="-u" Then
    set fso = CreateObject("Scripting.FileSystemObject")
    fso.DeleteFile(DesktopPath)
end if








