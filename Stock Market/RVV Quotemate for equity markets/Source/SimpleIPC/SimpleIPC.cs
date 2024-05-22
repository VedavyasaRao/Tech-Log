using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Reflection.Emit;

namespace SimpleIPC
{
    enum SIPCArgsIndex :int { Name, Exception, Returnval, Args };

    public interface IEventCaller
    {
        void calleventhandlers(string eventname, string calleename, params object[] args);
    }

    #region Win32
    public static class Win32
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MSG
        {
            public IntPtr hwnd;
            public UInt32 message;
            public IntPtr wParam;
            public IntPtr lParam;
            public UInt32 time;
            public POINT pt;
        }

        internal const uint WM_USER = 0X400;
        internal const uint WM_CLOSE = 0x0010;
        internal const int GWL_WNDPROC = -4;
        

        [DllImport("user32.dll")]
        internal static extern sbyte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        internal static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        internal static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        [DllImport("user32.dll")]
        internal static extern void PostQuitMessage(int nExitCode);


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool PostThreadMessage(uint threadId, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        internal static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        internal delegate IntPtr NativeWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr DefWindowProcW(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential,
           CharSet = System.Runtime.InteropServices.CharSet.Unicode
        )]
        internal struct WNDCLASS
        {
            public uint style;
            public NativeWndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public string lpszMenuName;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public string lpszClassName;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        internal static extern System.UInt16 RegisterClassW(
            [System.Runtime.InteropServices.In] ref WNDCLASS lpWndClass
        );

        [DllImport("user32.dll")]
        internal static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern void DisableProcessWindowsGhosting();

        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendNotifyMessage")]
        internal static extern bool SendIntNotifyMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        internal static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr CreateWindowExW(
           UInt32 dwExStyle,
           [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] 
       string lpClassName,
           [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] 
       string lpWindowName,
           UInt32 dwStyle,
           Int32 x,
           Int32 y,
           Int32 nWidth,
           Int32 nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam
        );


        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr OpenFileMapping(
           int dwDesiredAccess, bool bInheritHandle, String lpName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        internal static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,
            int dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = false)]
        internal static extern bool
            ConvertStringSecurityDescriptorToSecurityDescriptor(
            [In] string StringSecurityDescriptor,
            [In] uint StringSDRevision,
            [Out] out IntPtr SecurityDescriptor,
            [Out] out int SecurityDescriptorSize
        );

        [DllImport("kernel32", SetLastError = true)]
        internal static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
        internal static extern void ZeroMemory(IntPtr dest, uint size);

        [DllImport("kernel32", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        internal static extern IntPtr CreateFileMapping(
            IntPtr hFile,
            ref SecurityAttributes lpAttributes,
            FileMapProtection flProtect,
            Int32 dwMaxSizeHi,
            Int32 dwMaxSizeLow,
            string lpName);

        [StructLayout(LayoutKind.Sequential)]
        internal struct SecurityAttributes
        {
            public Int32 nLength;
            public IntPtr lpSecurityDescriptor;
            public Int32 bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SecurityDescriptor
        {
            public Byte revision;
            public Byte size;
            public Int16 control;
            public IntPtr owner;
            public IntPtr group;
            public IntPtr sacl;
            public IntPtr dacl;
        }

        [Flags]
        internal enum FileMapProtection
        {
            /// <summary>
            /// 
            /// </summary>
            PageReadOnly = 0x0002,
            /// <summary>
            /// 
            /// </summary>
            PageReadWrite = 0x0004,
            /// <summary>
            /// 
            /// </summary>
            PageWriteCopy = 0x0008
        }

        [DllImport("kernel32.dll")]
        internal static extern bool VirtualLock(IntPtr lpAddress, uint dwSize);

        internal const int SecurityDescriptorRevision = 1;

        [DllImport("kernel32.dll")]
        public static extern bool SetEvent(IntPtr hEvent);

    }
    #endregion Win32

    #region MemoryManager
    public sealed class MemoryManager 
    {
        const uint BUFFER_SIZE = 4094*4;
        private Mutex mtx = null;
        private EventWaitHandle evt = null;

        private string mmfname;
        private IntPtr dataptr = IntPtr.Zero;
        private IntPtr hmmf = IntPtr.Zero;

        static Win32.SecurityAttributes sa = new Win32.SecurityAttributes();
        static MutexSecurity ms;
        static EventWaitHandleSecurity ws;
        string _name;

        static MemoryManager()
        {
            int sz;
            sa.nLength = Marshal.SizeOf(typeof(Win32.SecurityAttributes));
            sa.lpSecurityDescriptor = IntPtr.Zero; ;
            sa.bInheritHandle = 0;
            Win32.ConvertStringSecurityDescriptorToSecurityDescriptor("D:(A;OICI;GA;;;WD)", 1, out sa.lpSecurityDescriptor, out sz);

            ms = new MutexSecurity();
            ms.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow));

            ws = new EventWaitHandleSecurity();
            ws.AddAccessRule(new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), EventWaitHandleRights.FullControl, AccessControlType.Allow));
        }

        public MemoryManager(string name)
        {
            _name = name;
        }

        public void CreateNew()
        {
            bool b;
            mtx = new Mutex(false, "Global\\mtx" + _name, out b, ms);
            evt = new EventWaitHandle(false, EventResetMode.AutoReset, "Global\\evt" + _name, out b, ws);

            mmfname = "Global\\mmf" + _name;
            hmmf = Win32.CreateFileMapping(new IntPtr(-1), ref sa, Win32.FileMapProtection.PageReadWrite, 0, (int)BUFFER_SIZE, mmfname);
            dataptr = Win32.MapViewOfFile(hmmf, 0x1f, 0, 0, 0);
            b = Win32.VirtualLock(dataptr, BUFFER_SIZE);
            Win32.ZeroMemory(dataptr, BUFFER_SIZE);
        }

        public void OpenExisitng()
        {
            bool b;
            mtx = Mutex.OpenExisting("Global\\mtx" + _name);
            evt = EventWaitHandle.OpenExisting("Global\\evt" + _name);
            mmfname = "Global\\mmf" + _name;
            hmmf = Win32.CreateFileMapping(new IntPtr(-1), ref sa, Win32.FileMapProtection.PageReadWrite, 0, (int)BUFFER_SIZE, mmfname);
            dataptr = Win32.MapViewOfFile(hmmf, 0x1f, 0, 0, 0);
            b = Win32.VirtualLock(dataptr, BUFFER_SIZE);
        }

        public void StoreParameters(string name, object[] paramlist)
        {
            int plen = (paramlist == null) ? 0 : paramlist.Length;
            object[] data = new object[plen + (int)SIPCArgsIndex.Args];
            data[(int)SIPCArgsIndex.Name] = name;
            data[(int)SIPCArgsIndex.Exception] = null;
            data[(int)SIPCArgsIndex.Returnval] = null;
            if (plen != 0)
                paramlist.CopyTo(data, (int)SIPCArgsIndex.Args);
            BinaryFormatter formatter = new BinaryFormatter();
            unsafe
            {
                UnmanagedMemoryStream memStream = new UnmanagedMemoryStream((byte*)dataptr.ToPointer(), BUFFER_SIZE, BUFFER_SIZE, FileAccess.ReadWrite);
                formatter.Serialize(memStream, data);
                memStream.Flush();
                memStream.Dispose();
            }
        }

        public object[] RetriveParameters()
        {
            object[] data = null;
            unsafe
            {
                UnmanagedMemoryStream memStream = new UnmanagedMemoryStream((byte*)dataptr.ToPointer(), BUFFER_SIZE, BUFFER_SIZE, FileAccess.ReadWrite);
                BinaryFormatter formatter = new BinaryFormatter();
                data = (object[])formatter.Deserialize(memStream);
                memStream.Dispose();
            }
            return data;
        }

        public void StoreResults(object[] returnlist)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            unsafe
            {
                UnmanagedMemoryStream memStream = new UnmanagedMemoryStream((byte*)dataptr.ToPointer(), BUFFER_SIZE, BUFFER_SIZE, FileAccess.ReadWrite);
                try
                {
                    formatter.Serialize(memStream, returnlist);
                }
                catch 
                {

                }

                memStream.Flush();
                memStream.Dispose();
            }
        }

        public void BeginCall()
        {
            try
            {
                evt.Reset();
                mtx.WaitOne();
            }
            catch
            { }
        }

        public void WaitProcessdone()
        {
            evt.WaitOne();
        }

        public void SetProcessdone()
        {
            evt.Set();
        }

        public void EndCall()
        {
            mtx.ReleaseMutex();
        }

        public void Close()
        {
            mtx.Close();
            evt.Close();
            Win32.UnmapViewOfFile(dataptr);
            Win32.CloseHandle(hmmf);
        }
    }
    #endregion MemoryManager 

    #region SIPCServer
    public class SIPCServerImpl 
    {
        private object _server;
        string _uniquename;
        private MemoryManager mm;
        static List<string> eventinvlist = new List<string>();
        internal SIPCServerImpl(string uniquename, object serverinstance)
        {
            _uniquename = uniquename;
            mm = new MemoryManager(_uniquename);
            mm.CreateNew();
            _server = serverinstance;
        }

        internal void addeventhandler(string eventhandlerdetails)
        {
            string servername = eventhandlerdetails.Split('/')[0];
            string evtname = eventhandlerdetails.Split('/')[1];
            foreach (var e in eventinvlist)
            {
                string server_event = servername  + "/" + evtname + "/";
                if (e.Contains(server_event))
                {
                    eventinvlist.Add(eventhandlerdetails);
                    return;
                }
            }

            eventinvlist.Add(eventhandlerdetails);
            var evinfo = _server.GetType().GetEvent(evtname);
            Type handlerType = evinfo.EventHandlerType;
            MethodInfo invokeMethod = handlerType.GetMethod("Invoke");
            ParameterInfo[] parms = invokeMethod.GetParameters();
            Type[] parmTypes = new Type[parms.Length];
            for (int i = 0; i < parms.Length; i++)
            {
                parmTypes[i] = parms[i].ParameterType;
            }

            var dm = typeof(SIPCServerImpl).GetMethod("dummy", BindingFlags.NonPublic | BindingFlags.Static);
            DynamicMethod eventHandler = new DynamicMethod("", invokeMethod.ReturnType, parmTypes,typeof(SIPCServerImpl));
            ILGenerator il = eventHandler.GetILGenerator();
            il.Emit(OpCodes.Ldstr, servername);
            il.Emit(OpCodes.Ldstr, evtname);
            for (byte i = 0; i < parmTypes.Length; ++i)
            {
                il.Emit(OpCodes.Ldarg_S, i);
            }

            il.EmitCall(OpCodes.Call, dm, parmTypes);
            il.Emit(OpCodes.Ret);
            evinfo.AddEventHandler(_server, eventHandler.CreateDelegate(handlerType));
        }

        internal static void dummy(string servername, string eventname, __arglist)
        {
            ArgIterator lIterator = new ArgIterator(__arglist);
            object[] args = new object[lIterator.GetRemainingCount()];
            int i = 0;
            while (lIterator.GetRemainingCount() > 0)
            {
                TypedReference r = lIterator.GetNextArg();
                args[i++] = TypedReference.ToObject(r);
            }

            foreach (var e in eventinvlist)
            {
                string server_event = servername + "/" + eventname + "/";
                if (e.Contains(server_event))
                {
                    string server = e.Split('/')[2];
                    var baseproxy = new SimpleIPC.NamedObject.SIPCProxy(server);
                    if (baseproxy.IsServerAlive())
                    {
                        var proxy = new GenericProxy<IEventCaller>(baseproxy);
                        try
                        {
                            proxy.Proxy.calleventhandlers(eventname, e.Split('/')[3], args);
                        }
                        catch
                        {
                        }
                        proxy.Dispose();
                    }
                }
            }
        }

        internal void process()
        {
            object[] data = mm.RetriveParameters();
            object[] pars = null;
            int parlen = data.Length - (int)SIPCArgsIndex.Args;
            if (parlen > 0)
            {
                pars = new object[parlen];
                Array.Copy(data, (int)SIPCArgsIndex.Args, pars, 0, parlen);
            }
            string mtdname = (string)data[(int)SIPCArgsIndex.Name];

            if (mtdname == "add_event")
            {
                addeventhandler(_uniquename + "/" + pars[0].ToString());
            }
            else if (mtdname == "remove_event")
            {
                eventinvlist.Remove(_uniquename + "/" + pars[0].ToString());
            }
            else
            {
                try
                {
                    data[(int)SIPCArgsIndex.Returnval] = _server.GetType().InvokeMember(mtdname,
                                BindingFlags.DeclaredOnly |
                                BindingFlags.Public | BindingFlags.NonPublic |
                                BindingFlags.Instance | BindingFlags.InvokeMethod, null, _server, pars);
                }
                catch (Exception ex)
                {
                    data[(int)SIPCArgsIndex.Exception] = ex;
                }
            }

            if (parlen > 0)
            {
                Array.Copy(pars, 0, data, (int)SIPCArgsIndex.Args, parlen);
            }

            mm.StoreResults(data);
            mm.SetProcessdone();
        }

        public void Close()
        {
            mm.Close();
        }
    }
    #endregion SIPCServer

    #region Proxy
    public abstract class ProxyBase 
    {
        public string UniqueName { get; set; }
        public ProxyBase(string UniqueName)
        {
            this.UniqueName = UniqueName;
        }
        abstract public void PostProcessMessage();
        abstract public void Close();
        abstract public bool IsServerAlive();
    }

    public struct APIData
    {
        public object retval;
        public object[] retargs;
        public IMethodCallMessage calldata;
    };

    public enum Callmode { sync, async };

    public class EventsManager : IEventCaller
    {
        public List<Tuple<string, Delegate>> eventlist = new List<Tuple<string, Delegate>>();

        public void AddEvent(string eventname, Delegate d)
        {
            eventlist.Add(new Tuple<string, Delegate>(eventname, d));

        }

        public void RemoveEvent(string eventname, string callermtdname)
        {
            foreach (var e in eventlist)
            {
                if (e.Item1 == eventname && e.Item2.Method.Name == callermtdname)
                {
                    eventlist.Remove(e);
                    break;
                }
            }
        }

        public void calleventhandlers(string eventname, string callermtdname, params object[] args)
        {
            foreach (var e in eventlist)
            {
                if (e.Item1 == eventname && e.Item2.Method.Name == callermtdname)
                {
                    e.Item2.DynamicInvoke(args);
                }
            }

        }

    }

    public class GenericProxy<T> : RealProxy, IDisposable
    {
        ProxyBase provider;
        MemoryManager mm;
        Callmode callmode = Callmode.sync;
        WaitCallback callback = null;
        delegate object eventcaller(string name, object[] pars);
        bool basynccall=false;
        bool basynccallback = false;
        bool bevents = false;
        EventsManager eventmgr = new EventsManager();
        Windows.ServerContainer asynccontainer = null;
        Windows.SingleActionServer asynccallserver = null;
        Windows.SingleActionServer callbackserver = null;
        NamedObject.SIPCServer eventserver = null;

        public GenericProxy(ProxyBase provider, bool basynccall = false, bool basynccallback = false, bool bevents = false)
            : base(typeof(T))
        {
            this.basynccall = basynccall;
            this.basynccallback = basynccallback;
            this.bevents = bevents;
            this.provider = provider;
            mm = new MemoryManager(provider.UniqueName);
            mm.OpenExisitng();
            if (basynccall)
            {
                asynccontainer = new Windows.ServerContainer();
                asynccontainer.Start();
                string servername = DateTime.Now.Ticks.ToString();
                asynccallserver = new Windows.SingleActionServer(servername, new WaitCallback(this.InvokeAsync));
                asynccontainer.CreateServer(asynccallserver);
                if (basynccallback)
                {
                    Thread.Sleep(200);
                    string servername2 = DateTime.Now.Ticks.ToString();
                    callbackserver = new Windows.SingleActionServer(servername2, null);
                    asynccontainer.CreateServer(callbackserver);
                }
            }

            if (bevents)
            {
                Thread.Sleep(200);
                eventmgr = new EventsManager();
                string servername3 = DateTime.Now.Ticks.ToString();
                eventserver = new NamedObject.SIPCServer(servername3, eventmgr);
                eventserver.Start();
            }
            
        }

        public void CallingMode(Callmode cm, WaitCallback cb)
        {
            callmode = cm;
            callback = cb;
        }

        void InvokeAsync(object apidata)
        {
            APIData apicalldata = (APIData)apidata;
            Exception ex;
            bool bret = InvokeMember(apicalldata.calldata.MethodName, apicalldata.retargs, out apicalldata.retval, out ex);
            if (basynccallback || callback != null)
            {
                callbackserver.DoAction = callback;
                callbackserver.AddItem((bret)?(new object[] { apicalldata }):new object[] { ex });
            }
        }

        public T Proxy
        {
            get
            {
                return (T)GetTransparentProxy();
            }
        }

        public override IMessage Invoke(IMessage myIMessage)
        {
            IMethodCallMessage calldata = myIMessage as IMethodCallMessage;
            object oret = null;
            string mtdname = myIMessage.Properties["__MethodName"].ToString();
            if ((mtdname.StartsWith("add_") || mtdname.StartsWith("remove_")) && calldata.InArgs.Length == 1 && calldata.InArgs[0] is Delegate)
            {
                string callname="";
                string arg="";
                string eventname = "";
                string callermtdname = ((Delegate)calldata.InArgs[0]).Method.Name;
                if (mtdname.StartsWith("add_"))
                {
                    eventname = mtdname.Substring(4);
                    eventmgr.AddEvent(eventname, (Delegate)calldata.InArgs[0]);
                    callname = "add_event";
                    arg = string.Format("{0}/{1}/{2}", eventname, eventserver.Name, callermtdname);
                }
                else if (mtdname.StartsWith("remove_"))
                {
                    callname = "remove_event";
                    eventname = mtdname.Substring(7);
                    eventmgr.RemoveEvent(eventname, callermtdname);
                    arg = string.Format("{0}/{1}/{2}", eventname, eventserver.Name, callermtdname);
                }
                Exception ex;
                if (InvokeMember(callname, new object[] { arg }, out oret, out ex))
                    return new ReturnMessage(oret, null, 0, null, null);
                return new ReturnMessage(ex, calldata);

            }

            object[] newargs = new object[calldata.Args.Length];
            Array.Copy(calldata.Args, newargs, calldata.Args.Length);
            if (callmode == Callmode.sync || !basynccall)
            {
                Exception ex;
                if (!InvokeMember(calldata.MethodName, newargs, out oret, out ex))
                    return new ReturnMessage(ex, calldata);
            }
            else
            {
                APIData apidata = new APIData();
                apidata.calldata = calldata;
                apidata.retargs = newargs;
                asynccallserver.AddItem(new object[] { apidata });
            }
            return new ReturnMessage(oret, newargs, newargs.Length, null, null);
        }

        private bool InvokeMember(string mtdname, object[] args, out object result, out Exception ex)
        {
            result = null;
            mm.BeginCall();
            mm.StoreParameters(mtdname, args);
            provider.PostProcessMessage();
            mm.WaitProcessdone();
            object[] data = mm.RetriveParameters();
            if (data[(int)SIPCArgsIndex.Exception] == null)
            {
                result = data[(int)SIPCArgsIndex.Returnval];
                if (args.Length > 0)
                {
                    Array.Copy(data, (int)SIPCArgsIndex.Args, args, 0, args.Length);
                }
            }
            mm.EndCall();
            ex = (Exception)data[(int)SIPCArgsIndex.Exception];
            return (data[(int)SIPCArgsIndex.Exception] == null);
        }

        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (asynccontainer != null)
            {
                asynccontainer.Stop();
                if (asynccallserver != null)
                    asynccallserver.Close();
                if (callbackserver != null)
                    callbackserver.Close();
            }

            if (eventserver != null)
                eventserver.Stop();
            mm.Close();
            provider.Close();
            disposed = true;
        }

    }
    #endregion Proxy

    #region Windows
    namespace Windows
    {
        public sealed class ServerContainer 
        {
            internal const uint WM_START_CONTAINER = Win32.WM_USER;
            internal const uint WM_STOP_CONTAINER = Win32.WM_USER + 1;
            internal const uint WM_ADD_SERVER = Win32.WM_USER + 2;

            public Thread thread = null;
            uint threadid = 0;
            CustomWindow server;

            private void MessagePump()
            {
                threadid = Win32.GetCurrentThreadId();
                sbyte bRet;
                Win32.MSG msg = new Win32.MSG();
                while ((bRet = Win32.GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0)
                {
                    if (msg.message == WM_STOP_CONTAINER)
                        break;

                    if (msg.message == WM_ADD_SERVER)
                    {
                        object[] data = (object[])GCHandle.FromIntPtr(msg.wParam).Target;
                        server = (CustomWindow )data[0];
                        server.Create();
                        ((EventWaitHandle)data[1]).Set();
                    }
                    
                    Win32.DispatchMessage(ref msg);
                }
            }

            public ServerContainer()
            {
                thread = new Thread(MessagePump);
            }

            public void Start()
            {
                thread.Start();
                while (!Win32.PostThreadMessage(threadid, WM_START_CONTAINER, IntPtr.Zero, IntPtr.Zero))
                    System.Threading.Thread.Sleep(10);
            }

            public void Stop()
            {
                server.Close();
                Win32.PostThreadMessage(threadid, WM_STOP_CONTAINER, IntPtr.Zero, IntPtr.Zero);
            }

            public void CreateServer(CustomWindow server)
            {
                object[] data = { server, new EventWaitHandle(false, EventResetMode.ManualReset)};
                GCHandle gch = GCHandle.Alloc(data);
                Win32.PostThreadMessage(threadid, WM_ADD_SERVER, GCHandle.ToIntPtr(gch), IntPtr.Zero);
                ((EventWaitHandle)data[1]).WaitOne();
            }
        }

        public abstract class CustomWindow 
        {
            internal const uint WM_PROCESS_CMD = Win32.WM_USER + 3;

            protected IntPtr windowHandle;
            Win32.WNDCLASS wind_class = new Win32.WNDCLASS();
            UInt16 classatom;
            string classname;
            string winname;
            protected CustomWindow(string classname, string winname)
            {
                this.winname = winname;
                this.classname = classname;
            }

            public virtual void Create()
            {
                Win32.DisableProcessWindowsGhosting();
                wind_class.lpszClassName = classname;
                wind_class.lpfnWndProc = Win32.DefWindowProcW;
                classatom = Win32.RegisterClassW(ref wind_class);
                // Create window 
                windowHandle = Win32.CreateWindowExW(
                     0,
                     classname,
                     winname,
                     0,
                     0,
                     0,
                     0,
                     0,
                     IntPtr.Zero,
                     IntPtr.Zero,
                     IntPtr.Zero,
                     IntPtr.Zero
                 );
            }

            public static IntPtr GetServerWindowHandle(string uniquename)
            {
                return Win32.FindWindow(null, uniquename);
            }

            public static void PostProcessMessage(IntPtr hwnd)
            {
                Win32.SendIntNotifyMessage(hwnd, WM_PROCESS_CMD, IntPtr.Zero, IntPtr.Zero);
            }

            public virtual void Close()
            {
                Win32.DestroyWindow(windowHandle );
            }
        }

        public class SingleActionServer : SimpleIPC.Windows.CustomWindow
        {
            private Win32.NativeWndProc newWndProc;
            private IntPtr fp;

            public SingleActionServer(string classname, Delegate action)
                : base(classname, classname)
            {
                DoAction = action;
            }

            public override void Create()
            {
                base.Create();
                newWndProc = new Win32.NativeWndProc(ActionhandlerWndProc);
                fp = Marshal.GetFunctionPointerForDelegate(newWndProc);
                Win32.SetWindowLong(windowHandle, Win32.GWL_WNDPROC, fp);
            }

            public IntPtr ActionhandlerWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
            {
                switch (msg)
                {
                    case WM_PROCESS_CMD:
                        if (wParam != IntPtr.Zero)
                        {
                            GCHandle gch = GCHandle.FromIntPtr(wParam);
                            DoAction.DynamicInvoke((object[])gch.Target);
                            gch.Free();
                        }
                        else
                            DoAction.DynamicInvoke();
                        break;

                    default:
                        return SimpleIPC.Win32.DefWindowProcW(hWnd, msg, wParam, lParam);
                }

                return IntPtr.Zero;
            }

            public Delegate DoAction;

            public void AddItem(params object[] args)
            {
                GCHandle gch = GCHandle.Alloc(args, GCHandleType.Weak);
                SimpleIPC.Win32.SendIntNotifyMessage(windowHandle, WM_PROCESS_CMD, GCHandle.ToIntPtr(gch), IntPtr.Zero);
            }

            public override void Close()
            {
                base.Close();
            }
        }

        public class SIPCServer : SingleActionServer
        {
            private SIPCServerImpl serverimpl;
            public SIPCServer(string uniquename, object serverinstance)
                : base(uniquename, null)
            {
                serverimpl = new SIPCServerImpl(uniquename, serverinstance);
                DoAction = new Action(serverimpl.process);
            }

            public override  void Close()
            {
                serverimpl.Close();
                base.Close();
            }

        }

        public class SIPCProxy : ProxyBase 
        {
            IntPtr hwnd;
            public SIPCProxy(string uniquename)
                : base(uniquename)
            {
                hwnd = CustomWindow.GetServerWindowHandle(uniquename);
            }

            public override void PostProcessMessage()
            {
                CustomWindow.PostProcessMessage(hwnd);
            }

            public override void Close()
            {
                hwnd = IntPtr.Zero;
            }

            public override bool IsServerAlive()
            {
                return (hwnd != null);
            }


        }
    }
    #endregion Windows

    #region NamedObject
    namespace NamedObject
    {
        public class SIPCServer : IDisposable
        {
            private SIPCServerImpl serverimpl;
            private EventWaitHandle evtserver= null;
            public Thread thread = null;
            private uint threadid = 0;
            private long started = 0;
            private long stop = 0;

            public SIPCServer(string uniquename, object serverinstance)
            {
                serverimpl = new SIPCServerImpl(uniquename, serverinstance);
                evtserver = new EventWaitHandle(false, EventResetMode.AutoReset,uniquename);
                Name = uniquename;
                thread = new Thread(MessagePump);
            }
            
            public string Name { get; private set; }

            private void MessagePump()
            {
                threadid = Win32.GetCurrentThreadId();
                while (evtserver.WaitOne())
                {
                    if (Interlocked.Read(ref stop) == 1)
                        break;

                    if (Interlocked.CompareExchange(ref started, 1, 0) == 0)
                        continue;

                    serverimpl.process();
                }
            }

            public void Start()
            {
                thread.Start();
                evtserver.Set();
                while ((Interlocked.Read(ref started) == 0))
                    System.Threading.Thread.Sleep(10);
            }

            public void Stop()
            {
                while ((Interlocked.Read(ref stop) == 0))
                {
                    Interlocked.CompareExchange(ref stop, 1, 0);
                    evtserver.Set();
                    System.Threading.Thread.Sleep(10);
                }
            }


            bool disposed = false;
            public void Dispose()
            {
                Dispose(true);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposed)
                    return;

                if (disposing)
                {
                    serverimpl.Close();
                    evtserver.Close();
                }
                disposed = true;
            }

        }

        public class SIPCProxy : ProxyBase 
        {
            EventWaitHandle evtserver;
            public SIPCProxy(string uniquename)
                : base(uniquename)
            {
                try
                {
                    evtserver = EventWaitHandle.OpenExisting(uniquename);
                }
                catch
                {
                }
            }

            public override void PostProcessMessage()
            {
                evtserver.Set();
            }

            public override void Close()
            {
                evtserver.Close();
            }

            public override bool IsServerAlive()
            {
                return (evtserver != null);
            }

        }
    }
    #endregion NamedObject
}

#region Example
namespace Example
{
    [Serializable]
    public class regdata
    {
        public string name;
        public string regid;
    }

    public interface ICallInterface
    {
        string current { get; }
        string doregister(string name, bool raiseevent, out regdata reginfo);
        event Action<String,regdata> Showmessage;
    }
}
#endregion Example
