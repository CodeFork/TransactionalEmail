using System;
using TransactionalEmail.Core.Interfaces;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail.Core.Rules
{
    public class MobileNumberInSubjectRule : IForwardingRule
    {
        private const string Prefix = ", from ";
        private const string Postfix = ", on ";

        public string RuleName => "Mobile Number In SUbject Rule";

        public RuleResult ApplyRule(Email email)
        {
            if (IsPhoneNumberInSubject(email.Subject))
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
                Email = email
            };
        }

        private static Email ForwardEmail(Email email)
        {
            var result = email;

            email.FromAddress.Email = email.FromAddress.Name = GetPhoneNumber(email.Subject, Prefix, Postfix);

            return result;
        }

        private static bool IsPhoneNumberInSubject(string subject)
        {
            return subject.Contains(Prefix);
        }

        private static string GetPhoneNumber(string subject, string prefix, string postfix)
        {
            var startIndex = subject.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) + prefix.Length;
            var endIndex = subject.IndexOf(postfix, StringComparison.OrdinalIgnoreCase);

            return subject.Substring(startIndex, (endIndex - startIndex)).Trim();
        }
    }
}
