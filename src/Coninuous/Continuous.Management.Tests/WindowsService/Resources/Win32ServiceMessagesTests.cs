using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Continuous.Management.WindowsService.Resources;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Continuous.Management.Library.Tests.WindowsService.Resources
{
    [TestFixture]
    public class Win32ServiceMessagesTests
    {
        private IWin32ServiceMessages _messages;

        [SetUp]
        public void SetUp()
        {
            _messages = new Win32ServiceMessages();
        }

        [Test]
        public void Can_GetMessage_FromProperCode()
        {
            int code = 6;
            string expectedMessage = "The service has not been started";

            var actualMessage = _messages.GetMessage(code);

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public void Can_GetDefaultMessage_FromUnvalidCode()
        {
            int code = -999;
            string expectedMessage = "unknown code -999";

            var actualMessage = _messages.GetMessage(code);

            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
