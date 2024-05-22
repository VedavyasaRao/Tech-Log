using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using COMAdmin;
using COMLibHelper;

namespace ComAppHelper
{
    class Program
    {
        enum appoptions { install = 1, uninstall = 2, start = 4, stop = 8 };
        static appoptions apopts = 0;
        static string appname = "";
        static List<string> data = new List<string>();
        static string filename = "";
        static bool breg = false;
        static bool bgac = false;
        static bool bunreg = false;
        static bool bungac = false;
        static bool brunforever = false;
        static bool bsvc = false;
        
        static private void performccreatecomappop(appoptions apopts)
        {
            try
            {
                bool bins = ((apopts & appoptions.install) != 0);
                bool bunins = ((apopts & appoptions.uninstall) != 0);
                bool bstart = ((apopts & appoptions.start) != 0);
                bool bstop = ((apopts & appoptions.stop) != 0);

                COMAdminCatalog catalog = new COMAdminCatalog();
                COMAdminCatalogCollection applications = catalog.GetCollection("Applications") as COMAdminCatalogCollection;
                applications.Populate();
                if (bins || bunins || bstart || bstop || breg)
                {

                    for (int i = 0; i < applications.Count; ++i)
                    {
                        COMAdminCatalogObject app = applications.get_Item(i) as COMAdminCatalogObject;
                        if (app.Name.ToString() == appname)
                        {
                            if (bstart)
                            {
                                catalog.StartApplication(appname);
                                return;
                            }

                            catalog.ShutdownApplication(appname);
                            if (bins || bunins)
                            {
                                applications.Remove(i);
                                applications.SaveChanges();
                            }
                            break;
                        }
                    }


                    if (bins) 
                    {
                        applications.Populate();

                        COMAdminCatalogObject application = applications.Add() as COMAdminCatalogObject;
                        application.Value["ID"] = Guid.NewGuid().ToString("B");
                        application.Value["Name"] = appname;
                        application.Value["RunForever"] = brunforever;
                        applications.SaveChanges();

                        if (System.Environment.OSVersion.Version.Major > 5)
                        {
                            COMAdminCatalogCollection roles = (COMAdminCatalogCollection)applications.GetCollection("Roles", application.Key);
                            roles.Populate();
                            COMAdminCatalogObject role = roles.Add() as COMAdminCatalogObject;
                            role.Value["Name"] = "DefaultRole";
                            roles.SaveChanges();
                            COMAdminCatalogCollection users = (COMAdminCatalogCollection)roles.GetCollection("UsersInRole", role.Key);
                            users.Populate();
                            COMAdminCatalogObject user = users.Add() as COMAdminCatalogObject;
                            user.Value["User"] = "BUILTIN\\Administrators";
                            users.SaveChanges();
                        }
                        applications.SaveChanges();
                    }
                }

                if (breg || bunreg)
                {
                    if (data.Count == 0)
                    {
                        if (filename == "")
                            ShowSyntax();

                        List<KeyValuePair<string, string>> progclsidlst = new List<KeyValuePair<string, string>>();
                        if (filename.ToLower().IndexOf(".wsc") != -1)
                        {
                            progclsidlst = CodeGenHelper.GetclsidsfromWSC(filename);

                        }
                        else
                        {
                            progclsidlst = CodeGenHelper.GetclsidsfromAssembly(filename);
                            if (progclsidlst.Count == 0)
                            {
                                progclsidlst = CodeGenHelper.GetclsidsfromTLB(filename);
                            }
                        }

                        foreach (var kv in progclsidlst)
                        {
                            data.Add(kv.Key);
                        }
                    }

                    foreach (string d in data)
                    {
                        if (breg)
                            catalog.ImportComponent(appname, d);
                    }

                }
                else if (bgac || bungac)
                {
                    if (bgac)
                        catalog.InstallComponent(appname, filename, "", "");
                }

                applications.SaveChanges();

            }
            catch 
            {
            }

        }



        static private void addtosvcconfig()
        {
            try
            {
                string clrruntime = System.Reflection.Assembly.GetExecutingAssembly().ImageRuntimeVersion;
                string ComSvcConfigpath="";
                if (clrruntime.Contains("v2."))
                    ComSvcConfigpath = @"%windir%\Microsoft.NET\Framework\v3.0\Windows Communication Foundation\ComSvcConfig.exe";
                else
                    ComSvcConfigpath = @"%windir%\Microsoft.NET\Framework\v4.0.30319\ComSvcConfig.exe";


                List<KeyValuePair<string, string>> objs = CodeGenHelper.GetclsidsfromAssembly(filename);
                foreach (var o in objs)
                {

                    List<string> itfs = CodeGenHelper.GetinterfacesforclsidfromAssembly(filename, o.Key);
                    foreach(var itf in itfs)
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = System.Environment.ExpandEnvironmentVariables(ComSvcConfigpath);
                        p.StartInfo.Arguments = String.Format("/install /application:{0} /contract:\"{1},{2}\"  /hosting:complus /mex", appname, o.Key, itf);
                        p.Start();
                        p.WaitForExit();
                    }
                }


            }
            catch 
            {
            }

        }

        static private void ShowSyntax()
        {
            Console.WriteLine("Syntax:");
            Console.WriteLine("ComAppHelper -{ins|unins|start|stop} -app:<app name> -clsprogid{progid | clsid} -{reg|gac|unreg|ungac} -runforever -svc [filename]");
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowSyntax();
                return;
            }

            for (int i = 0; i < args.Length; ++i)
            {
                string a = args[i];
                if (a.ToLower().Contains("-app:"))
                {
                    appname = a.Substring(a.IndexOf(':') + 1);
                }
                else if (a.ToLower().Contains("-ins"))
                {
                    apopts |= appoptions.install;
                }
                else if (a.ToLower().Contains("-unins"))
                {
                    apopts |= appoptions.uninstall;
                }
                else if (a.ToLower().Contains("-start"))
                {
                    apopts |= appoptions.start;
                }
                else if (a.ToLower().Contains("-stop"))
                {
                    apopts |= appoptions.stop;
                }
                else if (a.ToLower().Contains("-clsprogid:"))
                {
                    data.Add(a.Substring(a.IndexOf(':') + 1));
                }
                else if (a.ToLower().Contains("-reg"))
                {
                    breg = true;
                }
                else if (a.ToLower().Contains("-gac"))
                {
                    bgac = true;
                }
                else if (a.ToLower().Contains("-unreg"))
                {
                    bunreg = true;
                }
                else if (a.ToLower().Contains("-ungac"))
                {
                    bungac = true;
                }
                else if (a.ToLower().Contains("-runforever"))
                {
                    brunforever = true;
                }
                else if (a.ToLower().Contains("-svc"))
                {
                    bsvc = true;
                }
                else
                {
                    filename = a;
                }
            }

            if (filename.ToLower().IndexOf(".wsc") == -1)
            {

                if (breg)
                    new System.EnterpriseServices.Internal.Publish().RegisterAssembly(filename);
                else if (bunreg)
                    new System.EnterpriseServices.Internal.Publish().UnRegisterAssembly(filename);
                else if (bgac)
                    new System.EnterpriseServices.Internal.Publish().GacInstall(filename);
                else if (bungac)
                    new System.EnterpriseServices.Internal.Publish().GacRemove(filename);

            }

            if (appname == "")
                return;
            
            performccreatecomappop(apopts);
            if (bsvc)
            {
                addtosvcconfig();
                performccreatecomappop(appoptions.start);
            }

        }
    }
}
