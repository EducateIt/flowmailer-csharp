﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetEnv;
using Flowmailer.Models;

namespace Flowmailer.ConsoleApp
{
    public static class Program
    {
        public static async Task Main()
        {
            Env.TraversePath().Load();

            var clientId = Env.GetString("FLOWMAILER_API_CLIENTID");
            var clientSecret = Env.GetString("FLOWMAILER_API_CLIENTSECRET");
            var accountId = Env.GetString("FLOWMAILER_API_ACCOUNTID");
            var fromAddress = Env.GetString("FLOWMAILER_API_FROMADDRESS");
            var fromName = Env.GetString("FLOWMAILER_API_FROMNAME");
            var senderAddress = Env.GetString("FLOWMAILER_API_SENDERADDRESS");

            var client = new FlowmailerClient(clientId, clientSecret, accountId);

            //var message = new SubmitMessage
            //{
            //    Data = new Dictionary<string, string> { { "var1", "value3 "} },
            //    HeaderFromAddress = fromAddress,
            //    HeaderFromName = fromName,
            //    HeaderToName = "Fredrik Olsson",
            //    MessageType = MessageTypes.EMAIL,
            //    RecipientAddress = "fr3gu77@gmail.com",
            //    SenderAddress = senderAddress,
            //    Subject = "A test email sent via Flowmailer API",
            //    Html = "<html><head><title>A test email</title></head><body>This is a test!</body></html>"
            //};

            //var result = await client.SendMessageAsync(message);

            //var from = DateTime.Parse("2020-09-09 18:06:58Z").ToUniversalTime();
            //var to = DateTime.Parse("2021-10-20 12:52:23Z").ToUniversalTime();
            //var messageEvents = await client.GetMessageEventsAsync(from, to);

            //foreach (var messageEvent in messageEvents)
            //{
            //    Console.WriteLine($"MessageEventId: {messageEvent.Id}, MessageId: {messageEvent.MessageId}, type: {messageEvent.Type}");
            //}

            //Console.WriteLine("**************************");

            var message = await client.GetMessageAsync("20210816140624a31c008a1a1ea33e83");
            Console.WriteLine($"Message ID: {message.Id}. Number of events: {message.Events.Length}");
            foreach (var messageEvent in message.Events.OrderByDescending(e => e.Received))
            {
                Console.WriteLine($"{messageEvent.Received:yyyy-MM-dd kl. HH:mm:ss} :: {messageEvent.Type}");
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.Write("Press ENTER to exit...");

            Console.ReadLine();
        }
    }
}
