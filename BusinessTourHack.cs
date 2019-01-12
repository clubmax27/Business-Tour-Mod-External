using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Memory;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace BusinessTourHack
{

    public partial class BusinessTourHack : Form
    {
        [DllImport("kernel32.dll")]
        private static extern Int32 ReadProcessMemory(IntPtr Handle, int Address, byte[] buffer, int Size, int BytesRead = 0);

        public static int OBSCURED_OBJECT_SIZE = 16;
        public static string ExeName = "BusinessTour";
        private int MoneyAddress = 0;
        private int BaseAddress = 0;
        private int EncryptingKey = 0;
        private bool[] JailedPlayer = new bool[4];

        public BusinessTourHack()
        {
            var targetProcess = Process.GetProcessesByName("BusinessTour").FirstOrDefault();
            if (targetProcess == null)
            {
                MessageBox.Show("Can't find Business Tour, please open the game");
                return;
            }
            ProcessModuleCollection modules = targetProcess.Modules;

            foreach (ProcessModule procmodule in modules)
            {
                if ("GameAssembly.dll" == procmodule.ModuleName)
                {
                    this.BaseAddress = (int)procmodule.BaseAddress;
                }
            }
            InitializeComponent();
        }

        private void MoneyAdd_Click(object sender, EventArgs e)
        {
            int AddressModificatior = 168 * PlayersListBox.SelectedIndex;
            int MoneyWanted = (Memory.ReadInteger(this.MoneyAddress - AddressModificatior, 4)) + ((int)this.MoneyAmount.Value);
            Memory.WriteInteger(this.MoneyAddress - AddressModificatior, MoneyWanted, 4); //Writing a 4 byte integer
            Memory.WriteInteger(this.MoneyAddress - AddressModificatior - 4, MoneyWanted ^ this.EncryptingKey, 4); //Writing a 4 byte integer
        }

        private void MoneyRemove_Click(object sender, EventArgs e)
        {
            int AddressModificatior = 168 * PlayersListBox.SelectedIndex;
            int MoneyWanted = (Memory.ReadInteger(this.MoneyAddress - AddressModificatior, 4)) - ((int)this.MoneyAmount.Value);
            if(MoneyWanted < 0)
            {
                MoneyWanted = 0;
            }
            Memory.WriteInteger(this.MoneyAddress - AddressModificatior, MoneyWanted, 4); //Writing a 4 byte integer
            Memory.WriteInteger(this.MoneyAddress - AddressModificatior - 4, MoneyWanted ^ this.EncryptingKey, 4); //Writing a 4 byte integer
        }

        private void InfiniteJail_Click(object sender, EventArgs e)
        {
           JailedPlayer[PlayersListBox.SelectedIndex] = !JailedPlayer[PlayersListBox.SelectedIndex];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateAddress.RunWorkerAsync();
            CheckDebugger.RunWorkerAsync();
            UpdateValues.RunWorkerAsync();
            UpdateColor.RunWorkerAsync();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int ScrollValue = this.MoneyTrackBar.Value;
            switch (ScrollValue)
            {
                case 1:
                    this.MoneyAmount.Value = 25000;
                    break;

                case 2:
                    this.MoneyAmount.Value = 50000;
                    break;

                case 3:
                    this.MoneyAmount.Value = 100000;
                    break;

                case 4:
                    this.MoneyAmount.Value = 250000;
                    break;

                case 5:
                    this.MoneyAmount.Value = 500000;
                    break;

                case 6:
                    this.MoneyAmount.Value = 1000000;
                    break;

                case 7:
                    this.MoneyAmount.Value = 2500000;
                    break;

                case 8:
                    this.MoneyAmount.Value = 5000000;
                    break;

                case 9:
                    this.MoneyAmount.Value = 10000000;
                    break;

                case 10:
                    this.MoneyAmount.Value = 20000000;
                    break;

                default:
                    this.MoneyAmount.Value = 0;
                    break;
            }
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This software was made by Gayben#7736 \nSpecial thanks to Spyder#3252 for helping me manipluating the memory", "About this software", MessageBoxButtons.OK);
        }

        private void NotFirstTurn_Click(object sender, EventArgs e)
        {
            Memory.WriteInteger(this.MoneyAddress - 0x48, 0, 4); //Writing a 4 byte integer
        }

        delegate void SetTextCallback(string text);

        private void SetStatusText(string text)
        {
            if (this.txtStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtStatus.Text = text;
            }
        }

        private void UpdateAddress_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                int pointer = Memory.GetPointerAddress(BaseAddress + 0x0119191C, new int[] { 0x60, 0x54 });
                int Money = Memory.ReadInteger(pointer, 4);
                int EncryptedMoney = Memory.ReadInteger(pointer - 4, 4);
                int EncryptingKey = Memory.ReadInteger(pointer - 8, 4);
                int BooleanTrue = Memory.ReadInteger(pointer + 4, 4);

                if((Money ^ EncryptingKey) == EncryptedMoney && BooleanTrue == 1)
                {
                    this.MoneyAddress = pointer;
                    this.EncryptingKey = EncryptingKey;
                    SetStatusText(pointer.ToString("X"));
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void CheckDebugger_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                string[] Processes = {"Medusa", "Relyze", "ODA", "Hiew", "decompiler", "Disassembler", "Hopper", "radare", "Debugger", "dbg", "IlSpy", "DnSpy", "Binary", "IDA", "ArtMoney", "scanmem", "GameConqueror", "Cheat", "Squalr", "iHaxGamez", "Bit Slicer", "PINCE", "GameGuardian", "Hacking"};

                Process[] processlist = Process.GetProcesses();
                foreach (Process theprocess in processlist)
                {
                    string name = theprocess.ProcessName.ToString().ToUpper();
                    foreach (string HackSoftware in Processes)
                    {
                        if (name.Contains(HackSoftware.ToUpper()))
                        {
                            Application.Exit();
                        }
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void UpdateValues_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                { //FreePair
                    int pointer = Memory.GetPointerAddress(BaseAddress + 0x0115F2AC, new int[] { 0x20, 0x40, 0x20, 0x5C, 0x0 });
                    pointer += OBSCURED_OBJECT_SIZE * 7;
                    int EncryptingKey = Memory.ReadInteger(pointer, 4);
                    int EncryptedBonus = Memory.ReadInteger(pointer + 4, 4);
                    int Bonus = Memory.ReadInteger(pointer + 8, 4);
                    int BooleanTrue = Memory.ReadInteger(pointer + 12, 4);

                    if ((Bonus ^ EncryptingKey) == EncryptedBonus && BooleanTrue == 1)
                    {
                        if (FreePair.Checked)
                        {
                            Memory.WriteInteger(pointer + 8, 0, 4); //Writing a 4 byte integer
                            Memory.WriteInteger(pointer + 4, 0 ^ EncryptingKey, 4); //Writing a 4 byte integer
                        }
                        else
                        {
                            Memory.WriteInteger(pointer + 8, 2, 4); //Writing a 4 byte integer
                            Memory.WriteInteger(pointer + 4, 2 ^ EncryptingKey, 4); //Writing a 4 byte integer
                        }
                    }
                }

                {
                    int pointer = Memory.GetPointerAddress(BaseAddress + 0x0115F2AC, new int[] { 0x20, 0x40, 0x20, 0x5C, 0x0 });
                    pointer += OBSCURED_OBJECT_SIZE * 8;
                    int EncryptingKey = Memory.ReadInteger(pointer, 4);
                    int EncryptedBonus = Memory.ReadInteger(pointer + 4, 4);
                    int Bonus = Memory.ReadInteger(pointer + 8, 4);
                    int BooleanTrue = Memory.ReadInteger(pointer + 12, 4);

                    if ((Bonus ^ EncryptingKey) == EncryptedBonus && BooleanTrue == 1)
                    {
                        if (FreePair.Checked)
                        {
                            Memory.WriteInteger(pointer + 8, 0, 4); //Writing a 4 byte integer
                            Memory.WriteInteger(pointer + 4, 0 ^ EncryptingKey, 4); //Writing a 4 byte integer
                        }
                        else
                        {
                            Memory.WriteInteger(pointer + 8, 2, 4); //Writing a 4 byte integer
                            Memory.WriteInteger(pointer + 4, 2 ^ EncryptingKey, 4); //Writing a 4 byte integer
                        }
                    }
                }

                {
                    int pointer = Memory.GetPointerAddress(BaseAddress + 0x0115F2AC, new int[] { 0x20, 0x40, 0x20, 0x5C, 0x0 });
                    pointer += OBSCURED_OBJECT_SIZE * 9;
                    int EncryptingKey = Memory.ReadInteger(pointer, 4);
                    int EncryptedBonus = Memory.ReadInteger(pointer + 4, 4);
                    int Bonus = Memory.ReadInteger(pointer + 8, 4);
                    int BooleanTrue = Memory.ReadInteger(pointer + 12, 4);

                    if ((Bonus ^ EncryptingKey) == EncryptedBonus && BooleanTrue == 1)
                    {
                        if (FreePair.Checked)
                        {
                            Memory.WriteInteger(pointer + 8, 0, 4); //Writing a 4 byte integer
                            Memory.WriteInteger(pointer + 4, 0 ^ EncryptingKey, 4); //Writing a 4 byte integer
                        }
                        else
                        {
                            Memory.WriteInteger(pointer + 8, 2, 4); //Writing a 4 byte integer
                            Memory.WriteInteger(pointer + 4, 2 ^ EncryptingKey, 4); //Writing a 4 byte integer
                        }
                    }
                }

                {
                    int pointer = Memory.GetPointerAddress(BaseAddress + 0x0115F2AC, new int[] { 0x20, 0x40, 0x20, 0x5C, 0x0 });
                    pointer += OBSCURED_OBJECT_SIZE * 10;
                    int EncryptingKey = Memory.ReadInteger(pointer, 4);
                    int EncryptedBonus = Memory.ReadInteger(pointer + 4, 4);
                    int Bonus = Memory.ReadInteger(pointer + 8, 4);
                    int BooleanTrue = Memory.ReadInteger(pointer + 12, 4);

                    if ((Bonus ^ EncryptingKey) == EncryptedBonus && BooleanTrue == 1)
                    {
                        if (FreePair.Checked)
                        {
                            Memory.WriteInteger(pointer + 8, 0, 4); //Writing a 4 byte integer
                            Memory.WriteInteger(pointer + 4, 0 ^ EncryptingKey, 4); //Writing a 4 byte integer
                        }
                        else
                        {
                            Memory.WriteInteger(pointer + 8, 3, 4); //Writing a 4 byte integer
                            Memory.WriteInteger(pointer + 4, 3 ^ EncryptingKey, 4); //Writing a 4 byte integer
                        }
                    }
                }

                {
                    int pointer = Memory.GetPointerAddress(BaseAddress + 0x0115F2AC, new int[] { 0x20, 0x40, 0x20, 0x5C, 0x0 });
                    for(int i = 0; i < 4; i++)
                    {
                        if (JailedPlayer[i])
                        {
                            int AddressModificatior = 168 * i;
                            Memory.WriteInteger((this.MoneyAddress - AddressModificatior) - 20, 1000, 4); //Writing a 4 byte integerMemory.WriteInteger(this.MoneyAddress - AddressModificatior - 4, MoneyWanted ^ this.EncryptingKey, 4); //Writing a 4 byte integer
                        }
                        else
                        {
                            int AddressModificatior = 168 * i;
                            Memory.WriteInteger((this.MoneyAddress - AddressModificatior) - 20, -1, 4); //Writing a 4 byte integerMemory.WriteInteger(this.MoneyAddress - AddressModificatior - 4, MoneyWanted ^ this.EncryptingKey, 4); //Writing a 4 byte integer
                        }
                    }
                }
            }
        }

        private void UpdateColor_DoWork(object sender, DoWorkEventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            while(true)
            {

                for(int i = 0; i < 4; i++)
                {
                    int AddressModificatior = 168 * i;
                    int Money = (Memory.ReadInteger(this.MoneyAddress - AddressModificatior, 4));
                    if (Money != 0)
                    {
                        int colorNumber = Memory.ReadInteger((this.MoneyAddress - AddressModificatior) - 68, 4);
                        string color = Index2Color(colorNumber);
                        if(PlayersListBox.Items.Count < i + 1)
                        {
                            PlayersListBox.Items.Insert(i, color.ToString());
                        }
                        else
                        {
                            if (PlayersListBox.Items[i].ToString() != color.ToString())
                            {
                                PlayersListBox.Items.RemoveAt(i);
                                PlayersListBox.Items.Insert(i, color.ToString());
                            }
                        }
                    }
                    else
                    {
                        if(!(PlayersListBox.Items.Count < i + 1))
                        {
                            PlayersListBox.Items.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private string Index2Color(int index)
        {
            if(index == 0)
            {
                return "Blue player";
            }
            if (index == 1)
            {
                return "Blue player";
            }
            if (index == 2)
            {
                return "Green player";
            }
            if (index == 3)
            {
                return "Red player";
            }
            if (index == 4)
            {
                return "Red player";
            }
            if (index == 5)
            {
                return "Yellow player";
            }
            return "Undefined color Player";
        }
    }
}
