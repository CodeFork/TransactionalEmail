using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TransactionalEmail.Controllers;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Tests.Helper;
using Email = TransactionalEmail.Core.Objects.Email;

namespace TransactionalEmail.Tests.UnitTests.Controllers
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class EmailController_TestFixture
    {
        private readonly Mock<IEmailService> _mockEmailService = new Mock<IEmailService>();
        private EmailController _emailController;

        [SetUp]
        public void Setup()
        {
            _emailController = ControllerHelper.GetInitialisedEmailController(_mockEmailService.Object);
            AutoMapperConfig.Bootstrap();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void EmailController_NulEmailService_ThrowsException()
        {
            //act
            var sut = new EmailController(null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void GetEmails_ZeroNumberOfEmails_ThrowsException()
        {
            //act
            var result = _emailController.GetEmails(0);
        }

        [Test]
        public void GetEmails_CallsEmailService()
        {
            //arrange
            const int numberOfEmails = 10;

            _mockEmailService.ResetCalls();
            _mockEmailService.Setup(x => x.RetrieveMessages(numberOfEmails)).Returns(new List<Email>());

            //act
            var result = _emailController.GetEmails(numberOfEmails);

            //assert
            _mockEmailService.Verify(x => x.RetrieveMessages(numberOfEmails), Times.Once);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Send_NullEmail_ThrowsException()
        {
            //act
            var result = _emailController.Send(null);
        }

        [Test]
        public void Send_Email_CallsEmailService()
        {
            //arrange
            _mockEmailService.ResetCalls();
            _mockEmailService.Setup(x => x.Send(It.IsAny<Email>())).Returns(true);

            //act
            var result = _emailController.Send(new Models.Email());

            //assert
            _mockEmailService.Verify(x => x.Send(It.IsAny<Email>()), Times.Once);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void NotifyRetrievalResult_NullRetrievalResult_ThrowsException()
        {
            //act
            var result = _emailController.NotifyRetrievalResult(null);
        }

        [Test]
        public void NotifyRetrievalResult_RetrievalResult_CallsEmailService()
        {
            //arrange
            _mockEmailService.ResetCalls();
            _mockEmailService.Setup(x => x.NotifyRetrievalResult(It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

            //act
            var result = _emailController.NotifyRetrievalResult(new Models.RetrievalResult());

            //assert
            _mockEmailService.Verify(x => x.NotifyRetrievalResult(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }
    }
}