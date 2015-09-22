using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Core.Rules;

namespace TransactionalEmail.Tests.UnitTests.Rules
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class SubjectRule_TestFixture
    {
        private Mock<IEmailAddressValidator> _validatorMock;
        private IForwardingRule _rule;

        [SetUp]
        public void Setup()
        {
            _validatorMock = new Mock<IEmailAddressValidator>();
            _rule = new SubjectRule(_validatorMock.Object);
        }

        [Test]
        public void Subject_Is_Valid_Email_Address_Rule_Is_Applied()
        {
            //arrange
            const string subject = "james.benamor@rgroup.co.uk";

            var email = new Email
            {
                Subject = subject,
                EmailAddresses =
                    new List<EmailAddress>
                    {
                        new EmailAddress {Name = "Test", Email = "test@test.com", Type = EmailAddressType.From}
                    }
            };

            _validatorMock.Setup(x => x.IsValidEmail(subject)).Returns(true);

            //act
            var result = _rule.ApplyRule(email);

            //assert
            result.RuleApplied.Should().BeTrue();
        }

        [Test]
        public void Subject_Is_Not_Valid_Email_Address_Rule_Is_Not_Applied()
        {
            //arrange
            const string subject = "The quick brown fox";
            var email = new Email { Subject = subject };

            _validatorMock.Setup(x => x.IsValidEmail(subject)).Returns(false);

            //act
            var result = _rule.ApplyRule(email);

            //assert
            result.RuleApplied.Should().BeFalse();
        }
    }
}