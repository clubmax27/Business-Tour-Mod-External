using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace BusinessTourMod
{
    public partial class BusinessTourMod : Form
    {
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
    }
}
