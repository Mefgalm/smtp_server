using MailKit.Net.Smtp;
using MimeKit;
using SmtpServer;
using SmtpServer.Mail;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LunarSmtpServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var options = new OptionsBuilder()
                .ServerName("Lunar SmtpServer")
                .Port(25)
                .MessageStore(new SampleMessageStore())
                .Build();

            var server = new SmtpServer.SmtpServer(options);
            var serverTask = server.StartAsync(cancellationTokenSource.Token);

            Console.WriteLine("Press any key to shutdown the server.");
            Console.ReadKey();

            try
            {
                cancellationTokenSource.Cancel();
                serverTask.Wait();
            }
            catch (AggregateException e)
            {
                e.Handle(exception => exception is OperationCanceledException);
            }
        }       
    }

    

    public class SampleMessageStore : MessageStore
    {
        /// <summary>
        /// Save the given message to the underlying storage system.
        /// </summary>
        /// <param name="context">The session context.</param>
        /// <param name="transaction">The SMTP message transaction to store.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A unique identifier that represents this message in the underlying message store.</returns>
        public override Task<SmtpServer.Protocol.SmtpResponse> SaveAsync(ISessionContext context, IMessageTransaction transaction, CancellationToken cancellationToken)
        {
            var textMessage = (ITextMessage)transaction.Message;

            using (var reader = new StreamReader(textMessage.Content, Encoding.UTF8))
            {
                Console.WriteLine(reader.ReadToEnd());
            }

            return Task.FromResult(SmtpServer.Protocol.SmtpResponse.Ok);
        }
    }
}
