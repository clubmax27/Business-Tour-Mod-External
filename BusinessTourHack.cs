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

        private void InfiniteJail_Click(object sender, EventArgs e)
        {
           _JailedPlayer[PlayersListBox.SelectedIndex] = !_JailedPlayer[PlayersListBox.SelectedIndex];
        }
    }
}
