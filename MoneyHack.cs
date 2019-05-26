using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace BusinessTourHack
{
    public partial class BusinessTourHack
    {
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
    }
}
