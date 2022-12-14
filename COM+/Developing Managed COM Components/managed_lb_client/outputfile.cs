using System;
using System.Reflection;

namespace autogenerated
{
    class COM_LB_CalcServer_Calucalator
    {
        dynamic remoteobj;

        public COM_LB_CalcServer_Calucalator(string remotepc="")
        {
            remoteobj = (remotepc != "") ? Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0ad70785-5539-4ee0-83d5-37a62cf5318f"), remotepc)) : Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0ad70785-5539-4ee0-83d5-37a62cf5318f")));
        }

        public COM_LB_CalcServer_Calucalator(dynamic remoteobj)
        {
            this.remoteobj = remoteobj;
        }

        public Int32 add( Int32 op1,  Int32 op2 )
        {
            return  remoteobj.add( op1,  op2 );

        }

    }

    class CSComAdmin
    {

        public static void StartStopApplication(string remotepc, string clsid, bool bstart)
        {
            dynamic catalog = Activator.CreateInstance(Type.GetTypeFromProgID("COMAdmin.COMAdminCatalog"));
            if (remotepc != "")
            {
                catalog.Connect(remotepc);
            }

            dynamic applications = catalog.GetCollection("Applications");
            applications.Populate();

            for (int i = 0; i < applications.Count; ++i)
            {
                dynamic application = applications.item(i);
                dynamic components = applications.GetCollection("Components", application.Key);
                components.Populate();
                for (int j = 0; j < components.Count; ++j)
                {
                    dynamic component = components.item(j);
                    if (clsid.ToLower() == ((string)component.Value["CLSID"]).ToLower())
                    {
                        if (bstart)
                            catalog.StartApplication((string)application.Name);
                        else
                            catalog.ShutdownApplication((string)application.Name);
                        return;
                    }
                }

            }
        }

        public static void StartStop_CalcServer_Calucalator(string remotepc, bool bstart)
        {
            StartStopApplication(remotepc,"{0ad70785-5539-4ee0-83d5-37a62cf5318f}",bstart);
        }

    }
    class Program
    {
        public static void Main()
        {
            var lbclient = new COM_LB_CalcServer_Calucalator("127.0.0.1");
            Console.WriteLine("40+6={0}", lbclient.add(40, 6));
        }
    }
}
