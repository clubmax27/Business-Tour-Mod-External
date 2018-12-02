﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Memory;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace BusinessTourHack
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int BaseAddress = 0;

            var targetProcess = Process.GetProcessesByName("BusinessTour").FirstOrDefault();
            if (targetProcess == null)
            {
                MessageBox.Show("Can't find Business Tour, please open the game");
                return;
            }
            ProcessModuleCollection modules = targetProcess.Modules;

            foreach (ProcessModule procmodule in modules)
            {
                if ("steam_api.dll" == procmodule.ModuleName)
                {
                    BaseAddress = (int)procmodule.BaseAddress;
                }
            }

            byte[] SteamID1 = Memory.ReadByte(BaseAddress + 214456, 4);
            byte[] SteamID2 = Memory.ReadByte(BaseAddress + 214456 + 4, 4);

            List<byte> list1 = new List<byte>(SteamID1);
            List<byte> list2 = new List<byte>(SteamID2);
            list1.AddRange(list2);

            byte[] TempSteamID = list1.ToArray();
            long SteamID64 = BitConverter.ToInt64(TempSteamID, 0);

            WebResponse response = WebRequest.Create("http://176.188.41.30/registeredIDs.txt").GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string SteamIDList = streamReader.ReadToEnd();
            streamReader.Close();
            response.Close();

            string PlayerSteamID;
            using (MD5 md = MD5.Create())
            {
                byte[] array = md.ComputeHash(Encoding.UTF8.GetBytes(SteamID64.ToString()));
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append(array[i].ToString("x2"));
                }
                PlayerSteamID = stringBuilder.ToString();
            }

            if (!SteamIDList.Contains(PlayerSteamID))
            {
                MessageBox.Show("You aren't allowed to use this software, if you think this is an error, contact Gayben#7736");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BusinessTourHack());
        }
    }
}
