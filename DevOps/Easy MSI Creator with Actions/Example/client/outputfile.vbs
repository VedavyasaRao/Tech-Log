Class COM_LB_SillyCalc_WSC_1

    Private remoteobj

    Public Sub CreateComObject(remotepc)
        if remotepc = "" then
            set remoteobj = Createobject("SillyCalc.WSC")
        else
            set remoteobj = Createobject("SillyCalc.WSC",remotepc)
        End If
    End Sub

    Public Sub SetObject(premoteobj)
        set remoteobj = premoteobj
    End Sub

    Public Property Get CLSID
        CLSID = "{502b2117-d5ac-41b2-9441-b466932f613f}"
    End Property

    Public Function add(op1, op2 )
        add = remoteobj.add(op1,  op2  )
    End Function


End Class


Class VBCOMAdminHelper

    Public Sub StartStopApplication(remotepc, clsid, bstart )
		Set catalog = Createobject("COMAdmin.COMAdminCatalog")
	    if (remotepc <> "") Then
	        Call catalog.Connect(remotepc)
	    End If
	
	    Set applications = catalog.GetCollection("Applications")
	    Call applications.Populate()
	
	    for i = 0 To  applications.Count-1
	        Set application = applications.item(i)
	        Set components = applications.GetCollection("Components", application.Key)
	        Call components.Populate()
	        for  j = 0 To components.Count-1
	            Set component = components.item(j)
	            If (StrComp(clsid, component.Value("CLSID"), 1) = 0) Then
	                if (bstart) Then
	                    Call catalog.StartApplication(application.Name)
	                Else
	                    Call catalog.ShutdownApplication(application.Name)
	                End If    
	                Exit sub
	            End If
	        Next
	
	    Next
	End Sub

    Public Sub StartStopApplication_SillyCalc_WSC_1(remotepc, bstart )
        call StartStopApplication(remotepc,  "{502b2117-d5ac-41b2-9441-b466932f613f}",  bstart  )
    End Sub

End Class

set x =  new COM_LB_SillyCalc_WSC_1
x.CreateComObject ""
wscript.echo  "add (10,20) =  " & x.add(10,20)
