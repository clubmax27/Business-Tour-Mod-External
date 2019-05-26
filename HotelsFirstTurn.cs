using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace BusinessTourHack
{
    public partial class BusinessTourHack
    {
        private void NotFirstTurn_Click(object sender, EventArgs e)
        {
            Memory.WriteInteger(_MoneyAddress - 0x48, 0, 4); //Writing a 4 byte integer
        }
    }
}
