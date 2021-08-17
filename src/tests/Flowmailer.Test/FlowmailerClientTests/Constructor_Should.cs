using NUnit.Framework;

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
}
