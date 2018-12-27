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

        public static string ExeName = "BusinessTour";
        private int MoneyAddress = 0;
        private int BaseAddress = 0;
        private int EncryptingKey = 0;
        private int PlayerSelected = 0;

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
            int AddressModificatior = 168 * PlayerSelected;
            int MoneyWanted = (Memory.ReadInteger(this.MoneyAddress - AddressModificatior, 4)) + ((int)this.MoneyAmount.Value);
            Memory.WriteInteger(this.MoneyAddress - AddressModificatior, MoneyWanted, 4); //Writing a 4 byte integer
            Memory.WriteInteger(this.MoneyAddress - AddressModificatior - 4, MoneyWanted ^ this.EncryptingKey, 4); //Writing a 4 byte integer
        }

        private void MoneyRemove_Click(object sender, EventArgs e)
        {
            int AddressModificatior = 168 * PlayerSelected;
            int MoneyWanted = (Memory.ReadInteger(this.MoneyAddress - AddressModificatior, 4)) - ((int)this.MoneyAmount.Value);
            Memory.WriteInteger(this.MoneyAddress - AddressModificatior, MoneyWanted, 4); //Writing a 4 byte integer
            Memory.WriteInteger(this.MoneyAddress - AddressModificatior - 4, MoneyWanted ^ this.EncryptingKey, 4); //Writing a 4 byte integer
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateAddress.RunWorkerAsync();
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlayerSelected = PlayersListBox.SelectedIndex;
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This software was made by Gayben#7736 \nSpecial thanks to Spyder#3252 for helping me manipluating the memory", "About this software", MessageBoxButtons.OK);
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
                int pointer = Memory.GetPointerAddress(BaseAddress + 0x0119187C, new int[] { 0x60, 0x54 });
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
            }
        }
    }
}
