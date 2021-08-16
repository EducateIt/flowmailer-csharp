using NUnit.Framework;

namespace Flowmailer.Test.FlowmailerClientTests
{
    [TestFixture]
    public class Constructor_Should
    {
        [Test]
        public void Throw_GivenNullClientId()
        {
            Assert.That(() => new FlowmailerClient("asdasdasd", "sdfsdfsdfsdfsfsd", "1234"), Throws.ArgumentNullException);
        }
    }
}
