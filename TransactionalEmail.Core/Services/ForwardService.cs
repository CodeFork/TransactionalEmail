using System.Collections.Generic;
using System.Linq;
using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Services
{
    public class ForwardService : IForwardService
    {
        private readonly IForwardingRuleFactory _forwardingRuleFactory;

        public ForwardService(IForwardingRuleFactory forwardingRuleFactory)
        {
            Check.If(forwardingRuleFactory).IsNotNull();

            _forwardingRuleFactory = forwardingRuleFactory;
        }

        public ForwardResult ProcessEmail(Email email)
        {
            foreach (
                var result in
                    _forwardingRuleFactory.GetRules()
                        .Select(rule => rule.ApplyRule(email))
                        .Where(result => result.RuleApplied))
            {
                return new ForwardResult
                {
                    RuleApplied = result.RuleName,
                    EmailResult = result.Email,
                };
            }

            return new ForwardResult {EmailResult = email};
        }
    }
}
