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
        public string text_Course_materials_364_425
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"text_Course_materials_364_425\",        \"AEType\":\"text\",        \"AEText\":\"Course materials\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"1 0 0 0 0 0 0 0 0 \",        \"CenterPoint\":\"368,437\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_74_0\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"885.5,510\"    }}";
            }
        }
    
        //supported patterns:MSAAAccessible,Generic,Invoke,Selection,Navigation
        public string list_view_1963_520
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"list_view_1963_520\",        \"AEType\":\"list view\",        \"AEText\":\"\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke,Selection\",        \"Path\":\"0 0 0 2 0 4 0 \",        \"CenterPoint\":\"960.5,469\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_1920_52\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window\",        \"Path\":\"\",        \"CenterPoint\":\"960,540\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,SelectionItem,Navigation
        public string list_item_43_500
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"list_item_43_500\",        \"AEType\":\"list item\",        \"AEText\":\"\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke,SelectionItem\",        \"Path\":\"0 0 0 2 0 4 0 0 \",        \"CenterPoint\":\"44,570.5\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_0_0\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window\",        \"Path\":\"\",        \"CenterPoint\":\"960,539.5\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Selection,Navigation
        public string list_view_2091_540
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"list_view_2091_540\",        \"AEType\":\"list view\",        \"AEText\":\"\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke,Selection\",        \"Path\":\"1 0 0 0 0 0 0 0 \",        \"CenterPoint\":\"885,468\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2049_73\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"885.5,510\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Selection,Navigation
        public string list_view_43_21447
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"list_view_43_21447\",        \"AEType\":\"list view\",        \"AEText\":\"\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke,Selection\",        \"Path\":\"0 0 0 2 0 4 0 \",        \"CenterPoint\":\"960.5,-10191.5\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_0_0\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window\",        \"Path\":\"\",        \"CenterPoint\":\"960,539.5\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation,Scroll,ScrollItem
        public string button_Save_Ctrl_S_save_1239_257
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"button_Save_Ctrl_S_save_1239_257\",        \"AEType\":\"button\",        \"AEText\":\"Save (Ctrl+S)\",        \"AEAutomationId\":\"save\",        \"Patterns\":\"Invoke,Scroll,ScrollItem\",        \"Path\":\"2 0 0 3 2 0 0 2 1 0 5 0 0 0 0 0 0 0 0 0 11 0 \",        \"CenterPoint\":\"1042,157\"    },    \"ciroot\":    {        \"UserName\":\"window_combined_1984660ea785062_pdf_Personal_Microsoft_Edge_225_125\",        \"AEType\":\"window\",        \"AEText\":\" - Personal - Microsoft? Edge\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform,ScrollItem,ItemContainer\",        \"Path\":\"\",        \"CenterPoint\":\"574.5,724\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Value,Navigation
        public string edit_File_name_1001_403_525
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"edit_File_name_1001_403_525\",        \"AEType\":\"edit\",        \"AEText\":\"File name:\",        \"AEAutomationId\":\"1001\",        \"Patterns\":\"Value,Text\",        \"Path\":\"0 0 0 5 2 1 0 \",        \"CenterPoint\":\"553,413\"    },    \"ciroot\":    {        \"UserName\":\"window_combined_1984660ea785062_pdf_Personal_Microsoft_Edge_225_125\",        \"AEType\":\"window\",        \"AEText\":\" - Personal - Microsoft? Edge\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform,ScrollItem,ItemContainer\",        \"Path\":\"\",        \"CenterPoint\":\"574.5,724\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string button_Save_1_929_662
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"button_Save_1_929_662\",        \"AEType\":\"button\",        \"AEText\":\"Save\",        \"AEAutomationId\":\"1\",        \"Patterns\":\"Invoke\",        \"Path\":\"0 2 \",        \"CenterPoint\":\"758,553\"    },    \"ciroot\":    {        \"UserName\":\"window_combined_1984660ea785062_pdf_Personal_Microsoft_Edge_225_125\",        \"AEType\":\"window\",        \"AEText\":\" - Personal - Microsoft? Edge\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform,ScrollItem,ItemContainer\",        \"Path\":\"\",        \"CenterPoint\":\"574.5,724\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation,ScrollItem
        public string button_Close_view_7_1293_126
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"button_Close_view_7_1293_126\",        \"AEType\":\"button\",        \"AEText\":\"Close\",        \"AEAutomationId\":\"view_7\",        \"Patterns\":\"Invoke,ScrollItem\",        \"Path\":\"2 0 0 2 3 \",        \"CenterPoint\":\"1104,2\"    },    \"ciroot\":    {        \"UserName\":\"window_combined_196967c3d03510b_pdf_Personal_Microsoft_Edge_225_125\",        \"AEType\":\"window\",        \"AEText\":\" - Personal - Microsoft? Edge\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform,ScrollItem,ItemContainer\",        \"Path\":\"\",        \"CenterPoint\":\"575,724\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation,ScrollItem
        public string button_Downloads_638_258
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"button_Downloads_638_258\",        \"AEType\":\"button\",        \"AEText\":\"Downloads\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke,ScrollItem\",        \"Path\":\"0 2 0 0 1 0 0 0 1 \",        \"CenterPoint\":\"488,158\"    },    \"ciroot\":    {        \"UserName\":\"window_combined_19686abcf7a7973_pdf_Personal_Microsoft_Edge_225_125\",        \"AEType\":\"window\",        \"AEText\":\" - Personal - Microsoft? Edge\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform,ScrollItem,ItemContainer\",        \"Path\":\"\",        \"CenterPoint\":\"575,724\"    }}";
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

        public static void Main(string[] args)
        {
            if (args.Length < 2)
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

            //UIAAutomationElement.UIADriver.SetLogFile(root + "uia.log",false);

            UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_Course_materials_364_425);
            UIAAutomationElement.UIADriver.ProviderInvoke.Click();

            UIAElement lstitm = null;

            int previousk = 0;
            foreach (int k in items)
            {
                if ((k - previousk) != 1)
                {
                    UIAAutomationElement.UIADriver.SetAutomationElement(objsample.text_Course_materials_364_425);
                    lstitm = (UIAElement)UIAAutomationElement.UIADriver.ProviderNavigation.Parent.ProviderNavigation.Parent.ProviderNavigation.NextSibling.ProviderNavigation.FirstChild.ProviderNavigation.LastChild;
                    lstitm = (UIAElement)lstitm.ProviderNavigation.FetchSibling(k - 1, false);
                }
                else
                {
                    lstitm = (UIAElement)lstitm.ProviderNavigation.PreviousSibling;
                }

                if (lstitm == null)
                    break;

                previousk = k;
                var custm = lstitm.ProviderNavigation.FirstChild;
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
                System.IO.File.AppendAllText(filename,name + "\n");
                System.Threading.Thread.Sleep(1000);

                var pdffilename = k.ToString("000")+"_"+regx.Replace(name, "");

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

                txttim = txttim.ProviderNavigation.NextSibling;
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

                dwnbtn.ProviderInvoke.Click();
                System.Threading.Thread.Sleep(1000);

                int fk = 0;
                string[] lst;
                do
                {
                    lst = System.IO.Directory.GetFiles(pdfpath, "*.pdf");
                    if (lst != null && lst.Length != 0)
                        break;
                    System.Threading.Thread.Sleep(5000);
                } while (fk++ < 50);
                System.IO.File.Move(lst[0], pdfpath + "copied\\" + pdffilename);
                System.Threading.Thread.Sleep(1000);
            }

        }

    }

}

