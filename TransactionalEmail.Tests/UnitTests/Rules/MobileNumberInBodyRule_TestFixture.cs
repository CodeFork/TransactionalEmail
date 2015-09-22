using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Core.Rules;

namespace TransactionalEmail.Tests.UnitTests.Rules
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class MobileNumberInBodyRule_TestFixture
    {
        private IForwardingRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new MobileNumberInBodyRule();
        }

        [Test]
        public void Subject_Contains_Mobile_Number_Rule_Is_Applied()
        {
            //arrange
            const string body = "Message sent in by 07532 451297 to 447717989072 on Thursday, 21st March at 19:09:32. Message content follows: You've been sent a picture message by 447532451297 open it at http://www.vodafone.co.uk/getmyphoto your password is rfb51zsy";

            var email = new Email
            {
                PlainTextBody = body,
                EmailAddresses =
                    new List<EmailAddress>
                    {
                        new EmailAddress {Name = "Test", Email = "test@test.com", Type = EmailAddressType.From}
                    }
            };

            //act
            var result = _rule.ApplyRule(email);

            //assert
            result.RuleApplied.Should().BeTrue();
        }

        [Test]
        public void Subject_Does_Not_Contain_Mobile_Number_Rule_Is_Not_Applied()
        {
            //arrange
            const string body = "The quick brown fox";
            var email = new Email { PlainTextBody = body };

            //act
            var result = _rule.ApplyRule(email);

            //assert
            result.RuleApplied.Should().BeFalse();
        }
    }
}