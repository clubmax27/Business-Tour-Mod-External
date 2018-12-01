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
        private byte[] BytePage;

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
        

        private bool CheckPattern(byte[] BytePattern, string strMask, int nOffset)
        {
            // Loop the pattern and compare to the mask and dump. 
            for (int x = 0; x < BytePattern.Length; x++)
            {
                // If the mask char is a wildcard, just continue. 
                if (strMask[x] == '?')
                    continue;

                // If the mask char is not a wildcard, ensure a match is made in the pattern. 
                if ((strMask[x] == 'x') && (BytePattern[x] != this.BytePage[nOffset + x]))
                    return false;
            }

            // The loop was successful so we found the pattern. 
            return true;
        }
        
        protected IntPtr Scan(byte[] BytesPattern, string Mask)
        {
            for(int i = 0; i < (this.BytePage.Length - BytesPattern.Length); i++)
            {
                if(this.BytePage[i] == BytesPattern[0])
                {
                    if(CheckPattern(BytesPattern, Mask, i))
                    {
                        return new IntPtr(i);
                    }
                }
            }
            return IntPtr.Zero;
        }

        public IntPtr AobScan(byte[] Pattern, string Mask)
        {
            if (Pattern.Length != Mask.Length)
            {
                MessageBox.Show("The mask doesn't match the pattern", "Error");
                return IntPtr.Zero;
            }

            Process Game = Process.GetProcessesByName("BusinessTour")[0];
            if (Game.Id == 0) return IntPtr.Zero;
            MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
            MemInfo(Game.Handle);
            for (int i = 0; i < MemoryRegion.Count; i++)
            {
                this.BytePage = new byte[MemoryRegion[i].RegionSize];
                ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, this.BytePage, MemoryRegion[i].RegionSize, 0);
                IntPtr Result = Scan(Pattern, Mask);
                if (Result != IntPtr.Zero)
                {
                    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
                }
            }
            return IntPtr.Zero;
        }
    }
}