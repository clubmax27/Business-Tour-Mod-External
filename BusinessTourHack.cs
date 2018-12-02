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

namespace BusinessTourHack
{
    public partial class BusinessTourHack : Form
    {
        public static string ExeName = "BusinessTour";
        private IntPtr MoneyAddress = IntPtr.Zero;

        public BusinessTourHack()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int MoneyWanted = (Memory.ReadInteger((int)MoneyAddress + 0x10, 4)) + ((int)this.MoneyAmount.Value);
            Memory.WriteInteger((int)MoneyAddress + 0x10, MoneyWanted, 4); //Writing a 4 byte integer
            Memory.WriteInteger((int)MoneyAddress + 0x0C, MoneyWanted ^ 444444, 4); //Writing a 4 byte integer
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AobScan.RunWorkerAsync();
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

        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This software was made by Gayben#7736 \nSpecial thanks to Spyder#3252 for helping me manipluating the memory", "About this software", MessageBoxButtons.OK);
        }

        private void AobScan_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                var targetProcess = Process.GetProcessesByName("BusinessTour").FirstOrDefault();
                if (targetProcess == null)
                {
                    SetStatusText("Can't find Business Tour, please open the game");
                    MoneyAddress = IntPtr.Zero;
                    continue;
                }

                string MoneyMask = "xxxx????xxxx????????xxxxxxxx?xxx?xxxxxxxxxxxxxxx";
                byte[] MoneyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x1C, 0xC8, 0x06, 0x00, 0x9C, 0x4C, 0x18, 0x00, 0x80, 0x84, 0x1E, 0x00, 0x01, 0x00, 0x00, 0x00, 0x1C, 0xC8, 0x06, 0x00, 0x1C, 0xC8, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x1C, 0xC8, 0x06, 0x00, 0x1D, 0xC8, 0x06, 0x00 };

                AOBScan scanner = new AOBScan();
                MoneyAddress = scanner.AobScan(MoneyBytes, MoneyMask);

                if (MoneyAddress != IntPtr.Zero)
                {
                    SetStatusText("Trainer ready : " + MoneyAddress.ToString("X"));
                }
                else
                {
                    SetStatusText("Can't find addresses, please join a game");
                }
            }
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

        private void MoneyRemove_Click(object sender, EventArgs e)
        {
            int MoneyWanted = (Memory.ReadInteger((int)MoneyAddress + 0x10, 4)) - ((int)this.MoneyAmount.Value);
            Memory.WriteInteger((int)MoneyAddress + 0x10, MoneyWanted, 4); //Writing a 4 byte integer
            Memory.WriteInteger((int)MoneyAddress + 0x0C, MoneyWanted ^ 444444, 4); //Writing a 4 byte integer
        }
    }
}
