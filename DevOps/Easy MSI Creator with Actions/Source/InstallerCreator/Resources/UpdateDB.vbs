Const msiOpenDatabaseModeDirect = 2 

Const MSIMODIFY_REPLACE = 4 

Dim sPathToMSI, retVal, objWI, objDB, objView, objRecord 



Main


Sub Main 

    Dim objArgs, ArgCount, cArgument, objFS, sArgument 

    retVal = 1 

    'Get the command line parameters. 

    Set objArgs    = WScript.Arguments 

    ArgCount    = objArgs.Count 

    If ArgCount = 0 Then 

        WScript.Quit 

    End If 

	sQuery = "SELECT `ComponentId` FROM `Component` Where `Component`='" & CStr(WScript.Arguments(1)) &"'"
    sPathToMSI = CStr(WScript.Arguments(0)) 

    Set objFS = CreateObject("Scripting.FileSystemObject") 

    If Not objFS.FileExists(sPathToMSI) Then 

        Fail "File: '" & sPathToMSI & "' doesn't exist!" 

    End If 

    Set objWI = CreateObject("WindowsInstaller.Installer") 

    CheckError 

    Set objDB = objWI.OpenDatabase(sPathToMSI, msiOpenDatabaseModeDirect) 

    CheckError 

    Set objView = objDB.OpenView(sQuery) 

    CheckError 

    objView.Execute 

    Set objRecord = objView.Fetch 

    If Not objRecord Is Nothing Then 

        objRecord.StringData(1)=CStr(WScript.Arguments(2))

        objView.Modify MSIMODIFY_REPLACE, objRecord 

        CheckError 

        objView.Close 

        objDB.Commit 
        
        WScript.Quit retVal

    Else 

        Fail "No Matching Records found" 

    End If 


End Sub 

Sub CheckError  

    Dim message, errRec  

    If Err = 0 Then Exit Sub End If 

    message = Err.Source & " " & Hex(Err) & ": " & Err.Description 

    If Not objWI Is Nothing Then  

        Set errRec = objWI.LastErrorRecord  

        If Not errRec Is Nothing Then message = message & vbLf & errRec.FormatText  

    End If 

    Fail message 

End Sub 

Sub Fail(message) 

    WScript.Echo message 

    WScript.Quit 2 

End Sub 
