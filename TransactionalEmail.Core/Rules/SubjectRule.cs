using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Rules
{
    public class SubjectRule : IForwardingRule
    {
        private readonly IEmailAddressValidator _emailAddressValidator;

        public SubjectRule(IEmailAddressValidator emailAddressValidator)
        {
            Check.If(emailAddressValidator).IsNotNull();

            _emailAddressValidator = emailAddressValidator;
        }

        public string RuleName => "Subject Rule";

        public RuleResult ApplyRule(Email email)
        {
            if (IsValidEmailAddress(email.Subject))
            {
                return new RuleResult
                {
                    RuleName = RuleName,
                    RuleApplied = true,
                    Email = ForwardEmail(email),
                };
            }

            return new RuleResult
            {
                RuleName = RuleName,
                RuleApplied = false,
                Email = email,
            };
        }

        private bool IsValidEmailAddress(string subject)
        {
            return !string.IsNullOrEmpty(subject) && _emailAddressValidator.IsValidEmail(subject);
        }

        private static Email ForwardEmail(Email email)
        {
            var result = email;
            var subject = $"Forwarded Message from {email.Subject}";

            email.FromAddress.Name = email.FromAddress.Email = email.Subject;
            result.Subject = subject;

            return result;
        }
    }
}
