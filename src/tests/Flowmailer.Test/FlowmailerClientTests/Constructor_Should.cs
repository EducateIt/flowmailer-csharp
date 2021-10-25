using System;
using System.Net;
using Flowmailer.Helpers.Errors.Models;
using Flowmailer.Models;
using Flowmailer.Test.Core;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace Flowmailer.Test.FlowmailerClientTests
{
    [TestFixture]
    public class Constructor_Should
    {
        [Test]
        public void Throw_GivenNullCredentialsId()
        {
            Assert.That(() => new FlowmailerClient("", "sdfsdfsdfsdfsfsd", "1234"), Throws.ArgumentNullException);
            Assert.That(() => new FlowmailerClient("sdfsdfsdfsdfsfsd", " ", "1234"), Throws.ArgumentNullException);
            Assert.That(() => new FlowmailerClient("sdfsdfsdfsdfsfsd", "sdfsdfsdfsdfsfsd", null), Throws.ArgumentNullException);
        }


    }

    [TestFixture]
    public class GetAccessToken_Should
    {
        [Test]
        public void ReturnOK_GivenFakeFlowmailerClient()
        {
            var oAuthResult = new OAuthTokenResponse
            {
                AccessToken = "the token",
                ExpiresIn = 59,
                TokenType = "bearer",
                Scope = "api"
            };

            var response = new Mock<IRestResponse>();
            response.Setup(r => r.IsSuccessful).Returns(true);
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.OK);
            response.Setup(r => r.Content).Returns(JsonConvert.SerializeObject(oAuthResult));

            var sut = new FakeFlowmailerClient("1234", "2345", "1234", s => CreateAndSetupMockIRestClient(s, response.Object));

            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Throw_GivenErroneousCredentials()
        {
            var response = new Mock<IRestResponse>();
            response.Setup(r => r.IsSuccessful).Returns(false);
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.Unauthorized);
            response.Setup(r => r.Content).Returns(default(string));

            Assert.That(() => new FakeFlowmailerClient("1234", "2345", "1234", s => CreateAndSetupMockIRestClient(s, response.Object)), Throws.InstanceOf<FlowmailerClientConstructionException>().With.InnerException.InstanceOf<UnauthorizedException>());
        }

        private static IRestClient CreateAndSetupMockIRestClient(string url, IRestResponse expected)
        {
            var mockClient = new Mock<IRestClient>();
            mockClient.SetupGet(c => c.BaseUrl).Returns(new Uri(url));
            mockClient.Setup(c => c.Execute(It.IsAny<IRestRequest>(), Method.POST)).Returns(expected);

            return mockClient.Object;
        }
    }
}
