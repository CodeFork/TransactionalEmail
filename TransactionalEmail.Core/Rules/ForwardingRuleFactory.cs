using System;
using System.Collections.Generic;
using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;

namespace TransactionalEmail.Core.Rules
{
    public class ForwardingRuleFactory : IForwardingRuleFactory
    {
        private readonly IEmailAddressValidator _emailAddressValidator;
        public ForwardingRuleFactory(IEmailAddressValidator emailAddressValidator)
        {
            Check.If(emailAddressValidator).IsNotNull();

            _emailAddressValidator = emailAddressValidator;
        }

        public List<IForwardingRule> GetRules()
        {
            return new List<IForwardingRule>
            {
                new MobileNumberInSubjectRule(),
                new MobileNumberInBodyRule(),
                new SubjectRule(_emailAddressValidator),
            };
        }
    }
}
