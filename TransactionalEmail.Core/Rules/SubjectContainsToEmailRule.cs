using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Rules
{
    public class SubjectContainsToEmailRule : IForwardingRule
    {
        private readonly IEmailAddressValidator _emailAddressValidator;

        public SubjectContainsToEmailRule(IEmailAddressValidator emailAddressValidator)
        {
            Check.If(emailAddressValidator).IsNotNull();

            _emailAddressValidator = emailAddressValidator;
        }

        public string RuleName => "SubjectContainsEmailRule";

        public RuleResult ApplyRule(Email email)
        {
            var emailAddress = GetEmailAddressFromSubject(email.Subject);
            if (!string.IsNullOrEmpty(emailAddress))
            {
                return new RuleResult
                {
                    RuleName = RuleName,
                    RuleApplied = true,
                    Email = ForwardEmailFromAutoMessaging(email, emailAddress)
                };
            }

            return new RuleResult
            {
                RuleName = RuleName,
                RuleApplied = false,
                Email = email
            };
        }

        private string GetEmailAddressFromSubject(string subject)
        {
            //Emails from customer.io come in with a subject of
            //"[tenantemail@foo.com] The actual subject line"
            var email = subject.Substring(1).Split(']')[0];
            var validEmail = _emailAddressValidator.IsValidEmail(email);

            return validEmail ? email : string.Empty;
        }

        private Email ForwardEmailFromAutoMessaging(Email email, string tenantEmailAddress)
        {
            var result = email;
            var subject = email.Subject.Split(']')[1];

            var fromAddress = result.FromAddress;
            result.Subject = subject;
            result.EmailAddresses.Clear();
            result.EmailAddresses.Add(fromAddress);
            result.EmailAddresses.Add(new EmailAddress
            {
                Email = tenantEmailAddress,
                Type = EmailAddressType.To
            });

            return result;
        }
    }
}
