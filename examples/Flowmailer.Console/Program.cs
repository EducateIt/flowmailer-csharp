using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetEnv;
using Flowmailer.Models;

namespace Flowmailer.ConsoleApp
{
    public static class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Hello World!");

            Env.Load(".env");

            var clientId = Env.GetString("FLOWMAILER_API_CLIENTID");
            var clientSecret = Env.GetString("FLOWMAILER_API_CLIENTSECRET");
            var accountId = Env.GetString("FLOWMAILER_API_ACCOUNTID");
            var fromAddress = Env.GetString("FLOWMAILER_API_FROMADDRESS");
            var fromName = Env.GetString("FLOWMAILER_API_FROMNAME");
            var senderAddress = Env.GetString("FLOWMAILER_API_SENDERADDRESS");

            var client = new FlowmailerClient(clientId, clientSecret, accountId);

            var message = new Message
            {
                Data = new Dictionary<string, string> { { "var1", "value3 "} },
                HeaderFromAddress = fromAddress,
                HeaderFromName = fromName,
                HeaderToName = "Fredrik Olsson",
                MessageType = Message.MessageTypes.EMAIL,
                RecipientAddress = "fr3gu77@gmail.com",
                SenderAddress = senderAddress,
                Subject = "A test email sent via Flowmailer API",
                Html = "<html></head><title>A test email</title></head><body>This is a test!</body></html>"
            };

            await client.SendMessage(message);

            Console.WriteLine();
            Console.Write("Press ENTER to exit...");

            Console.ReadLine();
        }
    }
}
