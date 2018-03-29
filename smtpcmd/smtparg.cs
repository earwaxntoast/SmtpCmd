using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security;

namespace smtpcmd
{
    class smtparg
    {

        static byte[] entropy = Encoding.Unicode.GetBytes("DMCS Security are a bunch of dipshits that don't know their asses from a hole in the ground.");

        public bool AllowSend = true;

        public string Server
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string ToAddress
        {
            get;
            set;
        }

        public string CCAddress
        {
            get;
            set;
        }

        public string BCCAddress
        {
            get;
            set;
        }

        public string FromAddress
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }

        public string Attachment
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public bool WillLog
        {
            get;
            set;
        }

        public smtparg(string[] args)
        {
            string passwordStore;
            string passwordToStore;
            int count = 0;
            Server = "127.0.0.1";
            Port = 25;
            FromAddress = "test@test.com";
            Attachment = "";
            WillLog = false;
            foreach (string arg in args)
            {
                if (arg[0] == '-')
                {
                    switch (arg[1])
                    {
                        case 'S':
                            Server = args[count + 1];
                            break;
                        case 'P':
                            Port = Convert.ToInt32(args[count + 1]);
                            break;
                        case 'T':
                            ToAddress = args[count + 1];
                            break;
                        case 'C':
                            CCAddress = args[count + 1];
                            break;
                        case 'V':
                            BCCAddress = args[count + 1];
                            break;
                        case 'F':
                            FromAddress = args[count + 1];
                            break;
                        case 'J':
                            Subject = args[count + 1];
                            break;
                        case 'B':
                            Body = args[count + 1];
                            break;
                        case 'O':
                            Body = System.IO.File.ReadAllText(@args[count + 1]);
                            break;
                        case 'A':
                            Attachment = args[count + 1];
                            break;
                        case 'U':
                            Username = args[count + 1];
                            break;
                        case 'W':
                            Password = args[count + 1];
                            break;
                        case 'L':
                            WillLog = true;
                            break;
                        case 'R':
                            AllowSend = false;
                            passwordStore = args[count + 1];
                            passwordToStore = args[count + 2];
                            //Console.WriteLine("why: " + args[count + 2]);
                            SavePassword(passwordToStore, passwordStore);
                            break;
                        case '0':
                            Password = RetrievePassword(0);                            
                            break;
                        case '1':
                            Password = RetrievePassword(1);
                            break;
                        case '2':
                            Password = RetrievePassword(2);
                            break;
                        case '3':
                            Password = RetrievePassword(3);
                            break;
                        case '4':
                            Password = RetrievePassword(4);
                            break;
                        case '5':
                            Password = RetrievePassword(5);
                            break;
                        case '6':
                            Password = RetrievePassword(6);
                            break;
                        case '7':
                            Password = RetrievePassword(7);
                            break;
                        case '8':
                            Password = RetrievePassword(8);
                            break;
                        case '9':
                            Password = RetrievePassword(9);
                            break;

                    }
                }
                count++;
                
            }
           
        }

        public static string EncryptString(SecureString input)
        {
            byte[] encryptedData = System.Security.Cryptography.ProtectedData.Protect(
                System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)),
                entropy,
                System.Security.Cryptography.DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    System.Security.Cryptography.DataProtectionScope.LocalMachine);
                return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }

        static void SavePassword(string pass, string passnum)
        {
            SavePassword(pass, Convert.ToInt32(passnum));
        }

        static void SavePassword(string pass, int passnum)
        {
            string configKey = "";
            switch (passnum)
            {
                case 0:
                    configKey = "pass0";
                    break;
                case 1:
                    configKey = "pass1";
                    break;
                case 2:
                    configKey = "pass2";
                    break;
                case 3:
                    configKey = "pass3";
                    break;
                case 4:
                    configKey = "pass4";
                    break;
                case 5:
                    configKey = "pass5";
                    break;
                case 6:
                    configKey = "pass6";
                    break;
                case 7:
                    configKey = "pass7";
                    break;
                case 8:
                    configKey = "pass8";
                    break;
                case 9:
                    configKey = "pass9";
                    break;
                default:
                    break;


            }
            //Console.WriteLine("configKey: " + configKey);
            string hashedPass = EncryptString(ToSecureString(pass));
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[configKey].Value = hashedPass;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            //ConfigurationManager.AppSettings[configKey] = hashedPass;

            //ConfigurationManager.AppSettings.Remove(configKey);
            //ConfigurationManager.AppSettings.Add(configKey, hashedPass);

        }

        static string RetrievePassword(int passnum)
        {
            string configKey = "";
            switch (passnum)
            {
                case 0:
                    configKey = "pass0";
                    break;
                case 1:
                    configKey = "pass1";
                    break;
                case 2:
                    configKey = "pass2";
                    break;
                case 3:
                    configKey = "pass3";
                    break;
                case 4:
                    configKey = "pass4";
                    break;
                case 5:
                    configKey = "pass5";
                    break;
                case 6:
                    configKey = "pass6";
                    break;
                case 7:
                    configKey = "pass7";
                    break;
                case 8:
                    configKey = "pass8";
                    break;
                case 9:
                    configKey = "pass9";
                    break;
                default:
                    break;


            }
            string p = ConfigurationManager.AppSettings[configKey];
            p = ToInsecureString(DecryptString(p));
            //Console.WriteLine("Pass is: " + p);
            return p;
        }

    }
}
