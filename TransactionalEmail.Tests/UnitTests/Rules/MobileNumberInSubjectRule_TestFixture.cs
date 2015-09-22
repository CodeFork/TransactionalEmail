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
    public class MobileNumberInSubjectRule_TestFixture
    {
        private IForwardingRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new MobileNumberInSubjectRule();
        }

        [Test]
        public void Subject_Contains_Mobile_Number_Rule_Is_Applied()
        {
            //arrange
            const string subject = "Dynmark MMS Alert Service. To 447717989072, from 447873128112, on 2014-07-28 16:20 (Monday, 28 July at 16:20)- M";

            var email = new Email
            {
                Subject = subject,
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
            const string subject = "The quick brown fox";
            var email = new Email { Subject = subject };

            //act
            var result = _rule.ApplyRule(email);

            //assert
            result.RuleApplied.Should().BeFalse();
        }
    }
}
