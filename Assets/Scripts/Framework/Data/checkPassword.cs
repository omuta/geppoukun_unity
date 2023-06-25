using PreGeppou.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Framework.Data
{
    internal class CheckPassword
    {
        public static bool checkPassword(string password)
        {
            return makePassword() == password;
        }

        public static string getMACAddress()
        {
            var list = new List<PhysicalAddress>();
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var adapter in interfaces)
            {
                if (OperationalStatus.Up == adapter.OperationalStatus)
                {
                    if (NetworkInterfaceType.Unknown != adapter.NetworkInterfaceType &&
                        NetworkInterfaceType.Loopback != adapter.NetworkInterfaceType)
                    {
                        list.Add(adapter.GetPhysicalAddress());
                    }
                }
            }
            if (list.Count == 0)
            {
                return string.Empty;
            }
            return list[0].ToString();
        }

        public static string makePassword()
        {
            string password = "";
            long longMacaddress = 1L;
            long longAccount = 0L;

            string macAddress = getMACAddress();
            if (string.IsNullOrEmpty(macAddress) == true)
            {
                return "none";
            }
            string[] parameters = macAddress.Split(":");
            foreach (string param in parameters)
            {
                longMacaddress *= Convert.ToInt32(param, 16);
                int keta = longMacaddress.ToString().Length > 5 ? 5 : longMacaddress.ToString().Length;
                string strMacaddress = longMacaddress.ToString().Substring(0, keta);
                longMacaddress = long.Parse(strMacaddress);
            }
            longAccount = exchangeAccountToInteger(Common.getPrimaryAccount());

            macAddress = longMacaddress.ToString();
            string account = longAccount.ToString();

            for (int i = 0; i < macAddress.Length; i++)
            {
                int mac = int.Parse(macAddress.Substring(i, i + 1));
                int acc = 6;
                if (i < account.Length)
                {
                    acc = int.Parse(account.Substring(i, i + 1));
                }
                int result = mac * acc;
                password += result.ToString().Substring(0, 1);
            }

            if (5 < password.Length)
            {
                password = password.Substring(0, 5);
            }

            return password;
        }

        private static long exchangeAccountToInteger(string account)
        {
            string exchangedAccount = "";
            for (int i = 0; i < account.Length; i++)
            {
                string character = account.Substring(i, i + 1).ToLower();
                switch (character[0])
                {
                    case 'a':
                    case 'k':
                    case 'u':
                    case '5':
                        exchangedAccount += "1";
                        break;
                    case 'b':
                    case 'l':
                    case 'v':
                    case '6':
                        exchangedAccount += "2";
                        break;
                    case 'c':
                    case 'm':
                    case 'w':
                    case '7':
                        exchangedAccount += "3";
                        break;
                    case 'd':
                    case 'n':
                    case 'x':
                    case '8':
                        exchangedAccount += "4";
                        break;
                    case 'e':
                    case 'o':
                    case 'y':
                    case '9':
                        exchangedAccount += "5";
                        break;
                    case 'f':
                    case 'p':
                    case 'z':
                        exchangedAccount += "6";
                        break;
                    case 'g':
                    case 'q':
                    case '0':
                        exchangedAccount += "7";
                        break;
                    case 'h':
                    case 'r':
                    case '1':
                        exchangedAccount += "8";
                        break;
                    case 'i':
                    case 's':
                    case '3':
                        exchangedAccount += "9";
                        break;
                    case 'j':
                    case 't':
                    case '4':
                        exchangedAccount += "0";
                        break;
                    default:
                        exchangedAccount += "1";
                        break;
                }
            }
            exchangedAccount = exchangedAccount.Substring(0, 10);

            return long.Parse(exchangedAccount);
        }
    }
}
