using System.Runtime.InteropServices;
using System.Diagnostics;
using System;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Security;
using System.Reflection;
using System.Text;
using System.Runtime.ConstrainedExecution;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using System.Security.Permissions;


namespace Common.Utilities
{
    public delegate IntPtr NativeWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public UInt32 message;
        public IntPtr wParam;
        public IntPtr lParam;
        public UInt32 time;
        public POINT pt;
    }

    [System.Runtime.InteropServices.StructLayout(
        System.Runtime.InteropServices.LayoutKind.Sequential,
       CharSet = System.Runtime.InteropServices.CharSet.Unicode
    )]
    struct WNDCLASS
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

    static class IntPtrConverter<T>
    {
        public static IntPtr ToIntPtr(T data)
        {
            return (IntPtr)GCHandle.Alloc(data);
        }

        public static T FromIntPtr(IntPtr from)
        {
            GCHandle handle = (GCHandle)from;
            T ret= (T)handle.Target;
            handle.Free();
            return ret;
        }


    }

    class WindowManager
    {
        public const int WM_USER = 0X400;

        const int ALL_ACCESS = 0x001F0FFF;
        const int WM_QUIT = 0x12;
        const int WM_CREATE_CUSTOM_WINDOW = WM_USER + 100;


        delegate bool EnumWindowsProc(IntPtr hWnd, ref string classname);

        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, IntPtr lpWindowName);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref string enumclassname);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);

        [DllImport("user32.dll")]
        static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostThreadMessage(uint threadId, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        static extern void DisableProcessWindowsGhosting();

        [DllImport("user32.dll")]
        static extern sbyte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern System.UInt16 RegisterClassW(
            [System.Runtime.InteropServices.In] ref WNDCLASS lpWndClass
        );


        public class ClassData
        {
            public WNDCLASS wndclass;
            public Thread thread;
            public uint threadid;
        }

        class WindowData
        {
            public string classname;
            public string windowname;
            public ManualResetEvent wcevent;
            public CustomWindow wnd;
        }

        static List<IntPtr> enumwindlst = new List<IntPtr>();
        static Dictionary<string, ClassData> wind_class_map = new Dictionary<string, ClassData>();

        public static ClassData CreateClass(string classname, NativeWndProc wndproc)
        {
            DisableProcessWindowsGhosting();
            if (wind_class_map.ContainsKey(classname))
                return wind_class_map[classname];

            WNDCLASS wind_class = new WNDCLASS();
            wind_class.lpszClassName = classname;
            wind_class.lpfnWndProc = wndproc;
            Thread thuaactions = new Thread(ThreadProc);
            thuaactions.Priority = ThreadPriority.BelowNormal;
            
            ClassData cd = new ClassData();
            cd.wndclass = wind_class;
            cd.thread = thuaactions;
            wind_class_map[classname] = cd;
            return cd;
        }

        private static void ThreadProc(object ocd)
        {
            ClassData cd = (ClassData)ocd;
            UInt16 class_atom = RegisterClassW(ref cd.wndclass);
            cd.threadid = GetCurrentThreadId();
            MessagePump();
        }

        public static void Start(string classname)
        {
            wind_class_map[classname].thread.Start(wind_class_map[classname]);
            Thread.Sleep(500);
        }

        public static void Stop(string classname)
        {
            var windows = EnumWindowsforClass(classname);
            foreach (var w in windows)
            {
                CustomWindow.Close(w);
            }

            PostThreadMessage(wind_class_map[classname].threadid, WM_QUIT, IntPtr.Zero, IntPtr.Zero);
            Thread.Sleep(500);
            UnregisterClass(classname, IntPtr.Zero);
            wind_class_map.Remove(classname);
        }

        public static CustomWindow CreateWindow(string classname, string windowname)
        {
            WindowData wcd = new WindowData { classname = classname, windowname = windowname, wcevent = new ManualResetEvent(false)};
            PostThreadMessage(wind_class_map[classname].threadid, WM_CREATE_CUSTOM_WINDOW, IntPtrConverter<WindowData>.ToIntPtr(wcd), IntPtr.Zero);
            if (wcd.wcevent.WaitOne(3000))
                return wcd.wnd;
            Thread.Sleep(300);
            return null;
        }

        public static IntPtr FindWindowforClass(string classname)
        {
            return FindWindow(classname, new IntPtr(0));
        }

        private static bool enumwindows(IntPtr hWnd, ref string enumclassname)
        {
            int nRet;
            StringBuilder ClassName = new StringBuilder(100);

            nRet = GetClassName(hWnd, ClassName, ClassName.Capacity);
            if (nRet != 0 && ClassName.ToString() == enumclassname)
            {
                enumwindlst.Add(hWnd);
            }

            return nRet != 0;
        }

        public static IntPtr[] EnumWindowsforClass(string classname)
        {
            IntPtr[] retlist;
            enumwindlst.Clear();
            EnumWindows(new EnumWindowsProc(enumwindows), ref classname);
            retlist = new IntPtr[enumwindlst.Count];
            enumwindlst.CopyTo(retlist);
            return retlist;
        }


        private static void MessagePump()
        {
            sbyte bRet;
            MSG msg = new MSG();
            IntPtr hwnd = new IntPtr(0);
            while ((bRet = GetMessage(out msg, hwnd, 0, 0)) != 0)
            {
                if (bRet == -1)
                {
                    // handle the error and possibly exit
                    break;
                }
                else
                {
                    if (msg.message == WM_CREATE_CUSTOM_WINDOW)
                    {
                        WindowData wcd = IntPtrConverter<WindowData>.FromIntPtr(msg.wParam);
                        wcd.wnd = new CustomWindow(wcd.classname,wcd.windowname);
                        wcd.wcevent.Set();
                    }
                    TranslateMessage(ref msg);
                    DispatchMessage(ref msg);
                }
            }
        }
    }


    class CustomWindow
    {
        const uint WM_COPYDATA = 0X004A;
        const int WM_CLOSE = 0x0010;
        public const uint WM_USER = 0X400;

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr DefWindowProcW(
            IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam
        );

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern bool DestroyWindow(
            IntPtr hWnd
        );

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal  struct CopyDataStruct
        {
            /// <summary></summary>
            public IntPtr dwData;
            /// <summary></summary>
            public int cbData;
            /// <summary></summary>
            public IntPtr lpData;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern void PostQuitMessage(int nExitCode);

        [DllImport("kernel32", SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CreateWindowExW(
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

        static NativeWndProc defwndproc = new NativeWndProc(DefWindowProcW);
        IntPtr hwnd;
        string windowname;

        public CustomWindow(string classname, string windowname)
        {
            this.windowname = windowname;
            // Create window 
           hwnd = CreateWindowExW(
                0,
                classname,
                windowname,
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

        public static IntPtr DefHandler(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            return CustomWindow.defwndproc(hWnd, msg, wParam, lParam);
        }


        public bool SendNotifyMessage(uint Msg, IntPtr wParam, IntPtr lParam)
        {
            return SendNotifyMessage(hwnd, Msg, wParam, lParam);
        }

        public static void Close(IntPtr hwnd)
        {
            PostMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            Thread.Sleep(200);
        }

        public void Close()
        {
            Close(hwnd);
        }
    }


}











