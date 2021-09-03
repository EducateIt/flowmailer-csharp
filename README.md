# flowmailer-csharp

[![.NET](https://github.com/EducateIt/flowmailer-csharp/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/EducateIt/flowmailer-csharp/actions/workflows/dotnet.yml) [![Nuget](https://img.shields.io/nuget/v/EducateIt.Flowmailer)](https://www.nuget.org/packages/EducateIt.Flowmailer/) [![codecov](https://codecov.io/gh/EducateIt/flowmailer-csharp/branch/main/graph/badge.svg?token=81FXK7VGPB)](https://codecov.io/gh/EducateIt/flowmailer-csharp) ![GitHub](https://img.shields.io/github/license/EducateIt/flowmailer-csharp)

C# client library and examples for using Flowmailer API's to send mail and access API endpoints with .NET 4.5.2, .NET Standard 2 and .NET Core support.

## Get started

API documentation: https://flowmailer.com/apidoc/flowmailer-api

```C#
var client = new FlowmailerClient(clientId, clientSecret, accountId);

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
    Html = "<html><head><title>A test email</title></head><body>This is a test!</body></html>"
};

var messageId = await client.SendMessageAsync(message);
```
