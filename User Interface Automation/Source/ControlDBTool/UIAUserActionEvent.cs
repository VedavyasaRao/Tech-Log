using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Automation;
using System.Diagnostics;
using System.Threading;
using UITesting.Automated.MouseKeyboardActivityMonitor;
using UITesting.Automated.MouseKeyboardActivityMonitor.WinApi;
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;
using Common.Utilities;
using System.Runtime.InteropServices;

namespace UITesting.Automated.ControlDBTool
{
    public class UseractionEventData
    {
        public AutomationElement invokedlement;
        public ControlInfo ci;
        public AutomationEvent aevent;
        public bool ishandled = false;

        public static string geteventstring(AutomationEvent aevent)
        {
            if (aevent == null)
                return "null";

            if (aevent == InvokePattern.InvokedEvent)
            {
                return "InvokedEvent";
            }
            else if (aevent == SelectionItemPattern.ElementSelectedEvent)
            {
                return "ElementSelectedEvent";
            }
            else if (aevent == AutomationElement.MenuOpenedEvent)
            {
                return "MenuOpenedEvent";
            }
            else if (aevent == AutomationElement.MenuClosedEvent)
            {
                return "MenuClosedEvent";
            }
            else if (aevent == WindowPattern.WindowOpenedEvent)
            {
                return "WindowOpenedEvent";
            }
            else if (aevent == WindowPattern.WindowClosedEvent)
            {
                return "WindowClosedEvent";
            }
            else if (aevent == TextPattern.TextChangedEvent)
            {
                return "TextChangedEvent";
            }
            return "unknown";
        }

        public void captrueevent()
        {
            if (ci != null || invokedlement == null || ishandled)
                return;
            ci = UIAElementNode.GetUIATreeNode(invokedlement).Controlinfo_event;
            ishandled = true;
        }

        public bool isgood
        {
            get
            {
                return (ci != null);
            }
        }

        public UseractionEventData Clone()
        {
            UseractionEventData uecp = new UseractionEventData();
            uecp.ci = JSonSerializer.JSONPersister<ControlInfo>.SetJSON(JSonSerializer.JSONPersister<ControlInfo>.GetJSON(ci));
            uecp.aevent = AutomationEvent.LookupById(aevent.Id); 
            uecp.ishandled = ishandled;
            return uecp;
        }
    }


    public class UseractionEventManager 
    {
        const string CLASS_NAME = "UseractionEventManager";
        const uint WM_REGISTER_EVENTS = CustomWindow.WM_USER + 500;
        const uint WM_UNREGISTER_EVENTS = CustomWindow.WM_USER + 510;
        const uint WM_PROCESS_EVENT = CustomWindow.WM_USER + 520;

        private AutomationEventHandler autoeventHandler;
        //private StructureChangedEventHandler strchangedHandler = null;
        //private AutomationPropertyChangedEventHandler propchangedHandler = null;
        delegate void SetUserActionCallback(AutomationElement ae, AutomationEvent autoevent);
        public List<UseractionEventData> useractionevents = new List<UseractionEventData>();
        CustomWindow actioneventwnd;

        public IntPtr UserActionEventHandler(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WM_REGISTER_EVENTS)
            {
                RegisterForEvents();
                return IntPtr.Zero;
            }
            else if (msg == WM_UNREGISTER_EVENTS)
            {
                UnRegisterForEvents();
                return IntPtr.Zero;
            }
            else if (msg == WM_PROCESS_EVENT)
            {
                UseractionEventData uad = IntPtrConverter<UseractionEventData>.FromIntPtr(wParam);
                DoAddUserActionEvent(uad);
                return IntPtr.Zero;
            }

            return CustomWindow.DefHandler(hWnd, msg, wParam, lParam);
        }

        public UseractionEventManager()
        {
            WindowManager.CreateClass(CLASS_NAME, new NativeWndProc(UserActionEventHandler));
            WindowManager.Start(CLASS_NAME);
            actioneventwnd = WindowManager.CreateWindow(CLASS_NAME, CLASS_NAME);
        }

        public void Register()
        {
            actioneventwnd.SendNotifyMessage(WM_REGISTER_EVENTS, IntPtr.Zero, IntPtr.Zero);
            Thread.Sleep(1000);
        }

        public void Unregister()
        {
            actioneventwnd.SendNotifyMessage(WM_UNREGISTER_EVENTS, IntPtr.Zero, IntPtr.Zero);
            Thread.Sleep(1000);
        }

        public void Close()
        {
            actioneventwnd.Close();
            WindowManager.Stop(CLASS_NAME);
        }

        private void DoAddUserActionEvent(UseractionEventData uenew)
        {
            try
            {
                if (uenew.invokedlement == null || Process.GetCurrentProcess().Id == uenew.invokedlement.Current.ProcessId)
                    return;

                if (useractionevents.Count > 0)
                {
                    UseractionEventData ue = useractionevents[useractionevents.Count - 1];
                    if (ue.aevent == uenew.aevent && ue.invokedlement == uenew.invokedlement)
                        return;
                }
                uenew.captrueevent();
                useractionevents.Add(uenew);
                UIARecorder.UpdateStatus(string.Format("Received Event\tName:{0}\t\t\tEvent:{1}",uenew.ci.UserName,  UseractionEventData.geteventstring(uenew.aevent)));
            }
            catch (Exception ex)
            {
                UIARecorder.UpdateStatus(string.Format("Exeption occured in receive event {0}\t\tDetails:{1}", UseractionEventData.geteventstring(uenew.aevent), ex.Message));
            }
        }


        private void RegisterForEvents()
        {
            AutomationElement autoElement = AutomationElement.RootElement;
            autoeventHandler = new AutomationEventHandler(Onautoevent);
            Automation.AddAutomationEventHandler(
                InvokePattern.InvokedEvent,
                autoElement,
                TreeScope.Descendants,
                autoeventHandler);

            Automation.AddAutomationEventHandler(
                SelectionItemPattern.ElementSelectedEvent,
                autoElement,
                TreeScope.Descendants,
                autoeventHandler);

            Automation.AddAutomationEventHandler(
                AutomationElement.MenuOpenedEvent,
                autoElement,
                TreeScope.Descendants,
                autoeventHandler);

            //Automation.AddAutomationEventHandler(
            //    AutomationElement.MenuClosedEvent,
            //    autoElement,
            //    TreeScope.Descendants,
            //    autoeventHandler);

            //Automation.AddAutomationEventHandler(
            //    WindowPattern.WindowOpenedEvent,
            //    autoElement,
            //    TreeScope.Descendants,
            //    autoeventHandler);

            //Automation.AddAutomationEventHandler(
            //    WindowPattern.WindowClosedEvent,
            //    autoElement,
            //    TreeScope.Descendants,
            //    autoeventHandler);

            //Automation.AddAutomationEventHandler(
            //    TextPattern.TextChangedEvent,
            //    autoElement,
            //    TreeScope.Descendants,
            //    autoeventHandler);

            //strchangedHandler = new StructureChangedEventHandler(OnStructureChanged);
            //Automation.AddStructureChangedEventHandler(
            //    autoElement,
            //    TreeScope.Descendants,
            //    strchangedHandler);

            //propchangedHandler = new AutomationPropertyChangedEventHandler(OnPropertyChanged);
            //Automation.AddAutomationPropertyChangedEventHandler(
            //    autoElement,
            //    TreeScope.Descendants,
            //    propchangedHandler,
            //    new AutomationProperty[] { TogglePattern.ToggleStateProperty, AutomationElement.NameProperty });
        }

        private void UnRegisterForEvents()
        {
            AutomationElement autoElement = AutomationElement.RootElement;
            Automation.RemoveAutomationEventHandler(
                InvokePattern.InvokedEvent,
                autoElement,
                autoeventHandler);

            Automation.RemoveAutomationEventHandler(
                SelectionItemPattern.ElementSelectedEvent,
                autoElement,
                autoeventHandler);

            Automation.RemoveAutomationEventHandler(
                AutomationElement.MenuOpenedEvent,
                autoElement,
                autoeventHandler);

            //Automation.RemoveAutomationEventHandler(
            //    AutomationElement.MenuClosedEvent,
            //    autoElement,
            //    autoeventHandler);

            //Automation.RemoveAutomationEventHandler(
            //    WindowPattern.WindowOpenedEvent,
            //    autoElement,
            //    autoeventHandler);

            //Automation.RemoveAutomationEventHandler(
            //    WindowPattern.WindowClosedEvent,
            //    autoElement,
            //    autoeventHandler);

            //Automation.RemoveAutomationEventHandler(
            //    TextPattern.TextChangedEvent,
            //    autoElement,
            //    autoeventHandler);

            //Automation.RemoveStructureChangedEventHandler(
            //    autoElement,
            //    strchangedHandler);

            //Automation.RemoveAutomationPropertyChangedEventHandler(
            //    autoElement,
            //    propchangedHandler);
        }

        private void Onautoevent(object src, AutomationEventArgs e)
        {
            AutomationElement invokedelement = src as AutomationElement;
            UseractionEventData uenew = new UseractionEventData { invokedlement = invokedelement, aevent = e.EventId };
            actioneventwnd.SendNotifyMessage(WM_PROCESS_EVENT, IntPtrConverter<UseractionEventData>.ToIntPtr(uenew), IntPtr.Zero);
        }

        private void OnStructureChanged(object src, StructureChangedEventArgs e)
        {
            try
            {
                AutomationElement invokedElement = src as AutomationElement;
                if (invokedElement != null)
                {
                    if (Process.GetCurrentProcess().Id == invokedElement.Current.ProcessId)
                        return;
                    string s = string.Format("Structure ChangedEvent:{0}", invokedElement.Current.Name);
                    UIARecorder.UpdateStatus(s);
                }
            }
            catch
            {
            }

        }

        private void OnPropertyChanged(object src, AutomationPropertyChangedEventArgs e)
        {
            try
            {
                AutomationElement invokedElement = src as AutomationElement;
                if (invokedElement != null)
                {
                    if (Process.GetCurrentProcess().Id == invokedElement.Current.ProcessId)
                        return;
                    string s = string.Format("Property ChangedEvent:{0} {1} {2} {3}", invokedElement.Current.Name, e.Property.ProgrammaticName, e.OldValue, e.NewValue);
                    UIARecorder.UpdateStatus(s);
                }
            }
            catch
            {
            }
        }

    }


}
