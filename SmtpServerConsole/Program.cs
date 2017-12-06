using System;
using System.Collections.Generic;
using System.Net;
using Toolbelt.Net.Smtp;

namespace SmtpServerConsole
{
    class Program
    {
        private static SmtpServerCore _Server;

        private static List<SmtpMessage> _Messages;

        static void Main(string[] args)
        {
            _Server = new SmtpServerCore(IPAddress.Any, 587);
            _Messages = new List<SmtpMessage>();
            _Server.ReceiveMessage += _Server_ReceiveMessage;
            _Server.Start();

            Console.ReadKey();
        }

        private static void _Server_ReceiveMessage(object sender, ReceiveMessageEventArgs e)
        {
            _Messages.Add(e.Message);
            Console.WriteLine(e.Message.Body);
        }

        public void Dispose()
        {
            _Messages.Clear();
            _Server.Dispose();
        }
    }
}
