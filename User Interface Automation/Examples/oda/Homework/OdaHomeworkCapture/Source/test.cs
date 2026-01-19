using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows.Documents;
using System.Windows.Interop;
using UITesting.Automated.WindowsInput;

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

    class firstlevel
    {
        //supported patterns:MSAAAccessible,Generic,Navigation
        public string custom_1963_520
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"custom_1963_520\",        \"AEType\":\"custom\",        \"AEText\":\"\",        \"AEAutomationId\":\"\",        \"Patterns\":\"\",        \"Path\":\"0 0 0 2 0 4 0 0 0 \",        \"CenterPoint\":\"961,871\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_1920_52\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window\",        \"Path\":\"\",        \"CenterPoint\":\"960,540\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string hyperlink
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"hyperlink_225_FRACTIONS_Fun_Time_Overdue_438_713\",        \"AEType\":\"hyperlink\",        \"AEText\":\"\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"0 0 0 2 0 4 0 0 0 3 \",        \"CenterPoint\":\"574,572\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_394_180\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"566,360\"    }}";
            }
        }


        //supported patterns:MSAAAccessible,Generic,Invoke,SelectionItem,Navigation

        public bool Process(string root, int skipknt, int idx)
        {
            string filename = root+"description.txt";
            var newlink = hyperlink.Replace("0 0 0 2 0 4 0 0 0 3", "0 0 0 2 0 4 0 0 0 " + (skipknt).ToString());
            var temps = UIAAutomationElement.UIADriver.SearchOptions;
            UIAAutomationElement.UIADriver.SearchOptions = "PACS";
            UIAAutomationElement.UIADriver.SetAutomationElement(newlink);
            UIAAutomationElement.UIADriver.SearchOptions = temps;
            var hyplnk = UIAAutomationElement.UIADriver;

            var cusitm = hyplnk.ProviderNavigation.FirstChild;
            if (cusitm == null)
                return false;
            System.Threading.Thread.Sleep(1000);

            cusitm = cusitm.ProviderNavigation.NextSibling;
            if (cusitm == null)
                return false;
            System.Threading.Thread.Sleep(1000);

            var txtitm = cusitm.ProviderNavigation.FirstChild;
            if (txtitm == null)
                return false;
            System.Threading.Thread.Sleep(1000);

            System.IO.File.AppendAllText(filename, (idx).ToString("000") + "\n");
            System.Threading.Thread.Sleep(1000);
            System.IO.File.AppendAllText(filename, txtitm.ProviderGeneric.GetAutomationProperty(txtitm.Constants.AutomationProperty_Name) + "\n");
            System.Threading.Thread.Sleep(1000);

            hyplnk.ProviderInvoke.Click();
            System.Threading.Thread.Sleep(3000);

            return true;

        }
    }
    class secondlevel
    {
        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string custom_2638_543
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"custom_2638_543\",        \"AEType\":\"custom\",        \"AEText\":\"\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"0 0 0 1 0 0 0 0 0 0 4 \",        \"CenterPoint\":\"675,463\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2000_102\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"885.5,510\"    }}";
            }

        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string text_1_2652_552
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"text_1_2652_552\",        \"AEType\":\"text\",        \"AEText\":\"1\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"0 0 0 1 0 0 0 0 0 0 4 0 \",        \"CenterPoint\":\"657.5,462\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2000_102\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"885.5,510\"    }}";
            }
        }

        public void Process()
        {
            UIAAutomationElement.UIADriver.SetAutomationElement(text_1_2652_552);
            UIAAutomationElement.UIADriver.ProviderInvoke.Click();
            System.Threading.Thread.Sleep(3000);

        }
    }

    class thirdlevel
    {
        //supported patterns:MSAAAccessible,Generic,Selection,Navigation
        public string list_view_3262_315
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"list_view_3262_315\",        \"AEType\":\"list view\",        \"AEText\":\"\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Selection\",        \"Path\":\"0 0 0 1 0 0 0 0 0 1 2 \",        \"CenterPoint\":\"1411.5,355\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2007_92\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"885.5,510\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string button_Leave_127_57
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"button_Leave_127_57\",        \"AEType\":\"button\",        \"AEText\":\"Leave\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"1 0 0 0 0 0 \",        \"CenterPoint\":\"76,25\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_98_33\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"885.5,510\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string custom_2006_168
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"custom_2006_168\",        \"AEType\":\"custom\",        \"AEText\":\"\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"0 0 0 1 \",        \"CenterPoint\":\"885,547.5\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2007_92\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"885.5,510\"    }}";
            }
        }

        
        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string custom_1181_199
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"custom_1181_199\",        \"AEType\":\"custom\",        \"AEText\":\"\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke\",        \"Path\":\"0 0 0 1 0 0 0 0 0 0 0 0 0 \",        \"CenterPoint\":\"1744.5,503\"    },    \"ciroot\":    {        \"UserName\":\"pane_Oda_Class_2007_92\",        \"AEType\":\"pane\",        \"AEText\":\"Oda Class\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform\",        \"Path\":\"\",        \"CenterPoint\":\"885.5,510\"    }}";
            }
        }

        bool shouldscroll(IUIAElement partchild, jsonparser jspobj, int ybot, ref int parttop)
        {
            var isoffscreen = partchild.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_IsOffscreen);
            var bscroll = false;
            parttop = 0;
            if (isoffscreen == "false")
            {
                var tempbrs = partchild.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_BoundingRectangle);
                if (tempbrs.Contains("\"Bottom\":-Infinity"))
                    bscroll = true;
                else if ((int)jspobj.ParseObj(tempbrs, "Bottom") > ybot)
                {
                    bscroll = true;
                    parttop = (int)jspobj.ParseObj(tempbrs, "Top");
                }
            }
            return (isoffscreen == "true" || bscroll);
        }
        public void capturepics(int ord,string dir)
        {
            jsonparser jspobj = new jsonparser();

            UIAAutomationElement.UIADriver.SetAutomationElement(custom_1181_199);
            System.Threading.Thread.Sleep(1000);

            var root = UIAAutomationElement.UIADriver.ProviderNavigation.FirstChild;
            while (--ord > 0)
                root = root.ProviderNavigation.NextSibling;

            root = root.ProviderNavigation.FirstChild.ProviderNavigation.FirstChild;

            var parentbrs = root.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_BoundingRectangle);
            int x = (int)jspobj.ParseObj(parentbrs, "Right");
            int ytop = (int)jspobj.ParseObj(parentbrs, "Top");
            int ybot = (int)jspobj.ParseObj(parentbrs, "Bottom");

            var part = root.ProviderNavigation.FirstChild;
            var partlast = root.ProviderNavigation.LastChild;
            var lastbrs = partlast.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_BoundingRectangle);
            UITesting.Automated.WindowsInput.MouseSimulator ms = new MouseSimulator();
            int parttop = 0;
            int i = 1;
            int MouseScrollCount = 78;
            while (true)
            {
                Player.evt.WaitOne();
                bool bonce = false;
                bool bsaved = false;
                int shouldscrollcount = 0;
                while (shouldscroll(part,jspobj,ybot, ref parttop))
                {
                    if (!bonce)
                    {

                        UIAAutomationElement.UIADriver.ProviderGeneric.MoveandClick(x - 5, ytop + 5, false, false, false);
                        System.Threading.Thread.Sleep(1000);
                        bonce = true;
                    }
                    if (!bsaved && parttop > 0 && ((parttop - MouseScrollCount) <= ytop))
                    {
                        var filename2 = dir + (i++).ToString("00") + "_" + (parttop - MouseScrollCount).ToString("000") + ".png";
                        part.ProviderGeneric.CaptureBitmap(filename2);
                        System.Threading.Thread.Sleep(2000);
                        bsaved = true;
                    }
                    ms.VerticalScroll(-1);
                    if (shouldscrollcount++ == 30)
                        break;
                }
                if (bonce) 
                    lastbrs = partlast.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_BoundingRectangle);
                var filename = dir + (i).ToString("00") + ".png";
                part.ProviderGeneric.CaptureBitmap(filename);
                System.Threading.Thread.Sleep(2000);
                var brs = part.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_BoundingRectangle);
                if (brs == lastbrs)
                    break;
                part = part.ProviderNavigation.NextSibling;
                ++i;
            }

        }

        public void Process(string imgpath)
        {

            UIAAutomationElement.UIADriver.SetAutomationElement(list_view_3262_315);
            System.Threading.Thread.Sleep(1000);


            var li = UIAAutomationElement.UIADriver.ProviderNavigation.FirstChild;
            System.Threading.Thread.Sleep(1000);
            int i = 1;
            char c = 'A';
            while (li != null)
            {
                Player.evt.WaitOne(); 
                string tempdir = imgpath + string.Format("{0}\\", c);
                Directory.CreateDirectory(tempdir);
                if (li.ProviderInvoke != null)
                {
                    li.ProviderInvoke.Click();
                    System.Threading.Thread.Sleep(1000);
                }
                else
                    break;
                
                capturepics(i++, tempdir);

                li = li.ProviderNavigation.NextSibling;
                System.Threading.Thread.Sleep(3000);
                c++;
            }

            UIAAutomationElement.UIADriver.SetAutomationElement(button_Leave_127_57);
            UIAAutomationElement.UIADriver.ProviderInvoke.Click();
            System.Threading.Thread.Sleep(3000);
        }

    }


    //  ***************Playback Code********************
    class Player
    {

        internal static EventWaitHandle evt;
        static jsonparser jspobj = new jsonparser();
        static firstlevel first = new firstlevel();
        static secondlevel second = new secondlevel();
        static thirdlevel third = new thirdlevel();

        static object ParseObj( object injson,  object field)
        {
            return jspobj.ParseObj(injson, field);
        }

        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                var msg= "Syntax:OdaHomeworkCapture  <dir> <count> \"<item>,<item>,<item>,<item>\"\n" +
                    "Example:OdaHomeworkCapture  \"d:\\oda\\\" 300 \"95-99\"";
                System.Windows.MessageBox.Show(msg);
                return;
            }

            int i = 0;

            string root = args[0];
            int n = int.Parse(args[1]);
            var argitems = args[2].Split(',');

            var items = new List<int>();
            foreach (var item in argitems)
            {
                if (item.Contains("-"))
                {
                    var parts = item.Split('-');
                    for (i = int.Parse(parts[0]); i <= int.Parse(parts[1]); ++i)
                        items.Add(i);
                }
                else
                    items.Add(int.Parse(item));
            }



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


            //UIAAutomationElement.UIADriver.SetLogFile(root + "uia.log",false);


            foreach (int k in items)
            {
                first.Process(root, (n - k), k);
                evt.WaitOne();
                second.Process();
                evt.WaitOne();
                string dir = root + k.ToString("000") + "\\";
                Directory.CreateDirectory(dir);
                third.Process(dir);
                if (k == 35)
                    continue;
            }
        }
    }

}

