using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CacheStream
{
    public interface IIOHelper
    {
        void WroteBytes(int pid, IntPtr handle, int offset, int len);
        void WroteMemBytes(int pid, IntPtr address, int len);
    }

    public class IOHelper
    {
        public static uint GENERIC_READWRITE = 0xC0000000;
        public static uint SHARED_READWRITE = 3;
        public static uint OPEN_ALWAYS = 4;
        public static uint CREATE_ALWAYS = 2;
        public static uint FILE_NORMAL = 0x080;
        public static uint FILE_ATTRIBUTE_TEMPORARY = 0x100;
        public static uint FILE_FLAG_DELETE_ON_CLOSE = 0x04000000;
        public static uint FILE_SHARE_DELETE = 0x00000004;
        public static int ALL_ACCESS = 0x001F0FFF;
        public static int DUPLICATE_SAME_ACCESS = 2;


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(IntPtr hFile, [Out] IntPtr lpBuffer,
           uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, ref NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", EntryPoint = "WriteFile")]
        public static extern bool WriteFile(IntPtr hFile, IntPtr lpBuffer,
           uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, ref NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle,
           IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle,
           int dwDesiredAccess, int bInheritHandle, int dwOptions);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile(
           String lpFileName, uint dwDesiredAccess, uint dwShareMode,
           IntPtr lpSecurityAttributes, uint dwCreationDisposition,
           uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, ref uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, ref uint lpNumberOfBytesWritten);

        public IntPtr hImageFile;

        public IOHelper()
        {

        }

        public IOHelper(string filename)
        {
            hImageFile = CreateFile(filename, GENERIC_READWRITE, SHARED_READWRITE | FILE_SHARE_DELETE, IntPtr.Zero, OPEN_ALWAYS, FILE_ATTRIBUTE_TEMPORARY | FILE_FLAG_DELETE_ON_CLOSE, IntPtr.Zero);
        }

        public IOHelper(int sourcepid, IntPtr sourcehandle)
        {
            DuplicateHandle(OpenProcess(ALL_ACCESS, 0, sourcepid), sourcehandle, new IntPtr(-1), out hImageFile, DUPLICATE_SAME_ACCESS, 0, DUPLICATE_SAME_ACCESS);
        }

        public bool Read(byte[] buffer, int position, int length)
        {
            uint NumberOfBytesRead=0;
            NativeOverlapped Overlapped=new NativeOverlapped();
            bool ret=false;

            Overlapped.OffsetLow = position;
            EventWaitHandle ev = new EventWaitHandle(false, EventResetMode.ManualReset);
            Overlapped.EventHandle = ev.SafeWaitHandle.DangerousGetHandle();
            
            unsafe
            {
                fixed(byte* ptr=buffer)
                {
                    ret =  ReadFile(hImageFile, new IntPtr(ptr), (uint) length, out NumberOfBytesRead, ref  Overlapped);
                    ev.WaitOne();
                }
            }
            ev.Close();

            return ret;
        }

        public bool Write(byte[] buffer, int position, int length)
        {
            uint lpNumberOfBytesWritten=0;
            NativeOverlapped Overlapped=new NativeOverlapped();
            bool ret=false;

            Overlapped.OffsetLow = position;
            EventWaitHandle ev = new EventWaitHandle(false, EventResetMode.ManualReset);
            Overlapped.EventHandle = ev.SafeWaitHandle.DangerousGetHandle();
            
            unsafe
            {
                fixed(byte* ptr=buffer)
                {
                    ret =  WriteFile(hImageFile, new IntPtr(ptr), (uint)length, out lpNumberOfBytesWritten, ref Overlapped);
                    ev.WaitOne();
                }
            }
            ev.Close();

            return ret;
        }

        public bool ReadMemory(int sourcepid, IntPtr address, byte[] buffer, int length)
        {
            uint lpNumberOfBytesRead = 0;
            unsafe
            {
                fixed (byte *bufptr=buffer)
                {
                    return ReadProcessMemory(OpenProcess(ALL_ACCESS, 0, sourcepid),address, new IntPtr(bufptr), (uint)length, ref lpNumberOfBytesRead);
                }
            }
        }

        public bool WriteMemory(int destpid, IntPtr address, byte[] buffer, int length)
        {
            uint lpNumberOfBytesWrote = 0;
            unsafe
            {
                fixed (byte* bufptr = buffer)
                {
                    return WriteProcessMemory(OpenProcess(ALL_ACCESS, 0, destpid), address, new IntPtr(bufptr), (uint)length, ref lpNumberOfBytesWrote);
                }
            }
        }
    }
}
