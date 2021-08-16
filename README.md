# flowmailer-csharp

C# client library and examples for using Flowmailer API's to send mail and access API endpoints with .NET 4.5.2, .NET Standard 2 and .NET Core support.

## Get started

```var client = new FlowmailerClient(clientId, clientSecret, accountId);

var message = new SubmitMessage
{
    Data = new Dictionary<string, string> { { "var1", "value3 "} },
    HeaderFromAddress = fromAddress,
    HeaderFromName = fromName,
    HeaderToName = "Fredrik Olsson",
    MessageType = MessageTypes.EMAIL,
    RecipientAddress = "fr3gu77@gmail.com",
    SenderAddress = senderAddress,
    Subject = "A test email sent via Flowmailer API",
    Html = "<html></head><title>A test email</title></head><body>This is a test!</body></html>"
};

await client.SendMessage(message);
```
