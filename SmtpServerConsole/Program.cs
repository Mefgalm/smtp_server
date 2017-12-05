using System;
using System.Collections.Generic;
using Toolbelt.Net.Smtp;

namespace SmtpServerConsole
{
    class Program
    {
        private static SmtpServerCore _Server;

        private static List<SmtpMessage> _Messages;

        static void Main(string[] args)
        {
            _Server = new SmtpServerCore();
            _Messages = new List<SmtpMessage>();
            _Server.ReceiveMessage += _Server_ReceiveMessage;
            _Server.Start();

            Console.ReadKey();
        }

        private static void _Server_ReceiveMessage(object sender, ReceiveMessageEventArgs e)
        {
            _Messages.Add(e.Message);
        }

        public void Dispose()
        {
            _Messages.Clear();
            _Server.Dispose();
        }
    }
}
