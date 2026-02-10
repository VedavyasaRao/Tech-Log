using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using UITesting.Automated.UIADriver;
using static System.Net.Mime.MediaTypeNames;

namespace UITesting.Automated.UIADriver
{
    class UIAAutomationElement
    {
        static private UIAElement uiaelement;

        static UIAAutomationElement()
        {
            uiaelement = new UIAElement();
        }

        static public UIAElement UIADriver
        {
            get
            {
                return uiaelement;
            }
        }
    }
    class sample
    {
        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string text_Finished_2296_542
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"text_Finished_2296_542\",        \"AEType\":\"text\",        \"AEText\":\"Finished\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"1 1 0 0 0 0 0 1 0 0 0 0 2 0 3 1 0 \",        \"CenterPoint\":\"138,278.5\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2183_271\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"566,360\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string text_Leave_2223_289
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"text_Leave_2223_289\",        \"AEType\":\"text\",        \"AEText\":\"Leave\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"1 1 0 0 0 0 0 1 0 0 0 0 0 0 2 1 \",        \"CenterPoint\":\"60,26\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2183_271\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"566,360\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string text_2348_957
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"text_2348_957\",        \"AEType\":\"text\",        \"AEText\":\"/\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"1 1 0 0 0 0 0 1 0 0 0 0 0 1 1 1 0 4 2 \",        \"CenterPoint\":\"168.5,695\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2183_271\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"566,360\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string text_16_2741_855
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"text_16_2741_855\",        \"AEType\":\"text\",        \"AEText\":\"16: \",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"1 1 0 0 0 0 0 1 0 0 0 0 0 1 1 1 0 4 0 \",        \"CenterPoint\":\"137.5,695\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2615_169\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"566,360\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string text_19_2759_855
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"text_19_2759_855\",        \"AEType\":\"text\",        \"AEText\":\"19\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"1 1 0 0 0 0 0 1 0 0 0 0 0 1 1 1 0 4 1 \",        \"CenterPoint\":\"152.5,695\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2615_169\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"566,360\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string text_Yes_3320_353
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"text_Yes_3320_353\",        \"AEType\":\"text\",        \"AEText\":\"Yes\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"0 5 0 0 0 1 1 0 \",        \"CenterPoint\":\"716,191.5\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2615_169\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"566,360\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string button_Reload_2998_751
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"button_Reload_2998_751\",        \"AEType\":\"button\",        \"AEText\":\"Reload\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"0 0 0 0 1 2 0 2 1 \",        \"CenterPoint\":\"616.5,511\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2426_271\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"564.5,360\"    }}";
            }
        }

    }


    //  ***************Playback Code********************
    class Player
    {
        internal static EventWaitHandle evt;
        static jsonparser jspobj = new jsonparser();
        static sample objsample = new sample();

        static object ParseObj( object injson,  object field)
        {
            return jspobj.ParseObj(injson, field);
        }

        public static void init()
        {
            bool b;
            EventWaitHandleSecurity ws;
            ws = new EventWaitHandleSecurity();
            ws.AddAccessRule(new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), EventWaitHandleRights.FullControl, AccessControlType.Allow));
            evt = new EventWaitHandle(true, EventResetMode.ManualReset, "Global\\OdaCapture", out b, ws);

            var p = Process.GetProcessesByName("inspect");
            if (p.Length == 1)
                p[0].Kill();
            System.Threading.Thread.Sleep(3000);
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.CreateNoWindow = true;
            ps.WindowStyle = ProcessWindowStyle.Hidden;
            ps.FileName = "inspect.exe";
            Process.Start(ps);
            System.Threading.Thread.Sleep(3000);
        }

        public static List<int> preparelist(string args)
        {
            var items = new List<int>();
            var argitems = args.Split(',');
            foreach (var item in argitems)
            {
                if (item.Contains("-"))
                {
                    var parts = item.Split('-');
                    for (int i = int.Parse(parts[0]); i <= int.Parse(parts[1]); ++i)
                        items.Add(i);
                }
                else
                    items.Add(int.Parse(item));
            }
            items.Sort();

            return items;
        }

        public static void launchdebut(string dir, string file)
        {
            System.Diagnostics.Process.Start($"C:\\Program Files (x86)\\NCH Software\\Debut\\debut.exe", $"-hide -sound on -format mp4 -source screen -videodir \"{dir}\" -file \"{file}\"");
        }

        public static void startdebut()
        {
            System.Diagnostics.Process.Start($"C:\\Program Files (x86)\\NCH Software\\Debut\\debut.exe", "-record -hide");
        }

        public static void stopdebut()
        {
            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\NCH Software\Debut\debut.exe", "-stop");
        }

        public static void exitdebut()
        {
            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\NCH Software\Debut\debut.exe", "-exit");
        }

        public static void resetbar()
        {
            UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_2348_957);
            UIAAutomationElement.UIADriver.ProviderGeneric.Click(true, false);
            System.Threading.Thread.Sleep(1000);
            UIAAutomationElement.UIADriver.ProviderGeneric.MoveandClick(68, -4, true, false, false);
        }

        static void presspause()
        {
            UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_2348_957);
            System.Threading.Thread.Sleep(3000);
            UIAAutomationElement.UIADriver.ProviderGeneric.Click(false, false);
            System.Threading.Thread.Sleep(3000);
            UIAAutomationElement.UIADriver.ProviderGeneric.MoveandClick(-108,0,true,false, false);
            System.Threading.Thread.Sleep(3000);
        }

        public static void waitforfile(string dir, string moviefile)
        {
            int fk = 0;
            string[] lst;
            do
            {
                lst = System.IO.Directory.GetFiles(dir, "*.mp4");
                if (lst != null && lst.Length != 0 && lst.Count(f=>f.Contains(moviefile)) != 0)
                    break;
                System.Threading.Thread.Sleep(5000);
            } while (fk++ < 10);
            System.Threading.Thread.Sleep(1000);
        }

        public static void checkfortrty()
        {
            int knt = 0;
            var temps = UIAAutomationElement.UIADriver.SearchOptions;
            do
            {
                UIAAutomationElement.UIADriver.SearchOptions = "VPAC";
                bool b = UIAAutomationElement.UIADriver.SetAutomationElement(objsample.button_Reload_2998_751);
                if (b)
                {
                    UIAAutomationElement.UIADriver.ProviderInvoke.Click();
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            } while (knt++ < 10);
            UIAAutomationElement.UIADriver.SearchOptions = temps;
        }

        public static void waitforcompletionmins(string[] parts)
        {
            var maxtime = int.Parse(parts[0]);
            do
            {
                checkfortrty();
                try
                {
                    UIAAutomationElement.UIADriver.CheckControlVisiblity(objsample.text_2348_957, true, 3 * 60 * 1000);
                    UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_2348_957);
                    var curtimemin = (UIAElement)UIAAutomationElement.UIADriver.ProviderNavigation.PreviousSibling.ProviderNavigation.PreviousSibling;
                    var txt = curtimemin.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_Name);
                    if (txt.Contains(parts[0]))
                        break;
                    var curtime = int.Parse(txt.Replace('"', ' ').Replace(':', ' ').Trim());
                    var tint = maxtime - curtime;
                    if (tint > 1)
                        tint = 60000;
                    else
                        tint = 10000;
                    System.Threading.Thread.Sleep(tint);
                }
                catch { }
            } while (true);
        }

        public static void waitforcompletionsecs(string[] parts)
        {
            do
            {
                checkfortrty();
                try
                {
                    UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_2348_957);
                    var curtimesec = (UIAElement)UIAAutomationElement.UIADriver.ProviderNavigation.PreviousSibling;
                    var txt = curtimesec.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_Name);
                    if (txt.Contains(parts[1]))
                        break;
                    System.Threading.Thread.Sleep(10000);
                }
                catch { }

            } while (true);

        }

        public static void closewindow()
        {
            UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_Leave_2223_289);
            UIAAutomationElement.UIADriver.ProviderInvoke.Click();
            System.Threading.Thread.Sleep(1000);

            UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_Yes_3320_353);
            UIAAutomationElement.UIADriver.ProviderGeneric.Click(false, false);
        }

        public static void record(string dir, string file)
        {
            UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_2348_957);
            var nxt = UIAAutomationElement.UIADriver.ProviderNavigation.NextSibling;
            var txt = nxt.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_Name);
            var parts = txt.Replace('"',' ').Trim().Split(':');

            System.Threading.Thread.Sleep(6000);
            presspause();
            System.Threading.Thread.Sleep(3000);

            resetbar();
            System.Threading.Thread.Sleep(3000);

            launchdebut(dir, file);
            System.Threading.Thread.Sleep(3000);

            startdebut();
            System.Threading.Thread.Sleep(3000);
            presspause();
            System.Threading.Thread.Sleep(3000);

            waitforcompletionmins(parts);
            waitforcompletionsecs(parts);

            stopdebut();
            System.Threading.Thread.Sleep(5000);

            waitforfile(dir,  file);

            exitdebut();

            closewindow();
            System.Threading.Thread.Sleep(6000);
        }

        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                var msg = "Syntax:OdaCourseMaterialCapture  <dir> \"<item>,<item>,<item>,<item>\"\n" +
                    "Example:nOdaCourseMaterialCapture  \"d:\\oda\\\" \"95-900\"";
                System.Windows.MessageBox.Show(msg);
                return;
            }


            init();
            string root = args[0];
            string filename = root + "description.txt";
            string pdfpath = root;
            var regx = new Regex(@"[\\/:*?""<>|]");
            var items = preparelist(args[1]);

            UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_Finished_2296_542);
            UIAAutomationElement.UIADriver.ProviderInvoke.Click();

            UIAElement curitm = null;
            foreach (int k in items)
            {
                UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_Finished_2296_542);
                var lstitm = (UIAElement)UIAAutomationElement.UIADriver.ProviderNavigation.Parent.ProviderNavigation.Parent.ProviderNavigation.NextSibling.ProviderNavigation.FirstChild.ProviderNavigation.LastChild;
                curitm = (UIAElement)lstitm.ProviderNavigation.FetchSibling(k-1, false);
                if (curitm == null)
                    break;

                var custm = curitm.ProviderNavigation.FirstChild;
                if (custm == null)
                    return;
                System.Threading.Thread.Sleep(1000);

                custm = custm.ProviderNavigation.NextSibling;
                if (custm == null)
                    return;
                System.Threading.Thread.Sleep(1000);

                var txttim = custm.ProviderNavigation.FirstChild;
                if (txttim == null)
                    return;
                System.Threading.Thread.Sleep(1000);

                System.IO.File.AppendAllText(filename, k.ToString("000") + "\n");
                System.Threading.Thread.Sleep(1000);

                var name = txttim.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_Name);
                System.Threading.Thread.Sleep(1000);

                var mp4ilename = k.ToString("000") + "_" + regx.Replace(name, "");


                System.IO.File.AppendAllText(filename,name + "\n");
                System.Threading.Thread.Sleep(1000);

                var moviename = k.ToString("000")+"_"+regx.Replace(name, "");

                custm = custm.ProviderNavigation.NextSibling;
                if (custm == null)
                    return;
                System.Threading.Thread.Sleep(1000);
                
                txttim = custm.ProviderNavigation.FirstChild;
                if (txttim == null)
                    return;
                System.Threading.Thread.Sleep(1000);

                name = txttim.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_Name);
                System.Threading.Thread.Sleep(1000);
                System.IO.File.AppendAllText(filename, name + "\n");
                System.Threading.Thread.Sleep(1000);

                custm = custm.ProviderNavigation.NextSibling;
                if (custm == null)
                    return;
                System.Threading.Thread.Sleep(1000);

                var dwnbtn = custm.ProviderNavigation.FirstChild;
                if (dwnbtn == null)
                    return;
                System.Threading.Thread.Sleep(1000);
                var btnname = dwnbtn.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_Name);
                if (!btnname.Contains("Replay"))
                    continue;
                dwnbtn.ProviderInvoke.Click();
                System.Threading.Thread.Sleep(1000);
                record(root.Remove(root.Length-1), mp4ilename);
            }

        }

    }

}

