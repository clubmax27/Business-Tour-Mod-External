using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

using System.Windows.Forms;

namespace BusinessTourHack
{
    public class AOBScan
    {
        //protected uint ProcessID;
        public AOBScan(/*uint ProcessID*/)
        {
            //this.ProcessID = ProcessID;
        }

        [DllImport("kernel32.dll")]
        protected static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern Int32 VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [StructLayout(LayoutKind.Sequential)]
        protected struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public uint RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        protected List<MEMORY_BASIC_INFORMATION> MemoryRegion { get; set; }

        protected void MemInfo(IntPtr pHandle)
        {
            IntPtr Addy = new IntPtr();
            while (true)
            {
                MEMORY_BASIC_INFORMATION MemInfo = new MEMORY_BASIC_INFORMATION();
                int MemDump = VirtualQueryEx(pHandle, Addy, out MemInfo, (uint)Marshal.SizeOf(MemInfo));
                if (MemDump == 0) break;
                if ((MemInfo.State & 0x1000) != 0 && (MemInfo.Protect & 0x100) == 0)
                    MemoryRegion.Add(MemInfo);
                Addy = new IntPtr(MemInfo.BaseAddress.ToInt32() + (int)MemInfo.RegionSize);
            }
        }
        protected IntPtr Scan(byte[] sIn, byte[] sFor)
        {
            int[] sBytes = new int[256]; int Pool = 0;
            int End = sFor.Length - 1;
            for (int i = 0; i < 256; i++)
                sBytes[i] = sFor.Length;
            for (int i = 0; i < End; i++)
                sBytes[sFor[i]] = End - i;
            while (Pool <= sIn.Length - sFor.Length)
            {
                for (int i = End; sIn[Pool + i] == sFor[i]; i--)
                    if (i == 0) return new IntPtr(Pool);
                Pool += sBytes[sIn[Pool + End]];
            }
            return IntPtr.Zero;
        }
        public IntPtr AobScan(byte[] Pattern, string Mask)
        {
            Process Game = Process.GetProcessesByName("BusinessTour")[0];
            if (Game.Id == 0) return IntPtr.Zero;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                byte[] buff = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                IntPtr Result = Scan(buff, Pattern);
                if (Result != IntPtr.Zero)
                {
                    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
                }
            }
            return IntPtr.Zero;
        }
    }
}