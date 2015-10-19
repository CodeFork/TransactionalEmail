using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Factories;
using Attachment = TransactionalEmail.Models.Attachment;
using Email = TransactionalEmail.Models.Email;

namespace TransactionalEmail.Tests.UnitTests.Factory
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class EmailFactory_TestFixture
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperConfig.Bootstrap();    
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CreateCoreEmail_Null_EmailModel_Throws_Exception()
        {
            //Act
            var sut = EmailFactory.CreateCoreEmail(null);
        }

        [Test]
        public void CreateCoreEmail_EmailModel_Returns_Core_Email()
        {
            //Arrange
            var email = new Email
            {
                ToAddresses = "Kristian Wilson <kristian.wilson@rgroup.co.uk>",
                Subject = "Test Subject",
                PlainTextBody = "Plain Text",
                HtmlBody = "Html Body",
                Attachments = new List<Attachment> {new Attachment {AttachmentName = "Test", MimeType = "Test"}}
            };

            //Act
            var result = EmailFactory.CreateCoreEmail(email);

            //Assert
            result.EmailAddresses.Should().NotBeEmpty();
            result.EmailAddresses.Count().Should().Be(1);
            result.EmailAddresses.First().Name.Should().Be("Kristian Wilson");
            result.EmailAddresses.First().Email.Should().Be("kristian.wilson@rgroup.co.uk");
            result.EmailAddresses.First().Type.Should().Be(EmailAddressType.To);  
            result.Subject.Should().Be(email.Subject);
            result.PlainTextBody.Should().Be(email.PlainTextBody);
            result.HtmlBody.Should().Be(email.HtmlBody);
            result.Attachments.Should().NotBeEmpty();
            result.Attachments.Count().Should().Be(1);
            result.Attachments.First().AttachmentName.Should().Be("Test");
            result.Attachments.First().MimeType.Should().Be("Test");
        }

        [Test]
        public void CreateCoreEmail_MultipleEmailAddress_Returns_Core_Email_With_CorrectNumberOfAddresses()
        {
            //Arrange
            var email = new Email
            {
                ToAddresses = "Kristian Wilson <kristian.wilson@rgroup.co.uk>; Kristian Wilson <kristian.wilson@letme.com>",
                Subject = "Test Subject",
                PlainTextBody = "Plain Text",
                HtmlBody = "Html Body",
                Attachments = new List<Attachment> { new Attachment { AttachmentName = "Test", MimeType = "Test" } }
            };

            //Act
            var result = EmailFactory.CreateCoreEmail(email);

            //Assert
            result.EmailAddresses.Should().NotBeEmpty();
            result.EmailAddresses.Count().Should().Be(2);
            result.EmailAddresses.First().Name.Should().Be("Kristian Wilson");
            result.EmailAddresses.First().Email.Should().Be("kristian.wilson@rgroup.co.uk");
            result.EmailAddresses[1].Name.Should().Be("Kristian Wilson");
            result.EmailAddresses[1].Email.Should().Be("kristian.wilson@letme.com");
            result.EmailAddresses.First().Type.Should().Be(EmailAddressType.To);
            result.Subject.Should().Be(email.Subject);
            result.PlainTextBody.Should().Be(email.PlainTextBody);
            result.HtmlBody.Should().Be(email.HtmlBody);
            result.Attachments.Should().NotBeEmpty();
            result.Attachments.Count().Should().Be(1);
            result.Attachments.First().AttachmentName.Should().Be("Test");
            result.Attachments.First().MimeType.Should().Be("Test");
        }

        [Test]
        public void CreateCoreEmail_SingleEmailAddressWithErrantSeperator_Returns_Core_Email_With_SingleEmailAddress()
        {
            //Arrange
            var email = new Email
            {
                ToAddresses = "Kristian Wilson <kristian.wilson@rgroup.co.uk>;",
                Subject = "Test Subject",
                PlainTextBody = "Plain Text",
                HtmlBody = "Html Body",
                Attachments = new List<Attachment> { new Attachment { AttachmentName = "Test", MimeType = "Test" } }
            };

            //Act
            var result = EmailFactory.CreateCoreEmail(email);

            //Assert
            result.EmailAddresses.Should().NotBeEmpty();
            result.EmailAddresses.Count().Should().Be(1);
            result.EmailAddresses.First().Name.Should().Be("Kristian Wilson");
            result.EmailAddresses.First().Email.Should().Be("kristian.wilson@rgroup.co.uk");
            result.EmailAddresses.First().Type.Should().Be(EmailAddressType.To);
            result.Subject.Should().Be(email.Subject);
            result.PlainTextBody.Should().Be(email.PlainTextBody);
            result.HtmlBody.Should().Be(email.HtmlBody);
            result.Attachments.Should().NotBeEmpty();
            result.Attachments.Count().Should().Be(1);
            result.Attachments.First().AttachmentName.Should().Be("Test");
            result.Attachments.First().MimeType.Should().Be("Test");
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CreateEmailModel_Null_Email_Throws_Exception()
        {
            //Act
            var sut = EmailFactory.CreateEmailModel(null);
        }

        [Test]
        public void CreateEmailModel_Email_Returns_EmailModel()
        {
            //Arrange
            var timestamp = DateTime.UtcNow;

            var email = new Core.Objects.Email
            {
                EmailAddresses =
                    new List<EmailAddress>
                    {
                        new EmailAddress
                        {
                            Name = "Kristian Wilson",
                            Email = "kristian.wilson@gmail.com",
                            Type = EmailAddressType.To
                        }
                    },
                Subject = "Test Subject",
                PlainTextBody = "Plain Text",
                HtmlBody = "Html Body",
                Date = timestamp,
                Attachments =
                    new List<Core.Objects.Attachment>
                    {
                        new Core.Objects.Attachment {AttachmentName = "Test", MimeType = "Test"}
                    }
            };

            //Act
            var result = EmailFactory.CreateEmailModel(email);

            //Assert   
            result.ToAddresses.Should().Be("Kristian Wilson <kristian.wilson@gmail.com>");
            result.Subject.Should().Be(email.Subject);
            result.PlainTextBody.Should().Be(email.PlainTextBody);
            result.HtmlBody.Should().Be(email.HtmlBody);
            result.EmailDate.Should().Be(timestamp);
            result.Attachments.Should().NotBeEmpty();
            result.Attachments.Count().Should().Be(1);
            result.Attachments.First().AttachmentName.Should().Be("Test");
            result.Attachments.First().MimeType.Should().Be("Test");
        }
    }
}