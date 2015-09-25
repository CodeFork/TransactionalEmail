using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Core.Services;
using TransactionalEmail.Infrastructure;

namespace TransactionalEmail.Tests.UnitTests.Services
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class EmailService_TestFixture
    {
        private const string Reference = "ABCDE12345";
        private Mock<IEmailServiceSettings> _mockEmailServiceSettings;
        private Mock<IMailboxConfiguration> _mockConfiguration;
        private Mock<IReferenceGenerator> _mockReferenceGenerator;
        private Mock<IEmailRepository> _mockRepository;
        private Mock<IForwardService> _mockForwardService;
        private Mock<IEmailProvider> _mockEmailProvider;
        private EmailService _emailService;

        [SetUp]
        public void Setup()
        {
            _mockEmailServiceSettings = new Mock<IEmailServiceSettings>();
            _mockConfiguration = new Mock<IMailboxConfiguration>();
            _mockReferenceGenerator = new Mock<IReferenceGenerator>();
            _mockRepository = new Mock<IEmailRepository>();
            _mockForwardService = new Mock<IForwardService>();
            _mockEmailProvider = new Mock<IEmailProvider>();

            _emailService = new EmailService(   _mockConfiguration.Object, 
                                                _mockForwardService.Object,
                                                _mockEmailProvider.Object, 
                                                _mockRepository.Object, 
                                                _mockReferenceGenerator.Object,
                                                _mockEmailServiceSettings.Object);
        }

        [Test]
        public void RetrieveMessages_Calls_Email_Provider()
        {
            //arrange
            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> {new MailboxSettings()});

            _mockEmailProvider.Setup(
                x => x.GetEmails(It.IsAny<IMailboxSettings>(), It.IsAny<int>())).Returns(new List<Email> {new Email()});

            //act
            _emailService.RetrieveMessages(10);

            //assert
            _mockEmailProvider.Verify(x => x.GetEmails(It.IsAny<IMailboxSettings>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void RetrieveMessages_Calls_Repository_Create_Email_Method_For_Each_Email()
        {
            //arrange
            var emails = new List<Email> {new Email()};

            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> { new MailboxSettings() });

            _mockEmailProvider.Setup(
                x => x.GetEmails(It.IsAny<IMailboxSettings>(), It.IsAny<int>())).Returns(emails);

            //act
            _emailService.RetrieveMessages(10);

            //assert
            _mockRepository.Verify(x => x.CreateEmail(It.IsAny<Email>()), Times.Exactly(emails.Count));
        }

        [Test]
        public void RetrieveMessages_Runs_Processing_Rules_If_Enabled_For_Each_Email()
        {
            //arrange
            var emails = new List<Email> { new Email() };

            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> { new MailboxSettings {ApplyForwardingRules =  true} });

            _mockEmailProvider.Setup(
                x => x.GetEmails(It.IsAny<IMailboxSettings>(), It.IsAny<int>())).Returns(emails);

            _mockForwardService.Setup(x => x.ProcessEmail(It.IsAny<Email>())).Returns(new ForwardResult());

            //act
            _emailService.RetrieveMessages(10);

            //assert
            _mockForwardService.Verify(x => x.ProcessEmail(It.IsAny<Email>()), Times.Exactly(emails.Count));
        }

        [Test]
        public void RetrieveMessages_Updates_Applied_Rules_For_Each_Email()
        {
            //arrange
            var emails = new List<Email> { new Email() };

            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> { new MailboxSettings { ApplyForwardingRules = true } });

            _mockEmailProvider.Setup(
                x => x.GetEmails(It.IsAny<IMailboxSettings>(), It.IsAny<int>())).Returns(emails);

            _mockForwardService.Setup(x => x.ProcessEmail(It.IsAny<Email>())).Returns(new ForwardResult());

            //act
            _emailService.RetrieveMessages(10);

            //assert
            _mockRepository.Verify(x => x.UpdateAppliedRules(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(emails.Count));
        }
        
        [Test]
        public void Send_Email_No_Outbound_Configuration_Returns_False()
        {
            //arrange
            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> { new MailboxSettings()});

            //act
            var result = _emailService.Send(new Email());

            //assert
            result.Should().BeEmpty();
        }
        
        [Test]
        public void Send_Email_Calls_Repository_Create_Email_Method()
        {
            //arrange
            _mockRepository.Setup(x => x.CreateEmail(It.IsAny<Email>()))
                .Returns(true);

            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> { new MailboxSettings { AccountName = "TestAccount", Outbound = true } });

            _mockEmailProvider.Setup(
                x => x.SendEmail(It.IsAny<IMailboxSettings>(), It.IsAny<Email>())).Returns(true);

            //act
            _emailService.Send(new Email());

            //assert
            _mockRepository.Verify(
                x => x.CreateEmail(It.IsAny<Email>()), Times.Once());
        }


        [Test]
        public void Send_Email_Calls_Email_Provider_Send_Method()
        {
            //arrange
            _mockReferenceGenerator.Setup(x => x.CreateReference(It.IsAny<int>())).Returns("ABCD1234");

            _mockEmailServiceSettings.Setup(x => x.SendEnabled).Returns(true);

            _mockRepository.Setup(x => x.CreateEmail(It.IsAny<Email>()))
                .Returns(true);

            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> { new MailboxSettings { AccountName = "TestAccount", Outbound = true } });

            _mockEmailProvider.Setup(
                x => x.SendEmail(It.IsAny<IMailboxSettings>(), It.IsAny<Email>())).Returns(true);

            //act
            var result = _emailService.Send(new Email());

            //assert
            result.Should().NotBeEmpty();

            _mockEmailProvider.Verify(
                x => x.SendEmail(It.IsAny<IMailboxSettings>(), It.IsAny<Email>()), Times.Once());
        }

        [Test]
        public void Send_Email_Calls_Repository_Update_Status_Method()
        {
            //arrange
            _mockEmailServiceSettings.Setup(x => x.SendEnabled).Returns(true);

            _mockRepository.Setup(x => x.CreateEmail(It.IsAny<Email>()))
                .Returns(true);

            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> { new MailboxSettings { AccountName = "TestAccount", Outbound = true } });

            _mockEmailProvider.Setup(
                x => x.SendEmail(It.IsAny<IMailboxSettings>(), It.IsAny<Email>())).Returns(true);

            //act
            _emailService.Send(new Email());

            //assert
            _mockRepository.Verify(
                x => x.UpdateStatus(It.IsAny<string>(), It.IsAny<Status>()), Times.Once());
        }

        [Test]
        public void Mark_Email_As_Retrieved_Email_Does_Not_Exist_Returns_False()
        {
            //arrange
            _mockRepository.Setup(x => x.GetEmailByReference(It.IsAny<string>()))
                .Returns((Email)null);

            //act
            var result = _emailService.NotifyRetrievalResult(Reference, true);

            //assert
            result.Should().BeFalse();
        }


        [Test]
        public void Mark_Email_As_Retrieved_Calls_Repository_Get_Email_By_Reference_Method()
        {
            //arrange
            _mockRepository.Setup(x => x.GetEmailByReference(It.IsAny<string>()))
                .Returns(new Email { AccountName = "TestAccount" });

            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> { new MailboxSettings { AccountName = "TestAccount" } });

            _mockEmailProvider.Setup(
                x => x.UpdateEmailRetrievalResult(It.IsAny<IMailboxSettings>(), It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(true);

            //act
            _emailService.NotifyRetrievalResult(Reference, true);

            //assert
            _mockRepository.Verify(
                x => x.GetEmailByReference(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Mark_Email_As_Retrieved_Calls_Email_Provider_Method()
        {
            //arrange
            _mockRepository.Setup(x => x.GetEmailByReference(It.IsAny<string>()))
                .Returns(new Email {AccountName = "TestAccount"});

            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> {new MailboxSettings {AccountName = "TestAccount"}});

            _mockEmailProvider.Setup(
                x => x.UpdateEmailRetrievalResult(It.IsAny<IMailboxSettings>(), It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(true);

            //act
            _emailService.NotifyRetrievalResult(Reference, true);

            //assert
            _mockEmailProvider.Verify(
                x => x.UpdateEmailRetrievalResult(It.IsAny<IMailboxSettings>(), It.IsAny<long>(), It.IsAny<bool>()),
                Times.Once());
        }

        [Test]
        public void Mark_Email_As_Retrieved_Calls_Repository_Update_Retrieval_Result_Method()
        {
            //arrange
            _mockRepository.Setup(x => x.GetEmailByReference(It.IsAny<string>()))
                .Returns(new Email { AccountName = "TestAccount" });

            _mockConfiguration.Setup(x => x.Mailboxes)
                .Returns(new List<IMailboxSettings> { new MailboxSettings { AccountName = "TestAccount" } });

            _mockEmailProvider.Setup(
                x => x.UpdateEmailRetrievalResult(It.IsAny<IMailboxSettings>(), It.IsAny<long>(), It.IsAny<bool>()))
                .Returns(true);

            //act
            _emailService.NotifyRetrievalResult(Reference, true);

            //assert
            _mockRepository.Verify(
                x => x.UpdateStatus(It.IsAny<string>(), It.IsAny<Status>()),Times.Once());
        }
    }
}