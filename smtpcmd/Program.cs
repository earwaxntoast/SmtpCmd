using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Security;


namespace smtpcmd
{
    class Program
    {
        
        const string myVersion = "0.5";
        //0.1 - 08/20/2013 - Initial Coding
        //0.2 - 12/12/2016 - Added the option for CC and BCC lines
        //0.3 - 01/25/2017 - Added authentication support, Logging tag, function rewrite
        //0.4 - 09/20/2017 - Compiled for .net v4.5
        //0.5 - 03/28/2018 - Add option to save/use default password. 

        static void Main(string[] args)
        {
            smtparg SmtpArg = new smtparg(args);

            if (args.Length == 0)
            {
                ShowInfo();
                return;
            }

            //Console.WriteLine("my password: " +SmtpArg.Password);
            if (SmtpArg.AllowSend)
            SendMail(SmtpArg);
        }

        static void SendMail(smtparg sa)
        {
            
            MailMessage myMessage = new MailMessage(sa.FromAddress, sa.ToAddress, sa.Subject, sa.Body);

            if (!(sa.Attachment == ""))
            {
                if (sa.WillLog)
                    System.Console.WriteLine("Attaching " + sa.Attachment);
                Attachment data = new Attachment(sa.Attachment);
                myMessage.Attachments.Add(data);
            }

            if (!String.IsNullOrEmpty(sa.CCAddress))
                myMessage.CC.Add(sa.CCAddress);
            if (!String.IsNullOrEmpty(sa.BCCAddress))
                myMessage.Bcc.Add(sa.BCCAddress);
            SmtpClient smtpClient = new SmtpClient(sa.Server);
            smtpClient.Port = sa.Port;
            if (!(String.IsNullOrEmpty(sa.Username)) && !(String.IsNullOrEmpty(sa.Password)))
                smtpClient.Credentials = new System.Net.NetworkCredential(sa.Username, sa.Password);

            try
            {
                smtpClient.Send(myMessage);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.ToString());
            }
        }

        static void ShowInfo()
        {
            System.Console.WriteLine("smtpcmd version " + myVersion);
            System.Console.WriteLine("");
            System.Console.WriteLine("This program will allow you to send mail via command line. Useful for batch jobs notification.");
            System.Console.WriteLine("");
            System.Console.WriteLine("Arguments:");
            System.Console.WriteLine("");
            System.Console.WriteLine("-S : Server. Default = 127.0.0.1");
            System.Console.WriteLine("-P : Port. Default = 25");
            System.Console.WriteLine("-T : To Address. Required. Separate multiple recipients by comma.");
            System.Console.WriteLine("-C : CC Address. Separate multiple recipients by comma.");
            System.Console.WriteLine("-V : BCC Address. Separate multiple recipients by comma.");
            System.Console.WriteLine("-F : From Address. Default = test@test.com");
            System.Console.WriteLine("-J : Subject.");
            System.Console.WriteLine("-B : Body.");
            System.Console.WriteLine("-O : Path to text file to be used as the email body.");
            System.Console.WriteLine("-U : Username to authenticate SMTP Server.");
            System.Console.WriteLine("-W : Password to authenticate SMTP Server.");
            System.Console.WriteLine("-R : {Num} Store an encrypted password. Use a single digit number after R. Use this param by itself. Ex: -R 1 MyPass");
            System.Console.WriteLine("-{Num} : Use saved encrypted password to authenticate SMTP Server. Use a single digit number that corresponds to a saved password.");
            System.Console.WriteLine("-L : Log to standard output.");
            System.Console.WriteLine("-A : Attachment.");

            return;
        }
    }
}
