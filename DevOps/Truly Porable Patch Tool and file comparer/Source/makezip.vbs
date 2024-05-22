Set objShell = CreateObject("Shell.Application" ) 
Set Ag=Wscript.Arguments 
Set SrcFldr=objShell.NameSpace(Ag(0)) 
Set DestFldr=objShell.NameSpace(Ag(1)) 
dc = DestFldr.Items().count

Set FldrItems=SrcFldr.Items 
DestFldr.CopyHere FldrItems, 64

do 
WScript.Sleep 1000
loop while DestFldr.Items().count < (SrcFldr.Items().count + dc)
