Set objShell = CreateObject("Shell.Application" ) 
Set Ag=Wscript.Arguments 

if Ag.Count < 2 then 
msg = "Syntax:" & vbCrlf  & vbCrlf
msg = msg & "Zip:" & vbCrlf
msg = msg &  "MakeZip_Unzip.vbs <fullpath_to _folder>  <fullpath_to_zipfile>" & vbCrlf
msg = msg &  "Example:"& vbCrlf 
msg = msg &  "MakeZip_Unzip.vbs d:\temp\reports d:\reports.zip" & vbCrlf& vbCrlf
msg = msg &  "UnZip:" & vbCrlf
msg = msg &  "MakeZip_Unzip.vbs <fullpath_to_zipfile> <fullpath_to _folder>" & vbCrlf
msg = msg &  "Example:"& vbCrlf 
msg = msg &  "MakeZip_Unzip.vbs  d:\reports.zip  d:\temp\reports" 
MsgBox msg,0,"MakeZip_Unzip"
wscript.quit 0
end if

Set objFSO = CreateObject("Scripting.FileSystemObject")
if InStr(Ag(0),".zip") <> 0 then
if not objFSO.FolderExists(Ag(1)) then objFSO.CreateFolder Ag(1)
end if

if InStr(Ag(1),".zip") <> 0 then
if not objFSO.FileExists(Ag(1)) then 
Set objFile = objFSO.CreateTextFile(Ag(1))
objFile.close
end if
end if

Set SrcFldr=objShell.NameSpace(Ag(0)) 
Set DestFldr=objShell.NameSpace(Ag(1)) 

dc = DestFldr.Items().count
dcsrc=SrcFldr.Items().count


Set FldrItems=SrcFldr.Items 
DestFldr.CopyHere FldrItems, 64

do 
WScript.Sleep 1000
loop while (DestFldr.Items().count < (SrcFldr.Items().count + dc)) and (DestFldr.Items().count <> (SrcFldr.Items().count ))

