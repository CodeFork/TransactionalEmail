using System;
using Moq;
using NUnit.Framework;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Tests.Helper;

namespace TransactionalEmail.Tests.UnitTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class RoutingConfig_TestFixture
    {
        private readonly Mock<IEmailService> _mockEmailService = new Mock<IEmailService>();

        [Test]
        public void AttributeRouting_Is_CorrectlyConfigured()
        {
            //act
            try
            {
                var emailController = ControllerHelper.GetInitialisedEmailController(_mockEmailService.Object);
            }
            catch (Exception ex)
            {
                Assert.Fail("There is a problem with the attribute based routing configuration");
            }

        }
    }
}
