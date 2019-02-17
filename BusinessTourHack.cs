using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace BusinessTourHack
{
    public partial class BusinessTourHack : Form
    {
        private const int OBSCURED_OBJECT_SIZE = 16;
        private readonly int _BaseAddress;
        private int _BonusAddress;
        private int _MoneyAddress;
        private int _MoneyEncryptingKey;
        private readonly bool[] _JailedPlayer = new bool[4];

        public BusinessTourHack()
        {
            Process TargetProcess = Process.GetProcessesByName("BusinessTour").FirstOrDefault();
            if (TargetProcess == null)
            {
                MessageBox.Show(@"Can't find Business Tour, please open the game");
                return;
            }
            ProcessModuleCollection Modules = TargetProcess.Modules;

            foreach (ProcessModule Procmodule in Modules)
            {
                if ("GameAssembly.dll" == Procmodule.ModuleName)
                {
                    _BaseAddress = (int)Procmodule.BaseAddress;
                }
            }
            InitializeComponent();
        }

        private void MoneyAdd_Click(object sender, EventArgs e)
        {
            int AddressModificatior = 168 * PlayersListBox.SelectedIndex;
            int MoneyWanted = (Memory.ReadInteger(_MoneyAddress - AddressModificatior, 4)) + ((int)MoneyAmount.Value);
            Memory.WriteInteger(_MoneyAddress - AddressModificatior, MoneyWanted, 4); //Writing a 4 byte integer
            Memory.WriteInteger(_MoneyAddress - AddressModificatior - 4, MoneyWanted ^ _MoneyEncryptingKey, 4); //Writing a 4 byte integer
        }

        private void MoneyRemove_Click(object sender, EventArgs e)
        {
            int AddressModificatior = 168 * PlayersListBox.SelectedIndex;
            int MoneyWanted = (Memory.ReadInteger(_MoneyAddress - AddressModificatior, 4)) - ((int)MoneyAmount.Value);
            if(MoneyWanted < 0)
            {
                MoneyWanted = 0;
            }
            Memory.WriteInteger(_MoneyAddress - AddressModificatior, MoneyWanted, 4); //Writing a 4 byte integer
            Memory.WriteInteger(_MoneyAddress - AddressModificatior - 4, MoneyWanted ^ _MoneyEncryptingKey, 4); //Writing a 4 byte integer
        }

        private void InfiniteJail_Click(object sender, EventArgs e)
        {
           _JailedPlayer[PlayersListBox.SelectedIndex] = !_JailedPlayer[PlayersListBox.SelectedIndex];
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
            int ScrollValue = MoneyTrackBar.Value;
            switch (ScrollValue)
            {
                case 1:
                    MoneyAmount.Value = 25000;
                    break;

                case 2:
                    MoneyAmount.Value = 50000;
                    break;

                case 3:
                    MoneyAmount.Value = 100000;
                    break;

                case 4:
                    MoneyAmount.Value = 250000;
                    break;

                case 5:
                    MoneyAmount.Value = 500000;
                    break;

                case 6:
                    MoneyAmount.Value = 1000000;
                    break;

                case 7:
                    MoneyAmount.Value = 2500000;
                    break;

                case 8:
                    MoneyAmount.Value = 5000000;
                    break;

                case 9:
                    MoneyAmount.Value = 10000000;
                    break;

                case 10:
                    MoneyAmount.Value = 20000000;
                    break;

                default:
                    MoneyAmount.Value = 0;
                    break;
            }
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"This software was made by Gayben#7736 
Special thanks to Spyder#3252 for helping me manipulating the memory", @"About this software", MessageBoxButtons.OK);
        }

        private void NotFirstTurn_Click(object sender, EventArgs e)
        {
            Memory.WriteInteger(_MoneyAddress - 0x48, 0, 4); //Writing a 4 byte integer
        }

        private delegate void SetTextCallback(string text);

        private void SetStatusText(string text)
        {
            if (txtStatus.InvokeRequired)
            {
                SetTextCallback D = SetStatusText;
                Invoke(D, text);
            }
            else
            {
                txtStatus.Text = text;
            }
        }

        private void UpdateAddress_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                {
                    int Pointer = Memory.GetPointerAddress(_BaseAddress + 0x0117B30C, new[] { 0x10, 0xA8, 0x10, 0x54 });
                    int Money = Memory.ReadInteger(Pointer, 4);
                    int EncryptedMoney = Memory.ReadInteger(Pointer - 4, 4);
                    int EncryptingKey = Memory.ReadInteger(Pointer - 8, 4);
                    int BooleanTrue = Memory.ReadInteger(Pointer + 4, 4);

                    if((Money ^ EncryptingKey) == EncryptedMoney && BooleanTrue == 1)
                    {
                        _MoneyAddress = Pointer;
                        _MoneyEncryptingKey = EncryptingKey;
                    }
                }
                
                {
                    int Pointer = Memory.GetPointerAddress(_BaseAddress + 0x01183070, new[] { 0x24, 0x40, 0xE0, 0x5C, 0x10 });
                    int EncryptingKey = Memory.ReadInteger(Pointer, 4);
                    int EncryptedBonus = Memory.ReadInteger(Pointer + 4, 4);
                    int Bonus = Memory.ReadInteger(Pointer + 8, 4);
                    int BooleanTrue = Memory.ReadInteger(Pointer + 12, 4);
                    if((Bonus ^ EncryptingKey) == EncryptedBonus && BooleanTrue == 1)
                    {
                        _BonusAddress = Pointer;
                    }
                }
                
                System.Threading.Thread.Sleep(1000);
            }
            // ReSharper disable once FunctionNeverReturns
        }

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

        private void UpdateValues_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                {
                    if (FreePair.Checked)
                    {
                        int EncryptingKey = Memory.ReadInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6), 4);
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6) + 8, 0, 4); //Writing a 4 byte integer
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6) + 4, 0 ^ EncryptingKey, 4); //Writing a 4 byte integer
                    }
                    else
                    {
                        int EncryptingKey = Memory.ReadInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6), 4);
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6) + 8, 2, 4); //Writing a 4 byte integer
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6) + 4, 2 ^ EncryptingKey, 4); //Writing a 4 byte integer
                    }
                }
                
                {
                    if (FreeDouble.Checked)
                    {
                        int EncryptingKey = Memory.ReadInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6), 4);
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 7) + 8, 0, 4); //Writing a 4 byte integer
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 7) + 4, 0 ^ EncryptingKey, 4); //Writing a 4 byte integer
                    }
                    else
                    {
                        int EncryptingKey = Memory.ReadInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6), 4);
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 7) + 8, 2, 4); //Writing a 4 byte integer
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 7) + 4, 2 ^ EncryptingKey, 4); //Writing a 4 byte integer
                    }
                }

                {
                    if (FreeCard.Checked)
                    {
                        int EncryptingKey = Memory.ReadInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6), 4);
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 8) + 8, 0, 4); //Writing a 4 byte integer
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 8) + 4, 0 ^ EncryptingKey, 4); //Writing a 4 byte integer
                    }
                    else
                    {
                        int EncryptingKey = Memory.ReadInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6), 4);
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 8) + 8, 2, 4); //Writing a 4 byte integer
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 8) + 4, 2 ^ EncryptingKey, 4); //Writing a 4 byte integer
                    }
                }

                {
                   if (FreeReroll.Checked)
                    {
                        int EncryptingKey = Memory.ReadInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6), 4);
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 9) + 8, 0, 4); //Writing a 4 byte integer
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 9) + 4, 0 ^ EncryptingKey, 4); //Writing a 4 byte integer
                    }
                    else
                    {
                        int EncryptingKey = Memory.ReadInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 6), 4);
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 9) + 8, 3, 4); //Writing a 4 byte integer
                        Memory.WriteInteger(_BonusAddress + (OBSCURED_OBJECT_SIZE * 9) + 4, 3 ^ EncryptingKey, 4); //Writing a 4 byte integer
                    }
                }
                
                {
                    // ReSharper disable once InconsistentNaming
                    for(int i = 0; i < 4; i++)
                    {
                        if (_JailedPlayer[i])
                        {
                            int AddressModificatior = 168 * i;
                            Memory.WriteInteger((_MoneyAddress - AddressModificatior) - 20, 3, 4); //Writing a 4 byte integerMemory.WriteInteger(this.MoneyAddress - AddressModificatior - 4, MoneyWanted ^ this.EncryptingKey, 4); //Writing a 4 byte integer
                        }
                        else
                        {
                            int AddressModificatior = 168 * i;
                            Memory.WriteInteger(_MoneyAddress - AddressModificatior - 20, -1, 4); //Writing a 4 byte integerMemory.WriteInteger(this.MoneyAddress - AddressModificatior - 4, MoneyWanted ^ this.EncryptingKey, 4); //Writing a 4 byte integer
                        }

                        if(NeverJail.Checked)
                        {
                            Memory.WriteInteger(_MoneyAddress - 20, -1, 4); //Writing a 4 byte integerMemory.WriteInteger(this.MoneyAddress - AddressModificatior - 4, MoneyWanted ^ this.EncryptingKey, 4); //Writing a 4 byte integer
                        }
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void UpdateColor_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            while(true)
            {

                // ReSharper disable once InconsistentNaming
                for(int i = 0; i < 4; i++)
                {
                    int AddressModificatior = 168 * i;
                    int Money = (Memory.ReadInteger(_MoneyAddress - AddressModificatior, 4));
                    int EncryptedMoney = (Memory.ReadInteger(_MoneyAddress - AddressModificatior - 4, 4));
                    int EncryptedKey = (Memory.ReadInteger(_MoneyAddress - AddressModificatior - 8, 4));
                    int BoolValue = (Memory.ReadInteger(_MoneyAddress - AddressModificatior + 4, 4));
                    
                    if ((Money ^ EncryptedKey) == EncryptedMoney && BoolValue == 1)
                    {
                        int ColorNumber = Memory.ReadInteger((_MoneyAddress - AddressModificatior) - 68, 4);
                        string Color = Index2Color(ColorNumber);
                        if(PlayersListBox.Items.Count < i + 1)
                        {
                            PlayersListBox.Items.Insert(i, Color);
                        }
                        else
                        {
                            if (PlayersListBox.Items[i].ToString() != Color)
                            {
                                PlayersListBox.Items.RemoveAt(i);
                                PlayersListBox.Items.Insert(i, Color);
                            }
                        }
                        if(i == 0)
                        {
                            SetStatusText("The host is " + Color);
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
                System.Threading.Thread.Sleep(100);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static string Index2Color(int index)
        {
            switch (index)
            {
                case 0:
                    return "Blue player";
                case 1:
                    return "Blue player";
                case 2:
                    return "Green player";
                case 3:
                    return "Red player";
                case 4:
                    return "Red player";
                case 5:
                    return "Yellow player";
                default:
                    return "Undefined color Player";
            }
        }
    }
}
