using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace BusinessTourHack
{
    public partial class BusinessTourHack
    {
        private void CheckDebugger_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                string[] Processes = {"Medusa", "Relyze", "ODA", "Hiew", "decompiler", "Disassembler", "Hopper", "radare", "Debugger", "dbg", "IlSpy", "DnSpy", "Binary", "IDA", "ArtMoney", "scanmem", "GameConqueror", "Cheat", "Squalr", "iHaxGamez", "Bit Slicer", "PINCE", "GameGuardian", "Hacking"};
                
                foreach (Process P in Process.GetProcesses())
                {
                    try
                    {
                        if (P.MainWindowTitle.Length <= 0)
                        {
                            continue;
                        }

                        foreach (string HackSoftware in Processes)
                        {
                            if (P.MainWindowTitle.ToUpper().Contains(HackSoftware.ToUpper()))
                            {
                                Application.Exit();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
