using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;
using TransactionalEmail.Core.Services;

namespace TransactionalEmail.Tests.UnitTests.Services
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class ForwardService_TestFixture
    {
        private Mock<IForwardingRuleFactory> _mockForwardingRuleFactory;
        private Mock<IForwardingRule> _mockRule1;
        private Mock<IForwardingRule> _mockRule2;
        private Mock<IForwardingRule> _mockRule3;
        private IForwardService _forwardService;

        [SetUp]
        public void Setup()
        {
            _mockRule1 = new Mock<IForwardingRule>();
            _mockRule2 = new Mock<IForwardingRule>();
            _mockRule3 = new Mock<IForwardingRule>();
            _mockForwardingRuleFactory = new Mock<IForwardingRuleFactory>();

            _mockForwardingRuleFactory.Setup(x => x.GetRules())
                .Returns(new List<IForwardingRule> {_mockRule1.Object, _mockRule2.Object, _mockRule3.Object});

            _forwardService = new ForwardService(_mockForwardingRuleFactory.Object);
        }

        [Test]
        public void All_Rules_Are_Tried_If_No_Rules_Apply()
        {
            //arrange
            var email = new Email();

            _mockRule1.Setup(x => x.ApplyRule(email)).Returns(new RuleResult { RuleApplied = false });
            _mockRule2.Setup(x => x.ApplyRule(email)).Returns(new RuleResult { RuleApplied = false });
            _mockRule3.Setup(x => x.ApplyRule(email)).Returns(new RuleResult { RuleApplied = false });

            //act
            _forwardService.ProcessEmail(email);

            //assert
            _mockRule1.Verify(x => x.ApplyRule(email), Times.Once());
            _mockRule2.Verify(x => x.ApplyRule(email), Times.Once());
            _mockRule3.Verify(x => x.ApplyRule(email), Times.Once());
        }

        [Test]
        public void All_Rules_After_First_Rule_That_Applies_Are_Ignored()
        {
            //arrange
            var email = new Email();

            _mockRule1.Setup(x => x.ApplyRule(email)).Returns(new RuleResult { RuleApplied = true });
            _mockRule2.Setup(x => x.ApplyRule(email)).Returns(new RuleResult { RuleApplied = false });
            _mockRule3.Setup(x => x.ApplyRule(email)).Returns(new RuleResult { RuleApplied = false });

            //act
            _forwardService.ProcessEmail(email);

            //assert
            _mockRule1.Verify(x => x.ApplyRule(email), Times.Once());
            _mockRule2.Verify(x => x.ApplyRule(email), Times.Never());
            _mockRule3.Verify(x => x.ApplyRule(email), Times.Never());
        }

        [Test]
        public void Rules_Are_Tried_Until_Rule_Applies_And_Rest_Are_Ignored()
        {
            //arrange
            var email = new Email();

            _mockRule1.Setup(x => x.ApplyRule(email)).Returns(new RuleResult { RuleApplied = false });
            _mockRule2.Setup(x => x.ApplyRule(email)).Returns(new RuleResult { RuleApplied = true });
            _mockRule3.Setup(x => x.ApplyRule(email)).Returns(new RuleResult { RuleApplied = false });

            //act
            _forwardService.ProcessEmail(email);

            //assert
            _mockRule1.Verify(x => x.ApplyRule(email), Times.Once());
            _mockRule2.Verify(x => x.ApplyRule(email), Times.Once());
            _mockRule3.Verify(x => x.ApplyRule(email), Times.Never());
        }
    }
}