using System;
using System.Text.RegularExpressions;
using System.Windows;
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
                return "{    \"ci\":    {        \"UserName\":\"button_Save_Ctrl_S_save_1239_257\",        \"AEType\":\"button\",        \"AEText\":\"Save (Ctrl+S)\",        \"AEAutomationId\":\"save\",        \"Patterns\":\"Invoke,Scroll,ScrollItem\",        \"Path\":\"2 0 0 3 2 0 0 2 1 0 5 0 0 0 0 0 0 0 0 0 11 0 \",        \"CenterPoint\":\"1042,157\"    },    \"ciroot\":    {        \"UserName\":\"window_combined_1984660ea785062_pdf_Personal_Microsoft_Edge_225_125\",        \"AEType\":\"window\",        \"AEText\":\" - Personal - Microsoft​ Edge\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform,ScrollItem,ItemContainer\",        \"Path\":\"\",        \"CenterPoint\":\"574.5,724\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Value,Navigation
        public string edit_File_name_1001_403_525
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"edit_File_name_1001_403_525\",        \"AEType\":\"edit\",        \"AEText\":\"File name:\",        \"AEAutomationId\":\"1001\",        \"Patterns\":\"Value,Text\",        \"Path\":\"0 0 0 5 2 1 0 \",        \"CenterPoint\":\"553,413\"    },    \"ciroot\":    {        \"UserName\":\"window_combined_1984660ea785062_pdf_Personal_Microsoft_Edge_225_125\",        \"AEType\":\"window\",        \"AEText\":\" - Personal - Microsoft​ Edge\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform,ScrollItem,ItemContainer\",        \"Path\":\"\",        \"CenterPoint\":\"574.5,724\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation
        public string button_Save_1_929_662
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"button_Save_1_929_662\",        \"AEType\":\"button\",        \"AEText\":\"Save\",        \"AEAutomationId\":\"1\",        \"Patterns\":\"Invoke\",        \"Path\":\"0 2 \",        \"CenterPoint\":\"758,553\"    },    \"ciroot\":    {        \"UserName\":\"window_combined_1984660ea785062_pdf_Personal_Microsoft_Edge_225_125\",        \"AEType\":\"window\",        \"AEText\":\" - Personal - Microsoft​ Edge\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform,ScrollItem,ItemContainer\",        \"Path\":\"\",        \"CenterPoint\":\"574.5,724\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation,ScrollItem
        public string button_Close_view_7_1293_126
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"button_Close_view_7_1293_126\",        \"AEType\":\"button\",        \"AEText\":\"Close\",        \"AEAutomationId\":\"view_7\",        \"Patterns\":\"Invoke,ScrollItem\",        \"Path\":\"2 0 0 2 3 \",        \"CenterPoint\":\"1104,2\"    },    \"ciroot\":    {        \"UserName\":\"window_combined_196967c3d03510b_pdf_Personal_Microsoft_Edge_225_125\",        \"AEType\":\"window\",        \"AEText\":\" - Personal - Microsoft​ Edge\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform,ScrollItem,ItemContainer\",        \"Path\":\"\",        \"CenterPoint\":\"575,724\"    }}";
            }
        }

        //supported patterns:MSAAAccessible,Generic,Invoke,Navigation,ScrollItem
        public string button_Downloads_638_258
        {
            get
            {
                return "{    \"ci\":    {        \"UserName\":\"button_Downloads_638_258\",        \"AEType\":\"button\",        \"AEText\":\"Downloads\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Invoke,ScrollItem\",        \"Path\":\"0 2 0 0 1 0 0 0 1 \",        \"CenterPoint\":\"488,158\"    },    \"ciroot\":    {        \"UserName\":\"window_combined_19686abcf7a7973_pdf_Personal_Microsoft_Edge_225_125\",        \"AEType\":\"window\",        \"AEText\":\" - Personal - Microsoft​ Edge\",        \"AEAutomationId\":\"\",        \"Patterns\":\"Window,Transform,ScrollItem,ItemContainer\",        \"Path\":\"\",        \"CenterPoint\":\"575,724\"    }}";
            }
        }
    }

    //  ***************Playback Code********************
    class Player
    {

        static jsonparser jspobj = new jsonparser();
        static sample objsample = new sample();

        static object ParseObj( object injson,  object field)
        {
            return jspobj.ParseObj(injson, field);
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show("Syntax:OdaCourseMaterialCapture <dir> start\nExample\nOdaCourseMaterialCapture \"d:\\oda\\\" 10");
                return;
            }
            string root = args[0];
            int i = 1;
            int k = int.Parse(args[1]);

            string filename = root + "description.txt";
            string pdfpath = root;
            var regx = new Regex(@"[\\/:*?""<>|]");

            //UIAAutomationElement.UIADriver.SetAutomationElement(objsample.list_view_2091_540);
            UIAAutomationElement.UIADriver.SetAutomationElement(objsample.list_view_1963_520);
            var lstitm = UIAAutomationElement.UIADriver.ProviderNavigation.LastChild;
            if (lstitm == null)
                return;
            System.Threading.Thread.Sleep(1000);

            for (; i < k; ++i)
            {
                lstitm = lstitm.ProviderNavigation.PreviousSibling;
                if (lstitm == null)
                    return;
                System.Threading.Thread.Sleep(1000);

            }

            while (true)
            {
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

                System.IO.File.AppendAllText(filename, i.ToString("000") + "\n");
                System.Threading.Thread.Sleep(1000);
                var name = txttim.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_Name);
                System.Threading.Thread.Sleep(1000);
                System.IO.File.AppendAllText(filename,name + "\n");
                System.Threading.Thread.Sleep(1000);
                var pdffilename = i.ToString("000")+"_"+regx.Replace(name, "");

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

                var bvisible = UIAAutomationElement.UIADriver.CheckControlVisiblity(objsample.button_Save_Ctrl_S_save_1239_257,true,10*60*1000);
                if (!bvisible)
                    return;
                var savbtn = UIAAutomationElement.UIADriver.SetAutomationElement(objsample.button_Save_Ctrl_S_save_1239_257);
                if (savbtn == null)
                    return;
                System.Threading.Thread.Sleep(1000);

                for (int j = 0; j < 5; ++j)
                {
                    if (UIAAutomationElement.UIADriver.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_IsEnabled) == "true")
                        break;
                    System.Threading.Thread.Sleep(3000);
                }

                UIAAutomationElement.UIADriver.ProviderInvoke.Click();
                System.Threading.Thread.Sleep(1000);

                UIAAutomationElement.UIADriver.SetAutomationElement(objsample.edit_File_name_1001_403_525);
                System.Threading.Thread.Sleep(1000);
                UIAAutomationElement.UIADriver.ProviderValue.Value = pdfpath + pdffilename;
                UIAAutomationElement.UIADriver.ProviderGeneric.SendKeyStrokes("{ENTER}");
                System.Threading.Thread.Sleep(1000);

                UIAAutomationElement.UIADriver.CheckControlVisiblity(objsample.button_Downloads_638_258, true, 90000);


                UIAAutomationElement.UIADriver.SetAutomationElement(objsample.button_Close_view_7_1293_126);
                System.Threading.Thread.Sleep(1000);
                UIAAutomationElement.UIADriver.ProviderInvoke.Click();
                System.Threading.Thread.Sleep(1000);

                lstitm = lstitm.ProviderNavigation.PreviousSibling;
                if (lstitm== null)
                    return;
                System.Threading.Thread.Sleep(1000);
                ++i;
            }

        }

    }

}

